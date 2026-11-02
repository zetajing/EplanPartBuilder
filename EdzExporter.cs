using System.Diagnostics;
using System.Security.Cryptography;
using System.Text;
using System.Xml.Linq;

namespace EplanPartBuilder;

public sealed class ExportResult
{
    public string Path { get; set; } = "";
    public int AssetCount { get; set; }
    public string Notes { get; set; } = "";
}

public static class EdzExporter
{
    public static ExportResult Export(PartProject project, string destination, IReadOnlyList<FunctionDefinition> catalog)
    {
        List<string> errors = PartValidator.Validate(project, catalog);
        if (errors.Count > 0) throw new InvalidDataException(string.Join(Environment.NewLine, errors));
        if (!Path.GetExtension(destination).Equals(".edz", StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("输出文件扩展名必须为 .edz。");
        if (project.Package != null)
        {
            // 防止把输出写进输入目录或原始 EDZ，保留可回退的原包。
            string full = Path.GetFullPath(destination);
            string original = Path.GetFullPath(project.Package.OriginalSource);
            if (full.Equals(original, StringComparison.OrdinalIgnoreCase)
                || full.StartsWith(original + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                || full.StartsWith(Path.GetFullPath(project.Package.Folder) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
                throw new InvalidDataException("请选择原包和快照目录以外的新 EDZ 输出位置。");
            return PackageEditor.Export(project, destination);
        }

        string workRoot = Path.Combine(Path.GetTempPath(), "EplanPartBuilder");
        string stage = Path.Combine(workRoot, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(stage);
        try
        {
            // 文件夹中加入编号散列，防止不同部件的同名宏在导入时互相覆盖。
            string folder = "PartBuilder\\" + SafeName(project.Manufacturer) + "\\" + StableName(project.PartNumber);
            var references = new Dictionary<string, string>();
            var items = new XElement("items");
            int assets = 0;
            CopyAsset("schematic", project.SchematicMacro, "macro", "groupsymbolmacro", "$(MD_MACROS)");
            CopyAsset("layout", project.LayoutMacro, "macro", "graphicmacro", "$(MD_MACROS)");
            CopyAsset("picture", project.Picture, "picture", "picturefile", "$(MD_IMG)");
            CopyAsset("document", project.Document, "document", "externalDocument1", "$(MD_DOCUMENTS)");
            CopyAsset("step", project.StepFile, "document", "externalDocument2", "$(MD_DOCUMENTS)");

            string partFile = StableName(project.PartNumber) + ".part.xml";
            string partXmlPath = Path.Combine(stage, "items", "partxml", partFile);
            Directory.CreateDirectory(Path.GetDirectoryName(partXmlPath)!);
            EplanXmlWriter.Save(EplanXmlWriter.Create(project, references), partXmlPath);
            items.AddFirst(Item("partxml", "part", partFile));

            string manufacturerFile = StableName(project.Manufacturer) + ".manufacturer.xml";
            var manufacturer = new XDocument(new XElement("partsmanagement",
                new XAttribute("count", "1"), new XAttribute("type", "EPLAN.PartsManagement"),
                new XElement("address", new XAttribute("class", "12"),
                    new XAttribute("P_PART_ADDRESS_SHORTNAME", project.Manufacturer.Trim()),
                    new XAttribute("P_PART_ADDRESS_LONGNAME", project.ManufacturerName.Length == 0 ? project.Manufacturer.Trim() : project.ManufacturerName),
                    new XAttribute("P_PART_ADDRESS_NAME1", project.ManufacturerCompany),
                    new XAttribute("P_PART_ADDRESS_EMAIL", project.ManufacturerEmail),
                    new XAttribute("P_PART_ADDRESS_PHONE", project.ManufacturerPhone))));
            EplanXmlWriter.Save(manufacturer, Path.Combine(stage, "items", "partxml", manufacturerFile));
            items.Add(Item("partxml", "manufacturer", manufacturerFile));

            var manifest = new XDocument(new XElement("manifest", new XAttribute("version", "2.0"),
                new XElement("packages", new XElement("package", new XAttribute("type", "part"),
                    new XAttribute("key", project.PartNumber.Trim()),
                    new XAttribute("name", project.PartNumber.Trim()), items))));
            EplanXmlWriter.Save(manifest, Path.Combine(stage, "manifest.xml"));
            VerifyManifest(stage, manifest);

            // EDZ 样包使用 7z 容器。明确使用老版本可读取的 LZMA，不能只把 ZIP 改后缀。
            AtomicFile.Write(destination, temp =>
            {
                RunSevenZip(stage, "a", "-t7z", "-m0=LZMA", "-md=4k", "-ms=off", "-mhc=off", "-mx=3", temp, "manifest.xml", "items");
                RunSevenZip(stage, "t", "-t7z", temp);
            });

            string notes = string.IsNullOrWhiteSpace(project.StepFile) ? "" : "STEP 已作为源文件附件打包，未转换成 3D 宏。";
            if (string.IsNullOrWhiteSpace(project.LayoutMacro)) notes += " 尚未提供可用于布局的 3D / 安装宏。";
            if (project.Functions.Count == 0) notes += " 未填写功能模板；可在 EPLAN 内从原理图宏分配。";
            return new ExportResult { Path = Path.GetFullPath(destination), AssetCount = assets, Notes = notes.Trim() };

            void CopyAsset(string role, string source, string type, string name, string variable)
            {
                if (string.IsNullOrWhiteSpace(source)) return;
                // 按用途重命名：即使两个输入都叫 part.ema，也不会发生覆盖。
                string locator = folder + "\\" + role + Path.GetExtension(source).ToLowerInvariant();
                string target = Path.Combine(stage, "items", type, locator);
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.Copy(source, target, false);
                references.Add(role, variable + "\\" + locator);
                items.Add(Item(type, name, locator));
                assets++;
            }
        }
        finally
        {
            // 只清理本次创建的随机临时目录，不触碰用户提供的任何输入文件。
            string resolved = Path.GetFullPath(stage);
            if (resolved.StartsWith(Path.GetFullPath(workRoot) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
                && Directory.Exists(resolved)) Directory.Delete(resolved, true);
        }
    }

    public static string RunSevenZip(string workingDirectory, params string[] arguments)
    {
        string executable = Path.Combine(AppContext.BaseDirectory, "Tools", "7zr.exe");
        if (!File.Exists(executable)) throw new FileNotFoundException("缺少打包工具，请完整解压软件文件夹。", executable);
        var start = new ProcessStartInfo(executable)
        {
            WorkingDirectory = workingDirectory, UseShellExecute = false, CreateNoWindow = true,
            RedirectStandardOutput = true, RedirectStandardError = true
        };
        // ArgumentList 单独传参，中文路径、空格和特殊字符不会被解释成命令。
        foreach (string argument in arguments) start.ArgumentList.Add(argument);
        using var process = Process.Start(start) ?? throw new IOException("无法启动打包工具。");
        Task<string> output = process.StandardOutput.ReadToEndAsync();
        Task<string> error = process.StandardError.ReadToEndAsync();
        if (!process.WaitForExit(300000))
        {
            process.Kill(true);
            process.WaitForExit();
            throw new TimeoutException("打包超过 5 分钟，请检查文件大小和磁盘状态。");
        }
        string log = output.GetAwaiter().GetResult() + error.GetAwaiter().GetResult();
        if (process.ExitCode != 0) throw new IOException("打包工具返回错误：" + log);
        return log;
    }

    public static void VerifyManifest(string folder, XDocument manifest)
    {
        // 检查清单中每个文件确实存在，并限制引用路径位于打包目录中。
        string root = Path.GetFullPath(folder) + Path.DirectorySeparatorChar;
        foreach (var item in manifest.Descendants("item"))
        {
            string path = PackageStore.ItemPath(folder, item);
            if (!path.StartsWith(root, StringComparison.OrdinalIgnoreCase) || !File.Exists(path))
                throw new InvalidDataException("包内文件引用无效：" + item.Attribute("locator")?.Value);
        }
    }

    private static XElement Item(string type, string name, string locator)
        => new("item", new XAttribute("type", type), new XAttribute("name", name), new XAttribute("locator", locator));

    private static string StableName(string text)
    {
        string hash = Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(text.Trim())))[..12];
        return SafeName(text) + "_" + hash;
    }

    private static string SafeName(string text)
    {
        string name = new(text.Trim().Select(c => char.IsAsciiLetterOrDigit(c) || c is '-' or '_' or '.' ? c : '_').ToArray());
        name = name.Trim('.');
        if (name.Length == 0) name = "part";
        return name.Length > 50 ? name[..50] : name;
    }
}
