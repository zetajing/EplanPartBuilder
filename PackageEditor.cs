using System.Globalization;
using System.Text.Json;
using System.Xml.Linq;

namespace EplanPartBuilder;

public static class PackageEditor
{
    public static ExportResult Export(PartProject current, string destination)
    {
        PackageTemplate template = current.Package!;
        PartProject before = PackageStore.Baseline(current);
        string stage = Path.Combine(Path.GetTempPath(), "EplanPartBuilder", Guid.NewGuid().ToString("N"));
        try
        {
            PackageStore.CopyDirectory(template.Folder, stage);
            string manifestPath = Path.Combine(stage, "manifest.xml");
            var manifest = PackageStore.LoadXml(manifestPath);
            var originalManifest = new XDocument(manifest);
            var packages = manifest.Root!.Element("packages")!.Elements("package").ToList();
            var package = packages[template.PackageIndex];
            var items = package.Element("items")!;
            if (packages.Where((p, i) => i != template.PackageIndex).Any(p => (string?)p.Attribute("type") == "part" && (string?)p.Attribute("key") == current.PartNumber))
                throw new InvalidDataException("部件编号与包内其他部件重复。");
            var partItem = items.Elements("item").Single(i => (string?)i.Attribute("name") == "part");
            string partPath = PackageStore.ItemPath(stage, partItem);
            var partDocument = PackageStore.LoadXml(partPath);
            var originalPart = new XDocument(partDocument);
            var article = partDocument.Root!.Elements("part").ElementAt(template.PartIndex);
            var variant = article.Elements("variant").ElementAt(template.VariantIndex);
            if (article.Elements("variant").Where((v, i) => i != template.VariantIndex).Any(v => PackageStore.Text(v, "P_ARTICLE_VARIANT") == current.Variant))
                throw new InvalidDataException("变量编号与同一部件的其他变量重复。");

            // 所有写入都有“编辑前 / 编辑后”比较，不把原包重新生成为简化结构。
            Patch(article, "P_ARTICLE_PARTNR", before.PartNumber, current.PartNumber);
            Patch(article, "P_ARTICLE_MANUFACTURER", before.Manufacturer, current.Manufacturer);
            Patch(article, "P_ARTICLE_TYPENR", before.TypeNumber, current.TypeNumber);
            Patch(article, "P_ARTICLE_ORDERNR", before.OrderNumber, current.OrderNumber);
            PatchMultilingual(article, "P_ARTICLE_DESCR1", before.Name, current.Name);
            PatchMultilingual(article, "P_ARTICLE_NOTE", before.Description, current.Description);
            Patch(article, "P_ARTICLE_PRODUCTGROUP", before.ProductGroup, current.ProductGroup);
            Patch(article, "P_ARTICLE_WIDTH", before.Width, current.Width);
            Patch(article, "P_ARTICLE_HEIGHT", before.Height, current.Height);
            Patch(article, "P_ARTICLE_DEPTH", before.Depth, current.Depth);
            Patch(variant, "P_ARTICLE_VARIANT", before.Variant, current.Variant);
            Patch(variant, "P_ARTICLE_CHARACTERISTICS", before.Characteristics, current.Characteristics);
            Patch(variant, "P_ARTICLE_ELECTRICALCURRENT", before.RatedCurrent, current.RatedCurrent);
            Patch(variant, "P_ARTICLE_VOLTAGE", before.Voltage, current.Voltage);
            Patch(variant, "P_ARTICLE_VOLTAGETYPE", before.VoltageType, current.VoltageType);
            Patch(variant, "P_ARTICLE_POWERDISSIPATION", before.PowerDissipation, current.PowerDissipation);
            Patch(variant, "P_ARTICLE_CONNECTIONCROSSSECTION", before.CrossSection, current.CrossSection);

            if (before.PartNumber != current.PartNumber)
            {
                package.SetAttributeValue("key", current.PartNumber);
                package.SetAttributeValue("name", current.PartNumber);
            }
            PatchFunctions(variant, before, current);
            PatchManufacturer(stage, packages, items, before, current);

            string assetFolder = "PartBuilder\\" + Guid.NewGuid().ToString("N");
            int replacements = 0;
            ReplaceAsset("groupsymbolmacro", "macro", "P_ARTICLE_GROUPSYMBOLMACRO", "$(MD_MACROS)", before.SchematicMacro, current.SchematicMacro);
            ReplaceAsset("graphicmacro", "macro", "P_ARTICLE_MACRO", "$(MD_MACROS)", before.LayoutMacro, current.LayoutMacro);
            ReplaceAsset("picturefile", "picture", "P_ARTICLE_PICTUREFILE", "$(MD_IMG)", before.Picture, current.Picture);
            ReplaceAsset("externalDocument1", "document", "P_ARTICLE_EXTERNAL_DOCUMENT_1", "$(MD_DOCUMENTS)", before.Document, current.Document);
            ReplaceAsset("externalDocument2", "document", "P_ARTICLE_EXTERNAL_DOCUMENT_2", "$(MD_DOCUMENTS)", before.StepFile, current.StepFile);

            SaveIfChanged(originalPart, partDocument, partPath);
            if (before.PartNumber != current.PartNumber || before.Variant != current.Variant)
                PatchPartReferences(stage, before, current);
            SaveIfChanged(originalManifest, manifest, manifestPath);
            EdzExporter.VerifyManifest(stage, manifest);
            AtomicFile.Write(destination, temp =>
            {
                EdzExporter.RunSevenZip(stage, "a", "-t7z", "-m0=LZMA", "-md=4k", "-ms=off", "-mhc=off", "-mx=3", temp, ".");
                EdzExporter.RunSevenZip(stage, "t", "-t7z", temp);
            });
            string notes = "保留原包全部记录和文件，更新指定字段；替换 / 新增附件 " + replacements + " 个。保留原版本，不进行宏降版。";
            if (current.StepFile != before.StepFile && current.StepFile.Length > 0) notes += " 外部文档 2 作为源文件附件，不转换成 3D 宏。";
            if (JsonSerializer.Serialize(before.Functions) != JsonSerializer.Serialize(current.Functions))
                notes += " 功能模板已修改，原连接点排列样式保留，请在 EPLAN 中核对对应关系。";
            return new ExportResult { Path = Path.GetFullPath(destination), AssetCount = Directory.EnumerateFiles(stage, "*", SearchOption.AllDirectories).Count(), Notes = notes };

            void ReplaceAsset(string role, string type, string property, string variable, string oldValue, string newValue)
            {
                if (oldValue == newValue) return;
                var existing = items.Elements("item").FirstOrDefault(i => (string?)i.Attribute("name") == role);
                if (newValue.Length == 0)
                {
                    existing?.Remove(); article.Attribute(property)?.Remove(); return;
                }
                // 使用新路径，避免覆盖其他部件或宏引用的共享文件。旧文件仍保留。
                string locator = assetFolder + "\\" + role + Path.GetExtension(newValue).ToLowerInvariant();
                var item = existing == null ? new XElement("item") : new XElement(existing);
                item.SetAttributeValue("type", type); item.SetAttributeValue("name", role); item.SetAttributeValue("locator", locator);
                string target = PackageStore.ItemPath(stage, item);
                Directory.CreateDirectory(Path.GetDirectoryName(target)!);
                File.Copy(newValue, target, false);
                if (existing == null) items.Add(item); else existing.ReplaceWith(item);
                string oldProperty = PackageStore.Text(article, property);
                int newline = oldProperty.IndexOf('\n');
                string suffix = role.StartsWith("externalDocument") && newline >= 0 ? oldProperty[newline..] : "";
                article.SetAttributeValue(property, variable + "\\" + locator + suffix);
                replacements++;
            }
        }
        finally
        {
            // stage 是本次导出创建的随机目录。
            if (Directory.Exists(stage)) Directory.Delete(stage, true);
        }
    }

