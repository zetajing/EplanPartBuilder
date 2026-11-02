namespace EplanPartBuilder;

public sealed class FunctionPickerForm : Form
{
    private readonly IReadOnlyList<FunctionDefinition> catalog;
    private readonly TextBox searchBox;
    private readonly ListBox results;
    private readonly Label descriptionLabel;
    public FunctionDefinition? SelectedDefinition { get; private set; }

    public FunctionPickerForm(IReadOnlyList<FunctionDefinition> definitions)
    {
        catalog = definitions;
        Text = "选择 EPLAN 2.9 功能定义";
        Font = new Font("Microsoft YaHei UI", 10);
        ClientSize = new Size(740, 540); MinimumSize = new Size(600, 400);
        StartPosition = FormStartPosition.CenterParent;
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(15), RowCount = 5, ColumnCount = 1 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45));
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.Controls.Add(new Label { Text = "输入名称或编号，例如：线圈、PLC、数字输入、300.1.2", Dock = DockStyle.Fill }, 0, 0);
        searchBox = new TextBox { Dock = DockStyle.Fill };
        searchBox.TextChanged += SearchBox_TextChanged;
        layout.Controls.Add(searchBox, 0, 1);
        results = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false, DisplayMember = nameof(FunctionDefinition.Display) };
        results.DoubleClick += ConfirmButton_Click;
        results.SelectedIndexChanged += Results_SelectedIndexChanged;
        layout.Controls.Add(results, 0, 2);
        descriptionLabel = new Label { Dock = DockStyle.Fill, ForeColor = Color.DimGray };
        layout.Controls.Add(descriptionLabel, 0, 3);
        var buttons = new FlowLayoutPanel { Dock = DockStyle.Fill, FlowDirection = FlowDirection.RightToLeft };
        var cancel = new Button { Text = "取消", DialogResult = DialogResult.Cancel, Width = 90 };
        var confirm = new Button { Text = "选用功能", Width = 110 };
        confirm.Click += ConfirmButton_Click;
        buttons.Controls.Add(cancel); buttons.Controls.Add(confirm);
        layout.Controls.Add(buttons, 0, 4); Controls.Add(layout);
        AcceptButton = confirm; CancelButton = cancel;
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
