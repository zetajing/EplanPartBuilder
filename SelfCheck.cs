using System.ComponentModel;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Xml.Linq;

namespace EplanPartBuilder;

public static class SelfCheck
{
    public static int Run(string outputFolder)
    {
        string folder = Path.GetFullPath(outputFolder);
        Directory.CreateDirectory(folder);
        var checks = new List<string>();
        var catalog = FunctionCatalog.Load();
        try
        {
            Check(catalog.Count >= 1300 && catalog.Any(c => c.Code == "300.1.2" && c.Name.Contains("数字输入")), "官方功能定义表加载与 PLC 编号");
            var project = PartProject.RelayExample();
            Check(PartValidator.Validate(project, catalog).Count == 0, "继电器功能模板基本校验");
            project.PartNumber = "TEST.继电器<&>";
            project.Name = "继电器 <测试> & \"引号\" @ 24V";
            project.OrderNumber = "A&B\"C";
            project.Functions[0].ConnectionDescriptions = "+|-";
            project.Functions[0].Plug = "X1";
            project.Functions[0].Channel = "CH0";
            project.Width = 12.25m;
            string xmlFile = Path.Combine(folder, "special-chars.xml");
            EplanXmlWriter.Save(EplanXmlWriter.Create(project, new Dictionary<string, string>()), xmlFile);
            var xml = XDocument.Load(xmlFile);
            Check(xml.Root?.Name == "partsmanagement" && xml.Root.Elements("part").Count() == 1 && xml.Descendants("variant").Count() == 1, "部件 XML 层级与单变量结构");
            var article = xml.Descendants("part").Single();
            Check(article.Attribute("P_ARTICLE_PARTNR")?.Value == project.PartNumber && article.Attribute("P_ARTICLE_ORDERNR")?.Value == project.OrderNumber && article.Attribute("P_ARTICLE_DESCR1")!.Value.Contains("@@"), "中文、XML 特殊字符与多语言 @ 转义");
            Check(article.Attribute("P_ARTICLE_WIDTH")?.Value == "12.25", "尺寸使用固定小数点格式");
            var coil = xml.Descendants("functiontemplate").First();
            Check(coil.Attribute("connectionDesignation")?.Value == "A1\nA2" && coil.Attribute("connectiondescription")?.Value == "+\n-", "连接点与描述换行往返保留");
            Check(coil.Attribute("terminalNr")?.Value == "X1" && coil.Attribute("idx1")?.Value == "CH0", "PLC 插头与通道字段映射");

            project.Functions.Add(new FunctionRow { Code = "201.1.2", Connections = "A1|A3", Plug = "X1" });
            Check(PartValidator.Validate(project, catalog).Any(e => e.Contains("重复")), "同一插头重复端子被拒绝");
            project.Functions[^1].Plug = "X2";
            Check(PartValidator.Validate(project, catalog).Count == 0, "不同插头允许复用端子号");
            project.Functions.RemoveAt(project.Functions.Count - 1);
            project.Functions[0].Code = "99999.1.1";
            Check(PartValidator.Validate(project, catalog).Any(e => e.Contains("功能定义")), "未知功能编号被拒绝");
            project.Functions[0].Code = "200.1.1";
            project.Functions[0].Connections = "A1||A2";
            Check(PartValidator.Validate(project, catalog).Any(e => e.Contains("空项")), "连接点空项被拒绝");
            project.Functions[0].Connections = "A1|A2";

            string saved = Path.Combine(folder, "中文 空格.epart.json");
            ProjectStore.Save(saved, project);
            var restored = ProjectStore.Load(saved);
            Check(restored.PartNumber == project.PartNumber && restored.Functions[0].Connections == "A1|A2", "资料 JSON 保存与恢复");
            string broken = Path.Combine(folder, "broken.json");
            File.WriteAllText(broken, "{\"Name\":null}");
            bool rejected = false;
            try { ProjectStore.Load(broken); } catch (InvalidDataException) { rejected = true; }
            Check(rejected, "损坏资料中的 null 字段被拒绝");

            // 这些是专门的字节测试样本，不是 EPLAN 宏，不会作为用户示例交付。
            string input = Path.Combine(folder, "输入 空格");
            Directory.CreateDirectory(Path.Combine(input, "2D"));
            Directory.CreateDirectory(Path.Combine(input, "3D"));
            string schematic = Path.Combine(input, "2D", "相同名字.ema");
            string layout = Path.Combine(input, "3D", "相同名字.ema");
            string step = Path.Combine(input, "源模型.stp");
            string pdf = Path.Combine(input, "手册.pdf");
            string picture = Path.Combine(input, "图片.png");
            File.WriteAllBytes(schematic, Encoding.UTF8.GetBytes("OFFLINE BYTE FIXTURE 2D"));
            File.WriteAllBytes(layout, Encoding.UTF8.GetBytes("OFFLINE BYTE FIXTURE 3D"));
            File.WriteAllBytes(step, Encoding.UTF8.GetBytes("ISO-10303-21; OFFLINE FIXTURE"));
            File.WriteAllBytes(pdf, Encoding.UTF8.GetBytes("%PDF-1.4 OFFLINE FIXTURE"));
            File.WriteAllBytes(picture, new byte[] { 137, 80, 78, 71, 13, 10, 26, 10 });
            project.SchematicMacro = schematic; project.LayoutMacro = layout;
            project.StepFile = step; project.Document = pdf; project.Picture = picture;
            string edz = Path.Combine(folder, "离线打包测试.edz");
            ExportResult export = EdzExporter.Export(project, edz, catalog);
            Check(export.AssetCount == 5 && export.Notes.Contains("未转换"), "五类附件打包且 STEP 状态准确");
            Check(File.ReadAllBytes(edz).Take(6).SequenceEqual(new byte[] { 0x37, 0x7a, 0xbc, 0xaf, 0x27, 0x1c }), "EDZ 使用 7z 容器签名");
            string unpack = Path.Combine(folder, "解包验证");
            Directory.CreateDirectory(unpack);
            EdzExporter.RunSevenZip(folder, "x", "-y", "-o" + unpack, edz);
            var manifest = XDocument.Load(Path.Combine(unpack, "manifest.xml"));
            EdzExporter.VerifyManifest(unpack, manifest);
            Check(manifest.Root?.Attribute("version")?.Value == "2.0" && manifest.Descendants("item").Count() == 7, "manifest 版本和全部七个引用有效");
            string partPath = GetItemPath("part");
            var packedPart = XDocument.Load(partPath).Descendants("part").Single();
            Check(packedPart.Attribute("P_ARTICLE_GROUPSYMBOLMACRO")!.Value.StartsWith("$(MD_MACROS)") && packedPart.Attribute("P_ARTICLE_MACRO")!.Value.StartsWith("$(MD_MACROS)") && packedPart.Attribute("P_ARTICLE_EXTERNAL_DOCUMENT_2")!.Value.StartsWith("$(MD_DOCUMENTS)"), "宏与 STEP 属性分别映射且不泄漏本机绝对路径");
            var inputByName = new Dictionary<string, string> { ["groupsymbolmacro"] = schematic, ["graphicmacro"] = layout, ["externalDocument1"] = pdf, ["externalDocument2"] = step, ["picturefile"] = picture };
            foreach (var item in inputByName)
                Check(SHA256.HashData(File.ReadAllBytes(item.Value)).SequenceEqual(SHA256.HashData(File.ReadAllBytes(GetItemPath(item.Key)))), "附件字节完整：" + item.Key);
            Check(GetItemPath("groupsymbolmacro") != GetItemPath("graphicmacro"), "同名 2D 与 3D 宏不会覆盖");

            byte[] oldPackage = File.ReadAllBytes(edz);
            project.LayoutMacro = step;
            rejected = false;
            try { EdzExporter.Export(project, edz, catalog); } catch (InvalidDataException) { rejected = true; }
            Check(rejected && oldPackage.SequenceEqual(File.ReadAllBytes(edz)), "拒绝把 STEP 当 3D 宏且保留原输出");
            project.LayoutMacro = layout;
            project.SchematicMacro = Path.Combine(folder, "不存在.ema");
            Check(PartValidator.Validate(project, catalog).Any(e => e.Contains("不存在")), "缺失输入文件被拒绝");
            string atomic = Path.Combine(folder, "atomic.txt");
            File.WriteAllText(atomic, "original");
            try { AtomicFile.Write(atomic, temp => { File.WriteAllText(temp, "new"); throw new IOException("模拟写入中断"); }); } catch (IOException) { }
            Check(File.ReadAllText(atomic) == "original", "写入中断不破坏既有文件");

            WriteReport(true, "离线验证通过；未执行 EPLAN 导入或宏内容验证。");
            return 0;

            string GetItemPath(string name)
            {
                XElement item = manifest.Descendants("item").Single(i => (string?)i.Attribute("name") == name);
                return Path.Combine(unpack, "items", (string)item.Attribute("type")!, (string)item.Attribute("locator")!);
            }
        }
        catch (Exception ex)
        {
            WriteReport(false, ex.ToString());
            return 1;
        }

        void Check(bool condition, string title)
        {
            if (!condition) throw new InvalidOperationException("FAILED: " + title);
            checks.Add("PASS: " + title);
        }
        void WriteReport(bool passed, string detail)
        {
            File.WriteAllText(Path.Combine(folder, "report.txt"), $"EPLAN 部件打包助手离线验证\r\nPassed: {passed}\r\nChecks: {checks.Count}\r\n" + string.Join("\r\n", checks) + "\r\n" + detail);
        }
    }
}