    private static void PatchFunctions(XElement variant, PartProject before, PartProject current)
    {
        if (JsonSerializer.Serialize(before.Functions) == JsonSerializer.Serialize(current.Functions)) return;
        var oldElements = variant.Elements("functiontemplate").ToList();
        bool orderChanged = !before.Functions.Select(f => f.SourceXml).SequenceEqual(current.Functions.Select(f => f.SourceXml));
        var marker = new XComment("function insertion point");
        if (oldElements.Count > 0) oldElements[0].AddBeforeSelf(marker); else variant.Add(marker);
        foreach (var element in oldElements) element.Remove();
        for (int i = 0; i < current.Functions.Count; i++)
        {
            FunctionRow row = current.Functions[i];
            XElement element = row.SourceXml.Length > 0 ? ParseFunction(row.SourceXml) : new XElement("functiontemplate");
            FunctionRow old = row.SourceXml.Length > 0 ? PackageStore.ReadFunction(element) : new FunctionRow();
            if (old.Code != row.Code)
            {
                string[] code = row.Code.Split('.');
                element.SetAttributeValue("functiondefcategory", code[0]);
                element.SetAttributeValue("functiondefgroup", code[1]);
                element.SetAttributeValue("functiondefid", code[2]);
            }
            if (orderChanged || row.SourceXml.Length == 0) element.SetAttributeValue("pos", i + 1);
            Patch(element, "connectionDesignation", PartValidator.ToEplanLines(old.Connections), PartValidator.ToEplanLines(row.Connections));
            Patch(element, "connectiondescription", PartValidator.ToEplanLines(old.ConnectionDescriptions), PartValidator.ToEplanLines(row.ConnectionDescriptions));
            Patch(element, "terminalNr", old.Plug, row.Plug); Patch(element, "idx1", old.Channel, row.Channel);
            Patch(element, "characteristics", old.Characteristics, row.Characteristics); Patch(element, "symbol", old.Symbol, row.Symbol);
            marker.AddBeforeSelf(element);
        }
        marker.Remove();
    }

