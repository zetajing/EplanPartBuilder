using System.IO.Compression;
using System.Security.Cryptography;
using System.Xml.Linq;

namespace EplanPartBuilder;

public static class PackageChecks
{
    // 仅在独立检查程序中使用，不编译进正式软件。
    public static int Run(string source, string output)
    {
        output = Path.GetFullPath(output);
        Directory.CreateDirectory(output);
        var checks = new List<string>();
        var catalog = FunctionCatalog.Load();
        try
        {
            // 使用真实包验证保留行为，不用按导出实现拼出来的模型代替真实输入。
            var originalHashes = Hashes(source);
            var project = PackageStore.Open(source).Single();
            Check(project.PartNumber == "DLX.CDB6H632B8" && project.Name == "小型断路器" && project.Width == 35.4m, "真实部件资料读取");
            Check(project.Voltage == "400/415 V" && project.PowerDissipation == "6 W" && project.ManufacturerName == "DELIXI ELECTRIC", "技术参数与厂家资料读取");
            Check(project.Functions.Single().Code == "501.2.1" && project.Functions.Single().SourceXml.Contains("manualmoduletemplate"), "功能定义与未知属性读取");
            Check(project.Document.StartsWith("https://") && PartValidator.Validate(project, catalog).Count == 0, "原有手册网址可保留");
            string unchanged = Export(project, "unchanged");
            Check(EqualHashes(originalHashes, Hashes(unchanged)), "零修改导出：原包全部文件逐字节一致");
            Check(PackageStore.Open(Path.Combine(output, "unchanged.edz")).Single().PartNumber == project.PartNumber, "生成的 7z EDZ 可重新打开");

            var before = PackageStore.Baseline(project);
            project.PartNumber = "TEST.CDB6H632B8.COPY"; project.Name = "检查用断路器"; project.RatedCurrent = "8 A";
            string edited = Export(project, "edited");
            var editedPart = Part(edited); var originalPart = Part(source);
            Check(PackageStore.Text(editedPart, "P_ARTICLE_PARTNR") == project.PartNumber && PackageStore.Text(editedPart, "P_ARTICLE_DESCR1") == "en_US@MCB;zh_CN@检查用断路器;", "改编号和中文名称，保留英文");
            Check(PackageStore.Text(editedPart.Element("variant")!, "P_ARTICLE_ELECTRICALCURRENT") == "8 A", "补充结构化额定电流");
            var normalized = new XElement(editedPart);
            normalized.SetAttributeValue("P_ARTICLE_PARTNR", before.PartNumber);
            normalized.SetAttributeValue("P_ARTICLE_DESCR1", originalPart.Attribute("P_ARTICLE_DESCR1")!.Value);
            normalized.Element("variant")!.Attribute("P_ARTICLE_ELECTRICALCURRENT")!.Remove();
            Check(XNode.DeepEquals(normalized, originalPart), "指定三项修改以外的部件数据完整保留");
            CheckUnchangedAssets(source, edited);
            Check(PackageStore.LoadXml(PartPath(edited)).Root!.Attribute("version")!.Value == "2024.0.21729", "保留原始 2024 版本信息");

            string replacement = Path.Combine(output, "replacement.png");
            using (var bitmap = new Bitmap(16, 16)) { using var graphics = Graphics.FromImage(bitmap); graphics.Clear(Color.Teal); bitmap.Save(replacement); }
            project.Picture = replacement;
            string replaced = Export(project, "replaced");
            var imageItem = Manifest(replaced).Descendants("item").Single(i => (string?)i.Attribute("name") == "picturefile");
            Check(HashFile(PackageStore.ItemPath(replaced, imageItem)) == HashFile(replacement), "图片替换后清单指向新文件");
            Check(originalHashes.All(pair => File.Exists(Path.Combine(replaced, pair.Key))), "替换附件后原始文件仍保留");
            project.Document = "";
            string cleared = Export(project, "cleared");
            Check(Part(cleared).Attribute("P_ARTICLE_EXTERNAL_DOCUMENT_1") == null, "可清除原来的外部手册链接");

            string savePath = Path.Combine(output, "saved.epart.json"); project = ProjectStore.Save(savePath, project);
            string moved = Path.Combine(output, "moved"); Directory.CreateDirectory(moved);
            File.Copy(savePath, Path.Combine(moved, Path.GetFileName(savePath)), true);
            PackageStore.CopyDirectory(savePath + ".assets", Path.Combine(moved, Path.GetFileName(savePath) + ".assets"));
            var reloaded = ProjectStore.Load(Path.Combine(moved, Path.GetFileName(savePath)));
            Check(File.Exists(reloaded.LayoutMacro) && File.Exists(reloaded.Picture) && reloaded.Picture.StartsWith(moved) && reloaded.Package!.Folder.StartsWith(moved) && reloaded.RatedCurrent == "8 A", "资料、原包和替换文件整体移动后可打开");
            string loaded = Export(reloaded, "reloaded"); CheckUnchangedAssets(source, loaded);

            string fixture = Path.Combine(output, "multi-input"); PackageStore.CopyDirectory(source, fixture);
            var fixtureManifest = Manifest(fixture);
            var partPackage = fixtureManifest.Root!.Element("packages")!.Elements("package").First();
            var fixtureDoc = PackageStore.LoadXml(PartPath(fixture)); var firstPart = fixtureDoc.Root!.Element("part")!;
            var firstVariant = firstPart.Element("variant")!;
            firstVariant.Element("functiontemplate")!.SetAttributeValue("vendor_unknown", "KEEP-ME");
            var secondVariant = new XElement(firstVariant); secondVariant.SetAttributeValue("P_ARTICLE_VARIANT", "2"); firstPart.Add(secondVariant);
            EplanXmlWriter.Save(fixtureDoc, PartPath(fixture));
            var secondDoc = new XDocument(fixtureDoc); secondDoc.Root!.Element("part")!.SetAttributeValue("P_ARTICLE_PARTNR", "SECOND.PART");
            string secondFile = Path.Combine(fixture, "items", "partxml", "second.part.xml"); EplanXmlWriter.Save(secondDoc, secondFile);
            var secondPackage = new XElement(partPackage); secondPackage.SetAttributeValue("key", "SECOND.PART"); secondPackage.SetAttributeValue("name", "SECOND.PART");
            secondPackage.Descendants("item").Single(i => (string?)i.Attribute("name") == "part").SetAttributeValue("locator", "second.part.xml");
            fixtureManifest.Root.Element("packages")!.Add(secondPackage);
            string extra = Path.Combine(fixture, "extra.bin"); File.WriteAllBytes(extra, new byte[] { 2, 4, 6, 8 });
            EplanXmlWriter.Save(fixtureManifest, Path.Combine(fixture, "manifest.xml"));
            var multiple = PackageStore.Open(fixture); Check(multiple.Count == 4, "多部件与多变量均可选择");
            var selected = multiple[0]; selected.Name = "仅修改所选部件"; selected.Functions[0].ConnectionDescriptions = "IN|OUT|IN|OUT";
            string multi = Export(selected, "multi-output");
            Check(HashFile(Path.Combine(multi, "items", "partxml", "second.part.xml")) == HashFile(secondFile), "未选中的其他部件逐字节保留");
            Check(XNode.DeepEquals(Part(multi).Elements("variant").ElementAt(1), secondVariant), "未选中的其他变量保留");
            Check(Part(multi).Element("variant")!.Element("functiontemplate")!.Attribute("vendor_unknown")?.Value == "KEEP-ME", "编辑功能列保留厂商未知属性");
            Check(HashFile(Path.Combine(multi, "extra.bin")) == HashFile(extra), "清单之外的额外文件也保留");
            selected.ManufacturerEmail = "test@example.invalid";
            bool rejected = false; try { Export(selected, "shared-manufacturer"); } catch (InvalidDataException) { rejected = true; }
            Check(rejected, "共用厂家记录修改冲突被阻止");
            selected.Manufacturer = "MYTEST";
            string manufacturer = Export(selected, "new-manufacturer");
            var makerItem = Manifest(manufacturer).Descendants("package").First().Descendants("item").Single(i => (string?)i.Attribute("name") == "manufacturer");
            var address = PackageStore.LoadXml(PackageStore.ItemPath(manufacturer, makerItem)).Descendants("address").Single();
            Check(PackageStore.Text(address, "P_PART_ADDRESS_SHORTNAME") == "MYTEST" && PackageStore.Text(address, "P_PART_ADDRESS_EMAIL") == "test@example.invalid", "新厂家资料单独生成");

            string zip = Path.Combine(output, "source-zip.edz"); ZipFile.CreateFromDirectory(source, zip);
            Check(PackageStore.Open(zip).Single().PartNumber == before.PartNumber, "ZIP 容器 EDZ 可导入");
            string unsafeZip = Path.Combine(output, "unsafe.edz");
            using (var archive = ZipFile.Open(unsafeZip, ZipArchiveMode.Create)) { using var writer = new StreamWriter(archive.CreateEntry("../escaped.txt").Open()); writer.Write("must not escape"); }
            rejected = false; try { PackageStore.Open(unsafeZip); } catch (InvalidDataException) { rejected = true; }
            Check(rejected && !File.Exists(Path.Combine(output, "escaped.txt")), "拒绝压缩包路径穿越");
            rejected = false; try { PackageStore.SafePath(output, "items/../../escape"); } catch (InvalidDataException) { rejected = true; }
            Check(rejected, "拒绝清单中的越界路径");
            string bad = Path.Combine(output, "missing-input"); PackageStore.CopyDirectory(source, bad);
            var badManifest = Manifest(bad); badManifest.Descendants("item").First().SetAttributeValue("locator", "missing.xml"); EplanXmlWriter.Save(badManifest, Path.Combine(bad, "manifest.xml"));
            rejected = false; try { PackageStore.Open(bad); } catch (InvalidDataException) { rejected = true; }
            Check(rejected, "缺失清单文件时拒绝导入");
            Check(EqualHashes(originalHashes, Hashes(source)), "整个检查过程没有修改桌面原包");
            WriteReport(true, "真实样包的文件保留与编辑检查通过；不等于 EPLAN 导入、放置或布线验证。"); return 0;
        }
        catch (Exception ex) { WriteReport(false, ex.ToString()); return 1; }

        string Export(PartProject model, string name)
        {
            string path = Path.Combine(output, name + ".edz"); EdzExporter.Export(model, path, catalog);
            string unpack = Path.Combine(output, name + "-unpacked"); Directory.CreateDirectory(unpack);
            PackageStore.ExtractArchive(path, unpack); EdzExporter.VerifyManifest(unpack, Manifest(unpack)); return unpack;
        }
        void CheckUnchangedAssets(string from, string to)
        {
            foreach (var pair in Hashes(from).Where(p => !p.Key.Equals("manifest.xml", StringComparison.OrdinalIgnoreCase) && !p.Key.EndsWith(".part.xml", StringComparison.OrdinalIgnoreCase)))
                Check(File.Exists(Path.Combine(to, pair.Key)) && HashFile(Path.Combine(to, pair.Key)) == pair.Value, "未编辑文件完整：" + pair.Key);
        }
        void Check(bool passed, string title) { if (!passed) throw new InvalidOperationException("FAILED: " + title); checks.Add("PASS: " + title); }
        void WriteReport(bool passed, string detail) => File.WriteAllText(Path.Combine(output, "package-report.txt"), $"Passed: {passed}\r\nChecks: {checks.Count}\r\n" + string.Join("\r\n", checks) + "\r\n" + detail);
    }

    private static XDocument Manifest(string folder) => PackageStore.LoadXml(Path.Combine(folder, "manifest.xml"));
    private static string PartPath(string folder) => PackageStore.ItemPath(folder, Manifest(folder).Descendants("item").First(i => (string?)i.Attribute("name") == "part"));
    private static XElement Part(string folder) => PackageStore.LoadXml(PartPath(folder)).Root!.Element("part")!;
    private static Dictionary<string, string> Hashes(string folder) => Directory.EnumerateFiles(folder, "*", SearchOption.AllDirectories).ToDictionary(p => Path.GetRelativePath(folder, p), HashFile, StringComparer.OrdinalIgnoreCase);
    private static string HashFile(string path) { using var stream = File.OpenRead(path); return Convert.ToHexString(SHA256.HashData(stream)); }
    private static bool EqualHashes(Dictionary<string, string> first, Dictionary<string, string> second) => first.Count == second.Count && first.All(p => second.TryGetValue(p.Key, out string? hash) && hash == p.Value);
}
