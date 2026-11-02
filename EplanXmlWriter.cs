using System.Globalization;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace EplanPartBuilder;

public static class EplanXmlWriter
{
    public static XDocument Create(PartProject part, IReadOnlyDictionary<string, string> references)
    {
        // EPLAN 2.9 的层级必须为 partsmanagement → part → variant → functiontemplate。
        var root = new XElement("partsmanagement",
            new XAttribute("count", "1"), new XAttribute("length-unit", "mm"),
            new XAttribute("weight-unit", "kg"), new XAttribute("type", "EPLAN.PartsManagement"));
        var article = new XElement("part");
        Set(article, "P_ARTICLE_PARTTYPE", "1");
        Set(article, "P_ARTICLE_PARTNR", part.PartNumber.Trim());
        Set(article, "P_ARTICLE_MANUFACTURER", part.Manufacturer.Trim());
        Set(article, "P_ARTICLE_TYPENR", part.TypeNumber);
        Set(article, "P_ARTICLE_ORDERNR", part.OrderNumber);
        Set(article, "P_ARTICLE_DESCR1", MultiLanguage(part.Name));
        Set(article, "P_ARTICLE_NOTE", MultiLanguage(part.Description));
        Set(article, "P_ARTICLE_PRODUCTTOPGROUP", "1");
        Set(article, "P_ARTICLE_PRODUCTGROUP", part.ProductGroup.ToString(CultureInfo.InvariantCulture));
        Set(article, "P_ARTICLE_PRODUCTSUBGROUP", "0");
        Set(article, "P_ARTICLE_CRAFT_ELECTRICAL", "1");
        Set(article, "P_ARTICLE_WIDTH", part.Width.ToString(CultureInfo.InvariantCulture));
        Set(article, "P_ARTICLE_HEIGHT", part.Height.ToString(CultureInfo.InvariantCulture));
        Set(article, "P_ARTICLE_DEPTH", part.Depth.ToString(CultureInfo.InvariantCulture));
        Set(article, "P_ARTICLE_PACKAGINGQUANTITY", "1");

        // 原理图宏和安装宏是两个属性，不能都写入同一个字段。
        SetReference(article, "P_ARTICLE_GROUPSYMBOLMACRO", "schematic", references);
        SetReference(article, "P_ARTICLE_MACRO", "layout", references);
        SetReference(article, "P_ARTICLE_PICTUREFILE", "picture", references);
        SetReference(article, "P_ARTICLE_EXTERNAL_DOCUMENT_1", "document", references);
        SetReference(article, "P_ARTICLE_EXTERNAL_DOCUMENT_2", "step", references);

        var variant = new XElement("variant", new XAttribute("P_ARTICLE_VARIANT", part.Variant.Trim()));
        Set(variant, "P_ARTICLE_CHARACTERISTICS", part.Characteristics);
        Set(variant, "P_ARTICLE_ELECTRICALCURRENT", part.RatedCurrent);
        Set(variant, "P_ARTICLE_VOLTAGE", part.Voltage);
        Set(variant, "P_ARTICLE_VOLTAGETYPE", part.VoltageType);
        Set(variant, "P_ARTICLE_POWERDISSIPATION", part.PowerDissipation);
        Set(variant, "P_ARTICLE_CONNECTIONCROSSSECTION", part.CrossSection);
        for (int i = 0; i < part.Functions.Count; i++)
        {
            var row = part.Functions[i];
            string[] code = row.Code.Split('.');
            var function = new XElement("functiontemplate",
                new XAttribute("pos", i + 1),
                new XAttribute("functiondefcategory", code[0]),
                new XAttribute("functiondefgroup", code[1]),
                new XAttribute("functiondefid", code[2]));
            Set(function, "connectionDesignation", PartValidator.ToEplanLines(row.Connections));
            Set(function, "connectiondescription", PartValidator.ToEplanLines(row.ConnectionDescriptions));
            Set(function, "terminalNr", row.Plug);
            Set(function, "idx1", row.Channel);
            Set(function, "characteristics", row.Characteristics);
            Set(function, "symbol", row.Symbol);
            variant.Add(function);
        }
        article.Add(variant);
        root.Add(article);
        return new XDocument(new XDeclaration("1.0", "utf-8", null), root);
    }

    public static void Save(XDocument document, string path)
    {
        var settings = new XmlWriterSettings
        {
            Encoding = new UTF8Encoding(false), Indent = true,
            // 属性中的连接点换行须保存为 &#xA;，否则 XML 读取会把换行归一为空格。
            NewLineHandling = NewLineHandling.Entitize
        };
        using var writer = XmlWriter.Create(path, settings);
        document.Save(writer);
    }

    // EPLAN 多语言文本以 @ 分隔语言和内容，正文中的 @ 用 @@ 表示。
    private static string MultiLanguage(string text) => string.IsNullOrEmpty(text) ? "" : "zh_CN@" + text.Replace("@", "@@") + ";";
    private static void Set(XElement element, string name, string value)
    {
        if (!string.IsNullOrEmpty(value)) element.SetAttributeValue(name, value);
    }
    private static void SetReference(XElement element, string property, string role, IReadOnlyDictionary<string, string> references)
    {
        if (references.TryGetValue(role, out string? path)) Set(element, property, path);
    }
}