    public static XElement ParseFunction(string xml)
    {
        using var reader = System.Xml.XmlReader.Create(new StringReader(xml), new System.Xml.XmlReaderSettings { DtdProcessing = System.Xml.DtdProcessing.Prohibit, XmlResolver = null, MaxCharactersInDocument = 1024 * 1024 });
        var element = XElement.Load(reader);
        if (element.Name != "functiontemplate") throw new InvalidDataException("功能模板原始 XML 无效。");
        return element;
    }

    private static void PatchManufacturer(string folder, List<XElement> packages, XElement items, PartProject before, PartProject current)
    {
        bool changed = before.Manufacturer != current.Manufacturer || before.ManufacturerName != current.ManufacturerName
            || before.ManufacturerCompany != current.ManufacturerCompany || before.ManufacturerEmail != current.ManufacturerEmail
            || before.ManufacturerPhone != current.ManufacturerPhone;
        if (!changed) return;
        var oldItem = items.Elements("item").FirstOrDefault(i => (string?)i.Attribute("name") == "manufacturer");
        if (before.Manufacturer == current.Manufacturer && oldItem != null && packages.Any(p => p.Element("items") != items && p.Descendants("item").Any(i => (string?)i.Attribute("name") == "manufacturer" && (string?)i.Attribute("locator") == (string?)oldItem.Attribute("locator"))))
            throw new InvalidDataException("该厂家资料被包内其他部件共用。只修改当前部件时，请先使用新的厂家代号。");
        XDocument document = oldItem == null ? new XDocument(new XElement("partsmanagement", new XAttribute("count", "1"), new XAttribute("type", "EPLAN.PartsManagement")))
            : PackageStore.LoadXml(PackageStore.ItemPath(folder, oldItem));
        var address = document.Descendants("address").FirstOrDefault(a => PackageStore.Text(a, "P_PART_ADDRESS_SHORTNAME") == before.Manufacturer);
        if (address == null) { address = new XElement("address", new XAttribute("class", "12")); document.Root!.Add(address); }
        Patch(address, "P_PART_ADDRESS_SHORTNAME", before.Manufacturer, current.Manufacturer);
        Patch(address, "P_PART_ADDRESS_LONGNAME", before.ManufacturerName, current.ManufacturerName);
        Patch(address, "P_PART_ADDRESS_NAME1", before.ManufacturerCompany, current.ManufacturerCompany);
        Patch(address, "P_PART_ADDRESS_EMAIL", before.ManufacturerEmail, current.ManufacturerEmail);
        Patch(address, "P_PART_ADDRESS_PHONE", before.ManufacturerPhone, current.ManufacturerPhone);
        var newItem = oldItem == null ? new XElement("item") : new XElement(oldItem);
        newItem.SetAttributeValue("type", "partxml"); newItem.SetAttributeValue("name", "manufacturer");
        newItem.SetAttributeValue("locator", "manufacturer_" + Guid.NewGuid().ToString("N") + ".xml");
        EplanXmlWriter.Save(document, PackageStore.ItemPath(folder, newItem));
        if (oldItem == null) items.Add(newItem); else oldItem.ReplaceWith(newItem);
    }

