using System.ComponentModel;
using System.Globalization;
using System.IO.Compression;
using System.Text.Json;
using System.Xml;
using System.Xml.Linq;

namespace EplanPartBuilder;

public static class PackageStore
{
    public const long MaxTotalBytes = 2L * 1024 * 1024 * 1024;
    private const int MaxFiles = 20000;
    private static readonly string SessionFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
        "EplanPartBuilder", "Packages", Guid.NewGuid().ToString("N"));

    static PackageStore()
    {
        // 已保存的资料位于用户选择的 .assets 目录；退出时只清理本进程的临时快照。
        AppDomain.CurrentDomain.ProcessExit += (_, _) =>
        {
            try { if (Directory.Exists(SessionFolder)) Directory.Delete(SessionFolder, true); }
            catch (Exception ex) when (ex is IOException or UnauthorizedAccessException) { }
        };
    }

    public static List<PartProject> Open(string source)
    {
        // 先复制到独立快照中。编辑和导出始终不修改桌面目录或输入 EDZ。
        string folder = Path.Combine(SessionFolder, Guid.NewGuid().ToString("N"));
        Directory.CreateDirectory(folder);
        try
        {
            if (Directory.Exists(source)) CopyDirectory(source, folder);
            else ExtractArchive(source, folder);
            var manifest = LoadXml(Path.Combine(folder, "manifest.xml"));
            EdzExporter.VerifyManifest(folder, manifest);
            var choices = new List<PartProject>();
            var packages = manifest.Root?.Element("packages")?.Elements("package").ToList()
                           ?? throw new InvalidDataException("没有找到 packages 清单。");
            for (int pi = 0; pi < packages.Count; pi++)
            {
                var package = packages[pi];
                if ((string?)package.Attribute("type") != "part") continue;
                var partItem = package.Element("items")?.Elements("item").SingleOrDefault(i => (string?)i.Attribute("name") == "part");
                if (partItem == null) continue;
                var document = LoadXml(ItemPath(folder, partItem));
                var parts = document.Root?.Elements("part").ToList() ?? new();
                for (int ai = 0; ai < parts.Count; ai++)
                {
                    var article = parts[ai];
                    if (Text(article, "P_ARTICLE_PARTNR") != (string?)package.Attribute("key")) continue;
                    if (Text(article, "P_ARTICLE_PARTTYPE") is not ("" or "1")) continue;
                    var variants = article.Elements("variant").ToList();
                    for (int vi = 0; vi < variants.Count; vi++)
                    {
                        PartProject model = ReadModel(article, variants[vi], package, folder);
                        model.Package = new PackageTemplate
                        {
                            Folder = folder, OriginalSource = Path.GetFullPath(source),
                            PackageIndex = pi, PartIndex = ai, VariantIndex = vi,
                            BaselineJson = JsonSerializer.Serialize(model)
                        };
                        choices.Add(model);
                    }
                }
            }
            if (choices.Count == 0) throw new InvalidDataException("包中没有可编辑的零部件变量，部件组和模块暂不支持编辑。");
            return choices;
        }
        catch
        {
            // folder 是本次创建的固定根目录下的随机子目录。
            Directory.Delete(folder, true);
            throw;
        }
    }

    public static PartProject Baseline(PartProject project)
        => JsonSerializer.Deserialize<PartProject>(project.Package!.BaselineJson)
           ?? throw new InvalidDataException("原包字段基线无效。");

    public static PartProject Persist(string path, PartProject project)
    {
        var saved = JsonSerializer.Deserialize<PartProject>(JsonSerializer.Serialize(project))!;
        if (saved.Package == null) return saved;
        string oldFolder = Path.GetFullPath(saved.Package.Folder);
        string assetsRoot = Path.GetFullPath(path) + ".assets";
        if (!oldFolder.StartsWith(assetsRoot + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
        {
            string newFolder = Path.Combine(assetsRoot, Guid.NewGuid().ToString("N"));
            CopyDirectory(oldFolder, newFolder);
            var baseline = Baseline(saved);
            RemapFiles(saved, oldFolder, newFolder);
            RemapFiles(baseline, oldFolder, newFolder);
            saved.Package.Folder = newFolder;
            saved.Package.BaselineJson = JsonSerializer.Serialize(baseline);
        }
        // 替换的新文件也复制进资料目录，不能只保存原电脑上的绝对路径。
        saved.SchematicMacro = KeepFile(saved.SchematicMacro);
        saved.LayoutMacro = KeepFile(saved.LayoutMacro);
        saved.Picture = KeepFile(saved.Picture);
        saved.Document = KeepFile(saved.Document);
        saved.StepFile = KeepFile(saved.StepFile);
        return saved;

        string KeepFile(string value)
        {
            if (!File.Exists(value) || Path.GetFullPath(value).StartsWith(saved.Package.Folder + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)) return value;
            string target = Path.Combine(saved.Package.Folder, "editor-files", Guid.NewGuid().ToString("N") + Path.GetExtension(value));
            Directory.CreateDirectory(Path.GetDirectoryName(target)!); File.Copy(value, target, false); return target;
        }
    }

    public static void RestoreSavedPaths(PartProject project, string jsonPath)
    {
        // 快照地址保存成相对路径，整个 JSON 与 .assets 文件夹一起移动后仍能打开。
        if (project.Package == null || Path.IsPathRooted(project.Package.Folder)) return;
        string oldFolder = project.Package.Folder;
        string resolved = Path.GetFullPath(Path.Combine(Path.GetDirectoryName(Path.GetFullPath(jsonPath))!, oldFolder));
        var baseline = Baseline(project);
        RemapFiles(project, oldFolder, resolved);
        RemapFiles(baseline, oldFolder, resolved);
        project.Package.Folder = resolved;
        project.Package.BaselineJson = JsonSerializer.Serialize(baseline);
    }

    public static void MakeSavedPathsRelative(PartProject project, string jsonPath)
    {
        if (project.Package == null) return;
        string oldFolder = project.Package.Folder;
        string relative = Path.GetRelativePath(Path.GetDirectoryName(Path.GetFullPath(jsonPath))!, oldFolder);
        var baseline = Baseline(project);
        RemapFiles(project, oldFolder, relative);
        RemapFiles(baseline, oldFolder, relative);
        project.Package.Folder = relative;
        project.Package.BaselineJson = JsonSerializer.Serialize(baseline);
    }

    private static void RemapFiles(PartProject model, string from, string to)
    {
        model.SchematicMacro = Remap(model.SchematicMacro);
        model.LayoutMacro = Remap(model.LayoutMacro);
        model.Picture = Remap(model.Picture);
        model.Document = Remap(model.Document);
        model.StepFile = Remap(model.StepFile);
        string Remap(string value) => value.StartsWith(from + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase)
            ? Path.Combine(to, value[(from.Length + 1)..]) : value;
    }

    private static PartProject ReadModel(XElement part, XElement variant, XElement package, string folder)
    {
        var model = new PartProject
        {
            PartNumber = Text(part, "P_ARTICLE_PARTNR"), Manufacturer = Text(part, "P_ARTICLE_MANUFACTURER"),
            TypeNumber = Text(part, "P_ARTICLE_TYPENR"), OrderNumber = Text(part, "P_ARTICLE_ORDERNR"),
            Name = DisplayText(Text(part, "P_ARTICLE_DESCR1")), Description = DisplayText(Text(part, "P_ARTICLE_NOTE")),
            Variant = Text(variant, "P_ARTICLE_VARIANT"), Characteristics = Text(variant, "P_ARTICLE_CHARACTERISTICS"),
            ProductGroup = int.TryParse(Text(part, "P_ARTICLE_PRODUCTGROUP"), out int group) ? group : 0,
            Width = Dimension(part, "P_ARTICLE_WIDTH"), Height = Dimension(part, "P_ARTICLE_HEIGHT"), Depth = Dimension(part, "P_ARTICLE_DEPTH"),
            SchematicMacro = Asset("groupsymbolmacro", "P_ARTICLE_GROUPSYMBOLMACRO"),
            LayoutMacro = Asset("graphicmacro", "P_ARTICLE_MACRO"), Picture = Asset("picturefile", "P_ARTICLE_PICTUREFILE"),
            Document = Asset("externalDocument1", "P_ARTICLE_EXTERNAL_DOCUMENT_1"), StepFile = Asset("externalDocument2", "P_ARTICLE_EXTERNAL_DOCUMENT_2"),
            RatedCurrent = Text(variant, "P_ARTICLE_ELECTRICALCURRENT"), Voltage = Text(variant, "P_ARTICLE_VOLTAGE"),
            VoltageType = Text(variant, "P_ARTICLE_VOLTAGETYPE"), PowerDissipation = Text(variant, "P_ARTICLE_POWERDISSIPATION"),
            CrossSection = Text(variant, "P_ARTICLE_CONNECTIONCROSSSECTION")
        };
        var manufacturerItem = package.Element("items")?.Elements("item").FirstOrDefault(i => (string?)i.Attribute("name") == "manufacturer");
        if (manufacturerItem != null)
        {
            var address = LoadXml(ItemPath(folder, manufacturerItem)).Descendants("address")
                .FirstOrDefault(a => Text(a, "P_PART_ADDRESS_SHORTNAME") == model.Manufacturer);
            if (address != null)
            {
                model.ManufacturerName = Text(address, "P_PART_ADDRESS_LONGNAME");
                model.ManufacturerCompany = Text(address, "P_PART_ADDRESS_NAME1");
                model.ManufacturerEmail = Text(address, "P_PART_ADDRESS_EMAIL");
                model.ManufacturerPhone = Text(address, "P_PART_ADDRESS_PHONE");
            }
        }
        foreach (var function in variant.Elements("functiontemplate")) model.Functions.Add(ReadFunction(function));
        return model;

        string Asset(string name, string property)
        {
            var item = package.Element("items")?.Elements("item").FirstOrDefault(i => (string?)i.Attribute("name") == name);
            return item == null ? Text(part, property) : ItemPath(folder, item);
        }
    }

    public static FunctionRow ReadFunction(XElement element) => new()
    {
        SourceXml = element.ToString(SaveOptions.DisableFormatting),
        Code = Text(element, "functiondefcategory") + "." + Text(element, "functiondefgroup") + "." + Text(element, "functiondefid"),
        Connections = Text(element, "connectionDesignation").Replace("\n", "|"),
        ConnectionDescriptions = Text(element, "connectiondescription").Replace("\n", "|"),
        Plug = Text(element, "terminalNr"), Channel = Text(element, "idx1"),
        Characteristics = Text(element, "characteristics"), Symbol = Text(element, "symbol")
    };

    public static string DisplayText(string value)
    {
        var segments = value.Split(';');
        string? chinese = segments.FirstOrDefault(s => s.StartsWith("zh_CN@"));
        if (chinese != null) return chinese[6..].Replace("@@", "@");
        var first = segments.FirstOrDefault(s => s.Length >= 6 && s[5] == '@');
        return first == null ? value : first[6..].Replace("@@", "@");
    }

    public static string Text(XElement element, string name) => (string?)element.Attribute(name) ?? "";
    private static decimal Dimension(XElement element, string name)
    {
        string text = Text(element, name);
        if (text.Length == 0) return 0;
        if (!decimal.TryParse(text, NumberStyles.Float, CultureInfo.InvariantCulture, out decimal value) || value < 0 || value > 1000000)
            throw new InvalidDataException("尺寸无法显示：" + name + "=" + text);
        return value;
    }

    public static XDocument LoadXml(string path)
    {
        using var reader = XmlReader.Create(path, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = 64L * 1024 * 1024 });
        return XDocument.Load(reader, LoadOptions.PreserveWhitespace);
    }

    public static string ItemPath(string folder, XElement item)
    {
        string type = (string?)item.Attribute("type") ?? "";
        if (type.Length == 0 || type.Any(c => !char.IsAsciiLetterOrDigit(c) && c is not ('_' or '-')))
            throw new InvalidDataException("清单文件类型无效。");
        return SafePath(folder, "items\\" + type + "\\" + ((string?)item.Attribute("locator") ?? ""));
    }

    public static string SafePath(string root, string relative)
    {
        string normalized = relative.Replace('/', '\\');
        if (string.IsNullOrWhiteSpace(normalized) || Path.IsPathRooted(normalized)
            || normalized.Split('\\').Any(s => s is ".." or "." || s.Contains(':')))
            throw new InvalidDataException("包内路径不安全：" + relative);
        string full = Path.GetFullPath(Path.Combine(root, normalized));
        if (!full.StartsWith(Path.GetFullPath(root) + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("包内路径超出目录：" + relative);
        return full;
    }

    public static void CopyDirectory(string source, string target)
    {
        source = Path.GetFullPath(source); target = Path.GetFullPath(target);
        if (target.Equals(source, StringComparison.OrdinalIgnoreCase) || target.StartsWith(source + Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException("快照目标不能位于源目录中。");
        long total = 0; int count = 0;
        Copy(source, target);
        void Copy(string from, string to)
        {
            if ((File.GetAttributes(from) & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("部件包中不支持目录链接。");
            Directory.CreateDirectory(to);
            foreach (string entry in Directory.EnumerateFileSystemEntries(from))
            {
                FileAttributes attributes = File.GetAttributes(entry);
                if ((attributes & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("部件包中不支持文件或目录链接。");
                string destination = SafePath(to, Path.GetFileName(entry));
                if ((attributes & FileAttributes.Directory) != 0) Copy(entry, destination);
                else
                {
                    total += new FileInfo(entry).Length;
                    if (++count > MaxFiles || total > MaxTotalBytes) throw new InvalidDataException("部件包超过 20000 文件或 2 GiB 限制。");
                    File.Copy(entry, destination, false);
                }
            }
        }
    }

    public static void ExtractArchive(string source, string folder)
    {
        byte[] signature = new byte[6];
        using (var stream = File.OpenRead(source)) stream.ReadExactly(signature);
        if (signature[0] == 'P' && signature[1] == 'K')
        {
            using var zip = ZipFile.OpenRead(source);
            long size = 0;
            var paths = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var entry in zip.Entries)
            {
                string path = SafePath(folder, entry.FullName);
                if (!paths.Add(path) || ((entry.ExternalAttributes >> 16) & 0xF000) == 0xA000)
                    throw new InvalidDataException("压缩包中包含重复路径或链接。");
                size += entry.Length;
                if (paths.Count > MaxFiles || size > MaxTotalBytes) throw new InvalidDataException("压缩包展开大小超过限制。");
            }
            foreach (var entry in zip.Entries)
            {
                string path = SafePath(folder, entry.FullName);
                if (entry.FullName.EndsWith('/') || entry.FullName.EndsWith('\\')) { Directory.CreateDirectory(path); continue; }
                Directory.CreateDirectory(Path.GetDirectoryName(path)!);
                entry.ExtractToFile(path, false);
            }
            return;
        }
        if (!signature.SequenceEqual(new byte[] { 0x37, 0x7a, 0xbc, 0xaf, 0x27, 0x1c }))
            throw new InvalidDataException("不支持此 EDZ 容器；请选择 7z 或 ZIP 格式的 EDZ。");

        // 先读取目录校验路径、链接和展开大小，再调用 7zr 解包。
        string listing = EdzExporter.RunSevenZip(folder, "l", "-slt", "-ba", "-sccUTF-8", Path.GetFullPath(source));
        long totalSize = 0; int fileCount = 0;
        var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        foreach (string block in listing.Replace("\r\n", "\n").Split("\n\n", StringSplitOptions.RemoveEmptyEntries))
        {
            var values = new Dictionary<string, string>();
            foreach (string line in block.Split('\n'))
            {
                int at = line.IndexOf(" = ", StringComparison.Ordinal);
                if (at > 0) values[line[..at]] = line[(at + 3)..];
            }
            if (!values.TryGetValue("Path", out string? path)) continue;
            if (!seen.Add(SafePath(folder, path)) || values.ContainsKey("Symbolic Link") || values.ContainsKey("Hard Link")
                || (values.GetValueOrDefault("Attributes", "").Contains('L')))
                throw new InvalidDataException("压缩包中包含重复路径或链接。");
            if (!long.TryParse(values.GetValueOrDefault("Size", "0"), out long size) || size < 0)
                throw new InvalidDataException("压缩包大小信息无效。");
            totalSize += size;
            if (++fileCount > MaxFiles || totalSize > MaxTotalBytes) throw new InvalidDataException("压缩包展开大小超过限制。");
        }
        if (fileCount == 0) throw new InvalidDataException("无法读取压缩包文件清单。");
        EdzExporter.RunSevenZip(folder, "x", "-y", "-o" + Path.GetFullPath(folder), Path.GetFullPath(source));
        foreach (string entry in Directory.EnumerateFileSystemEntries(folder, "*", SearchOption.AllDirectories))
            if ((File.GetAttributes(entry) & FileAttributes.ReparsePoint) != 0) throw new InvalidDataException("解包结果中包含链接。");
    }
}
