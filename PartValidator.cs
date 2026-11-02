using System.Xml;

namespace EplanPartBuilder;

public static class PartValidator
{
    public static List<string> Validate(PartProject part, IReadOnlyList<FunctionDefinition> catalog)
    {
        var errors = new List<string>();
        PartProject? before = part.Package == null ? null : PackageStore.Baseline(part);
        EditedText(part.PartNumber, before?.PartNumber, "部件编号", 255, true);
        EditedText(part.Manufacturer, before?.Manufacturer, "制造商", 30, true);
        EditedText(part.Name, before?.Name, "名称", 220, true);
        EditedText(part.TypeNumber, before?.TypeNumber, "型号", 255);
        EditedText(part.OrderNumber, before?.OrderNumber, "订货号", 255);
        EditedText(part.Characteristics, before?.Characteristics, "技术参数", 60);
        EditedText(part.Variant, before?.Variant, "变量", 3, true);
        EditedText(part.Description, before?.Description, "说明", 10000);
        EditedText(part.RatedCurrent, before?.RatedCurrent, "额定电流", 60);
        EditedText(part.Voltage, before?.Voltage, "电压", 60);
        EditedText(part.VoltageType, before?.VoltageType, "电压类型", 30);
        EditedText(part.PowerDissipation, before?.PowerDissipation, "功耗", 60);
        EditedText(part.CrossSection, before?.CrossSection, "连接横截面", 255);
        EditedText(part.ManufacturerName, before?.ManufacturerName, "厂家全称", 255);
        EditedText(part.ManufacturerCompany, before?.ManufacturerCompany, "公司名称", 255);
        EditedText(part.ManufacturerEmail, before?.ManufacturerEmail, "厂家邮箱", 255);
        EditedText(part.ManufacturerPhone, before?.ManufacturerPhone, "厂家电话", 255);
        if (part.Width < 0 || part.Height < 0 || part.Depth < 0)
            errors.Add("外形尺寸不能小于 0。");
        if (part.ProductGroup != before?.ProductGroup && !new[] { 0, 2, 3, 4, 5, 6, 9, 20, 26 }.Contains(part.ProductGroup))
            errors.Add("产品组不在本软件支持的列表内。");
        if ((part.Name != before?.Name && part.Name.Contains(';')) || (part.Description != before?.Description && part.Description.Contains(';')))
            errors.Add("名称和说明请使用中文分号“；”，英文分号是 EPLAN 多语言字段的分隔符。");

        // 未编辑的外部网址或路径按原包保留，不把它们误判为待复制的本机文件。
        if (part.SchematicMacro != before?.SchematicMacro) CheckAsset(part.SchematicMacro, "原理图宏", new[] { ".ema", ".ems" }, errors);
        if (part.LayoutMacro != before?.LayoutMacro) CheckAsset(part.LayoutMacro, "3D / 安装宏", new[] { ".ema" }, errors);
        if (part.StepFile != before?.StepFile) CheckAsset(part.StepFile, "外部文档 2", new[] { ".step", ".stp", ".pdf" }, errors);
        if (part.Picture != before?.Picture) CheckAsset(part.Picture, "产品图片", new[] { ".png", ".jpg", ".jpeg", ".bmp" }, errors);
        if (part.Document != before?.Document) CheckAsset(part.Document, "产品手册", new[] { ".pdf" }, errors);

        bool functionsUnchanged = before != null && System.Text.Json.JsonSerializer.Serialize(before.Functions) == System.Text.Json.JsonSerializer.Serialize(part.Functions);
        if (functionsUnchanged) return errors;

        var knownCodes = catalog.Select(f => f.Code).ToHashSet();
        var connections = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
        for (int i = 0; i < part.Functions.Count; i++)
        {
            var function = part.Functions[i];
            string row = $"功能模板第 {i + 1} 行";
            string originalCode = function.SourceXml.Length == 0 ? "" : PackageStore.ReadFunction(PackageEditor.ParseFunction(function.SourceXml)).Code;
            if (!knownCodes.Contains(function.Code) && originalCode != function.Code) errors.Add($"{row}：请选择有效的功能定义。");
            CheckText(ToEplanLines(function.Connections), row + "连接点", 255, false, errors);
            CheckText(ToEplanLines(function.ConnectionDescriptions), row + "连接点描述", 255, false, errors);
            CheckText(function.Plug, row + "插头", 30, false, errors);
            CheckText(function.Channel, row + "通道", 30, false, errors);
            CheckText(function.Characteristics, row + "技术参数", 60, false, errors);
            CheckText(function.Symbol, row + "符号", 50, false, errors);
            string[] pins = SplitPins(function.Connections);
            if (!string.IsNullOrWhiteSpace(function.Connections) && pins.Any(string.IsNullOrWhiteSpace))
                errors.Add($"{row}：连接点之间有空项，请检查竖线分隔符。");
            foreach (string pin in pins.Where(p => p.Length > 0))
            {
                // 相同端子号可在不同插头下复用，同一插头内不允许重复。
                if (!connections.Add(function.Plug.Trim() + "\u001f" + pin))
                    errors.Add($"{row}：连接点 {function.Plug}:{pin} 重复。");
            }
        }
        return errors;

        void EditedText(string value, string? original, string title, int max, bool required = false)
        {
            if (value != original) CheckText(value, title, max, required, errors);
        }
    }

    public static string[] SplitPins(string text)
    {
        if (string.IsNullOrWhiteSpace(text)) return Array.Empty<string>();
        return text.Replace("\r\n", "\n").Replace('\r', '\n').Replace('\n', '|').Split('|').Select(p => p.Trim()).ToArray();
    }

    public static string ToEplanLines(string text) => string.Join("\n", SplitPins(text));

    private static void CheckText(string? value, string title, int max, bool required, List<string> errors)
    {
        if (value == null) { errors.Add(title + "不能为 null。"); return; }
        if (required && string.IsNullOrWhiteSpace(value)) errors.Add(title + "不能为空。");
        if (value.Length > max) errors.Add($"{title}不能超过 {max} 个字符。");
        try { XmlConvert.VerifyXmlChars(value); }
        catch (XmlException) { errors.Add(title + "包含 XML 不允许的控制字符。"); }
    }

    private static void CheckAsset(string? path, string title, string[] extensions, List<string> errors)
    {
        if (string.IsNullOrWhiteSpace(path)) return;
        try
        {
            if (!extensions.Contains(Path.GetExtension(path), StringComparer.OrdinalIgnoreCase))
                errors.Add(title + "格式不支持。");
            if (!File.Exists(path)) { errors.Add(title + "文件不存在：" + path); return; }
            if (new FileInfo(path).Length == 0) errors.Add(title + "文件为空。");
        }
        catch (Exception ex) when (ex is IOException or UnauthorizedAccessException or ArgumentException)
        {
            errors.Add(title + "无法读取：" + ex.Message);
        }
    }
}