    private static void PatchPartReferences(string folder, PartProject before, PartProject current)
    {
        // 只改明确的部件引用字段，不对宏内容、任意描述和路径作全文替换。
        foreach (string file in Directory.EnumerateFiles(Path.Combine(folder, "items", "partxml"), "*.xml", SearchOption.AllDirectories))
        {
            var document = PackageStore.LoadXml(file);
            bool changed = false;
            foreach (var element in document.Descendants().Where(e => (string?)e.Attribute("partnr") == before.PartNumber))
            {
                if (before.PartNumber != current.PartNumber) { element.SetAttributeValue("partnr", current.PartNumber); changed = true; }
                if (before.Variant != current.Variant && (string?)element.Attribute("variant") == before.Variant)
                { element.SetAttributeValue("variant", current.Variant); changed = true; }
            }
            if (changed) EplanXmlWriter.Save(document, file);
        }
    }

    private static void Patch<T>(XElement element, string property, T oldValue, T newValue)
    {
        if (EqualityComparer<T>.Default.Equals(oldValue, newValue)) return;
        string value = Convert.ToString(newValue, CultureInfo.InvariantCulture) ?? "";
        element.SetAttributeValue(property, value.Length == 0 ? null : value);
    }

    private static void PatchMultilingual(XElement element, string property, string oldValue, string newValue)
    {
        if (oldValue == newValue) return;
        string text = PackageStore.Text(element, property);
        var languages = System.Text.RegularExpressions.Regex.Matches(text, @"(?:^|;)(?<language>[a-z]{2}_[A-Z]{2}|\?\?_\?\?)@");
        var chinese = languages.Cast<System.Text.RegularExpressions.Match>().FirstOrDefault(m => m.Groups["language"].Value == "zh_CN");
        string replacement = "zh_CN@" + newValue.Replace("@", "@@");
        if (chinese == null)
            text += (text.Length > 0 && !text.EndsWith(';') ? ";" : "") + replacement + ";";
        else
        {
            // 只替换中文片段；其余语言的原始字节内容、转义和分隔符不重新拼装。
            int start = chinese.Groups["language"].Index;
            var next = languages.Cast<System.Text.RegularExpressions.Match>().FirstOrDefault(m => m.Index > chinese.Index);
            int end = next?.Index ?? (text.EndsWith(';') ? text.Length - 1 : text.Length);
            text = text[..start] + replacement + text[end..];
        }
        element.SetAttributeValue(property, text);
    }

    private static void SaveIfChanged(XDocument before, XDocument after, string path)
    {
        if (!XNode.DeepEquals(before, after)) EplanXmlWriter.Save(after, path);
    }
}
