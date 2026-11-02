namespace EplanPartBuilder;

public partial class FunctionPickerForm : Form
{
    private readonly IReadOnlyList<FunctionDefinition> catalog = Array.Empty<FunctionDefinition>();
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public FunctionDefinition? SelectedDefinition { get; private set; }

    public FunctionPickerForm()
    {
        InitializeComponent();
    }

    public FunctionPickerForm(IReadOnlyList<FunctionDefinition> definitions) : this()
    {
        catalog = definitions;
        UpdateResults();
    }

    private void SearchBox_TextChanged(object? sender, EventArgs e) => UpdateResults();
    private void UpdateResults()
    {
        string keyword = searchBox.Text.Trim();
        // 在中文名称、类别和内部编号中搜索，便于找到连接点用途接近的功能。
        results.DataSource = catalog.Where(d => (d.Code + " " + d.Name + " " + d.Category + " " + d.Group).Contains(keyword, StringComparison.OrdinalIgnoreCase)).ToList();
    }
    private void Results_SelectedIndexChanged(object? sender, EventArgs e)
    {
        if (results.SelectedItem is FunctionDefinition definition)
            descriptionLabel.Text = $"类别：{definition.Category}    组：{definition.Group}\r\n请按实际产品功能选用；标有“不再使用”的定义建议改用适合的新定义。";
        else descriptionLabel.Text = "没有匹配的功能定义。";
    }
    private void ConfirmButton_Click(object? sender, EventArgs e)
    {
        if (results.SelectedItem is not FunctionDefinition definition) return;
        SelectedDefinition = definition;
        DialogResult = DialogResult.OK;
        Close();
    }
}
