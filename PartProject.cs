using System.ComponentModel;
using System.Text.Json;

namespace EplanPartBuilder;

public sealed class PartProject
{
    // 一个资料文件对应一个零部件、一个变量。宏文件的内容由用户提供。
    public int FileVersion { get; set; } = 1;
    public string PartNumber { get; set; } = "";
    public string Manufacturer { get; set; } = "";
    public string TypeNumber { get; set; } = "";
    public string OrderNumber { get; set; } = "";
    public string Name { get; set; } = "";
    public string Description { get; set; } = "";
    public string Characteristics { get; set; } = "";
    public string Variant { get; set; } = "1";
    public int ProductGroup { get; set; }
    public decimal Width { get; set; }
    public decimal Height { get; set; }
    public decimal Depth { get; set; }
    public string SchematicMacro { get; set; } = "";
    public string LayoutMacro { get; set; } = "";
    public string StepFile { get; set; } = "";
    public string Picture { get; set; } = "";
    public string Document { get; set; } = "";
    public string RatedCurrent { get; set; } = "";
    public string Voltage { get; set; } = "";
    public string VoltageType { get; set; } = "";
    public string PowerDissipation { get; set; } = "";
    public string CrossSection { get; set; } = "";
    public string ManufacturerName { get; set; } = "";
    public string ManufacturerCompany { get; set; } = "";
    public string ManufacturerEmail { get; set; } = "";
    public string ManufacturerPhone { get; set; } = "";
    public PackageTemplate? Package { get; set; }
    public BindingList<FunctionRow> Functions { get; set; } = new();

    public static PartProject RelayExample()
    {
        return new PartProject
        {
            PartNumber = "DEMO.RELAY.24V.1NO1NC",
            Manufacturer = "DEMO",
            TypeNumber = "示例继电器",
            Name = "示例继电器，24 V DC，1 常开 + 1 常闭",
            Description = "练习资料；实际参数和连接点请按产品手册填写。",
            Characteristics = "24 V DC",
            ProductGroup = 2,
            Functions = new BindingList<FunctionRow>
            {
                new() { Code = "200.1.1", Connections = "A1|A2", Characteristics = "24 V DC" },
                new() { Code = "201.1.2", Connections = "13|14" },
                new() { Code = "202.1.2", Connections = "21|22" }
            }
        };
    }
}

public sealed class FunctionRow
{
    // 原有功能的完整 XML 随资料保存，编辑一列时保留其他未知属性。
    public string SourceXml { get; set; } = "";
    public string Code { get; set; } = "";
    public string Connections { get; set; } = "";
    public string ConnectionDescriptions { get; set; } = "";
    public string Plug { get; set; } = "";
    public string Channel { get; set; } = "";
    public string Characteristics { get; set; } = "";
    public string Symbol { get; set; } = "";
}

public sealed class PackageTemplate
{
    public string Folder { get; set; } = "";
    public string OriginalSource { get; set; } = "";
    public int PackageIndex { get; set; }
    public int PartIndex { get; set; }
    public int VariantIndex { get; set; }
    public string BaselineJson { get; set; } = "";
}

public sealed class FunctionDefinition
{
    public string Code { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public string Group { get; set; } = "";
    public string Display => $"{Code}  {Name}";
}

public static class FunctionCatalog
{
    public static List<FunctionDefinition> Load()
    {
        // 内部编号来自 EPLAN 2.9 官方功能定义表，避免把中文名称当成编号导出。
        string path = Path.Combine(AppContext.BaseDirectory, "Data", "FunctionDefinitions.json");
        return JsonSerializer.Deserialize<List<FunctionDefinition>>(File.ReadAllText(path))
               ?? throw new InvalidDataException("功能定义表为空。");
    }
}
