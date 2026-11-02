namespace EplanPartBuilder;

partial class MainForm
{
    private TextBox partNumber = null!;
    private TextBox manufacturer = null!;
    private TextBox typeNumber = null!;
    private TextBox orderNumber = null!;
    private TextBox partName = null!;
    private TextBox description = null!;
    private TextBox characteristics = null!;
    private TextBox variant = null!;
    private ComboBox productGroup = null!;
    private NumericUpDown partWidth = null!;
    private NumericUpDown partHeight = null!;
    private NumericUpDown partDepth = null!;
    private TextBox schematicPath = null!;
    private TextBox layoutPath = null!;
    private TextBox stepPath = null!;
    private TextBox picturePath = null!;
    private TextBox documentPath = null!;
    private DataGridView functionGrid = null!;
    private TextBox resultBox = null!;
    private Label status = null!;
    private Label assetStatus = null!;
    private Button exportButton = null!;
    private Panel editorPanel = null!;
    private FlowLayoutPanel toolbar = null!;

    // 控件和布局集中在这个文件，业务按钮使用具名事件，便于逐步调试。
    private void InitializeComponent()
    {
        SuspendLayout();
        Text = "EPLAN 部件包编辑助手 · 1.1";
        Font = new Font("Microsoft YaHei UI", 9.5f);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(1180, 790);
        MinimumSize = new Size(1060, 720);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = Color.FromArgb(244, 246, 249);

        var shell = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 5, ColumnCount = 1 };
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 88));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 52));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 110));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 30));
        Controls.Add(shell);

        var header = new Panel { Dock = DockStyle.Fill, BackColor = Color.FromArgb(27, 43, 65), Margin = Padding.Empty };
        header.Controls.Add(new Label { Text = "EPLAN 部件打包助手", ForeColor = Color.White, Font = new Font(Font.FontFamily, 21, FontStyle.Bold), AutoSize = true, Location = new Point(22, 12) });
        header.Controls.Add(new Label { Text = "打开已有部件包 → 修改资料或替换文件 → 保留其他数据 → 生成 EDZ。", ForeColor = Color.FromArgb(192, 207, 224), AutoSize = true, Location = new Point(25, 56) });
        shell.Controls.Add(header, 0, 0);

        toolbar = new FlowLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(14, 7, 10, 4), WrapContents = false };
        toolbar.Controls.Add(MakeButton("新建部件", NewButton_Click, 92));
        toolbar.Controls.Add(MakeButton("打开 EDZ", OpenEdz_Click, 105));
        toolbar.Controls.Add(MakeButton("打开目录", OpenPackageFolder_Click, 105));
        toolbar.Controls.Add(MakeButton("打开资料", OpenButton_Click, 92));
        toolbar.Controls.Add(MakeButton("保存资料", SaveButton_Click, 92));
        toolbar.Controls.Add(MakeButton("继电器示例", ExampleButton_Click, 105));
        toolbar.Controls.Add(MakeButton("检查资料", ValidateButton_Click, 92));
        exportButton = MakeButton("生成 EDZ", ExportButton_Click, 130);
        exportButton.BackColor = Color.FromArgb(22, 112, 103);
        exportButton.ForeColor = Color.White;
        toolbar.Controls.Add(exportButton);
        shell.Controls.Add(toolbar, 0, 1);

        editorPanel = new Panel { Dock = DockStyle.Fill, Padding = new Padding(16, 4, 16, 4) };
        var tabs = new TabControl { Dock = DockStyle.Fill };
        var details = new TabPage("1  部件资料") { BackColor = Color.White, Padding = new Padding(18) };
        var files = new TabPage("2  宏与文件") { BackColor = Color.White, Padding = new Padding(18) };
        var functions = new TabPage("3  功能与端子") { BackColor = Color.White, Padding = new Padding(10) };
        var help = new TabPage("使用说明") { BackColor = Color.White, Padding = new Padding(18) };
        tabs.TabPages.AddRange(new[] { details, files, functions, help });
        BuildPackagePages(tabs);
        editorPanel.Controls.Add(tabs);
        shell.Controls.Add(editorPanel, 0, 2);

        var fields = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4 };
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115));
        fields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        partNumber = MakeTextBox(); manufacturer = MakeTextBox(); typeNumber = MakeTextBox(); orderNumber = MakeTextBox();
        partName = MakeTextBox(); characteristics = MakeTextBox(); variant = MakeTextBox();
        productGroup = new ComboBox { Dock = DockStyle.Fill, DropDownStyle = ComboBoxStyle.DropDownList };
        partWidth = MakeDimension(); partHeight = MakeDimension(); partDepth = MakeDimension();
        description = MakeTextBox(); description.Multiline = true; description.ScrollBars = ScrollBars.Vertical; description.Height = 76;
        AddField(fields, "部件编号 *", partNumber, 0, 0); AddField(fields, "制造商 *", manufacturer, 2, 0);
        AddField(fields, "型号", typeNumber, 0, 1); AddField(fields, "订货号", orderNumber, 2, 1);
        AddField(fields, "名称 *", partName, 0, 2); AddField(fields, "产品组", productGroup, 2, 2);
        AddField(fields, "技术参数", characteristics, 0, 3); AddField(fields, "变量", variant, 2, 3);
        AddField(fields, "宽度 / mm", partWidth, 0, 4); AddField(fields, "高度 / mm", partHeight, 2, 4);
        AddField(fields, "深度 / mm", partDepth, 0, 5);
        AddField(fields, "说明", description, 0, 6); fields.SetColumnSpan(description, 3);
        details.Controls.Add(fields);
        details.Controls.Add(new Label { Text = "* 为必填项。部件编号填写产品库编号，例如 DEMO.RELAY.24V；尺寸为 0 时表示尚未填写。", Dock = DockStyle.Bottom, Height = 40, ForeColor = Color.FromArgb(85, 100, 120) });

        var fileFields = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4 };
        fileFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145));
        fileFields.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100));
        fileFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85));
        fileFields.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70));
        schematicPath = MakeTextBox(); layoutPath = MakeTextBox(); stepPath = MakeTextBox(); picturePath = MakeTextBox(); documentPath = MakeTextBox();
        AddFileRow(fileFields, 0, "原理图宏", schematicPath, "EPLAN 宏|*.ema;*.ems", BrowseSchematic_Click, ClearSchematic_Click);
        AddFileRow(fileFields, 1, "3D / 安装宏", layoutPath, "EPLAN 3D 宏|*.ema", BrowseLayout_Click, ClearLayout_Click);
        AddFileRow(fileFields, 2, "外部文档 2 / STEP", stepPath, "模型或文档|*.step;*.stp;*.pdf", BrowseStep_Click, ClearStep_Click);
        AddFileRow(fileFields, 3, "产品图片", picturePath, "图片|*.png;*.jpg;*.jpeg;*.bmp", BrowsePicture_Click, ClearPicture_Click);
        AddFileRow(fileFields, 4, "产品手册", documentPath, "PDF 文档|*.pdf", BrowseDocument_Click, ClearDocument_Click);
        assetStatus = new Label { Dock = DockStyle.Top, Height = 120, Padding = new Padding(10), ForeColor = Color.FromArgb(115, 78, 17), BackColor = Color.FromArgb(255, 247, 226), Text = "文件用途\r\n原理图 .ema/.ems 与 3D .ema 分别关联到部件的不同宏属性。\r\nSTEP/STP 作为源文件附件保存，不会自动变成 3D 宏。要用于三维布局，请先在 EPLAN 2.9 中转换为 .ema。\r\n仅有 3D 外形还不代表已经定义安装方向、连接点或布线数据。" };
        var fileShell = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
        fileShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 230));
        fileShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 130));
        fileShell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        fileShell.Controls.Add(fileFields, 0, 0); fileShell.Controls.Add(assetStatus, 0, 1);
        files.Controls.Add(fileShell);

        var functionShell = new TableLayoutPanel { Dock = DockStyle.Fill, ColumnCount = 1, RowCount = 3 };
        functionShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 42));
        functionShell.RowStyles.Add(new RowStyle(SizeType.Absolute, 43));
        functionShell.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        functionShell.Controls.Add(new Label { Text = "每行表示一个电气功能。多个连接点用 | 分隔，例如 A1|A2。PLC 的插头和通道分别填写。\r\n宏不会在本软件中解析；功能模板请按实际产品填写，也可留空后在 EPLAN 中分配。", Dock = DockStyle.Fill }, 0, 0);
        var functionTools = new FlowLayoutPanel { Dock = DockStyle.Fill, WrapContents = false };
        functionTools.Controls.Add(MakeButton("添加功能", AddFunction_Click));
        functionTools.Controls.Add(MakeButton("更换功能", ChangeFunction_Click));
        functionTools.Controls.Add(MakeButton("删除行", DeleteFunction_Click));
        functionTools.Controls.Add(MakeButton("上移", MoveFunctionUp_Click, 70));
        functionTools.Controls.Add(MakeButton("下移", MoveFunctionDown_Click, 70));
        functionShell.Controls.Add(functionTools, 0, 1);
        functionGrid = new DataGridView { Dock = DockStyle.Fill, AutoGenerateColumns = false, AllowUserToAddRows = false, AllowUserToDeleteRows = false, AllowUserToResizeRows = false, MultiSelect = false, RowHeadersWidth = 42, BackgroundColor = Color.White, BorderStyle = BorderStyle.FixedSingle, SelectionMode = DataGridViewSelectionMode.CellSelect, RowTemplate = { Height = 32 } };
        AddColumn("Code", "功能定义", 280, true);
        AddColumn("Connections", "连接点（| 分隔）", 155);
        AddColumn("Plug", "插头", 75);
        AddColumn("Channel", "通道", 75);
        AddColumn("ConnectionDescriptions", "连接点描述（| 分隔）", 180);
        AddColumn("Characteristics", "技术参数", 130);
        AddColumn("Symbol", "符号库;类型;编号;变量", 200);
        functionGrid.CellFormatting += FunctionGrid_CellFormatting;
        functionGrid.RowPostPaint += FunctionGrid_RowPostPaint;
        functionGrid.DataError += FunctionGrid_DataError;
        functionShell.Controls.Add(functionGrid, 0, 2);
        functions.Controls.Add(functionShell);

        help.Controls.Add(new TextBox { Dock = DockStyle.Fill, ReadOnly = true, Multiline = true, ScrollBars = ScrollBars.Vertical, BorderStyle = BorderStyle.None, BackColor = Color.White, Text = PackageHelpText });

        resultBox = new TextBox { Dock = DockStyle.Fill, Multiline = true, ReadOnly = true, ScrollBars = ScrollBars.Vertical, BackColor = Color.White, Margin = new Padding(20, 4, 20, 4), Text = "就绪。先填写部件资料，再选择宏与文件。" };
        shell.Controls.Add(resultBox, 0, 3);
        status = new Label { Dock = DockStyle.Fill, Padding = new Padding(20, 3, 0, 0), ForeColor = Color.FromArgb(85, 100, 120) };
        shell.Controls.Add(status, 0, 4);
        FormClosing += MainForm_FormClosing;
        ResumeLayout(false);
    }

    private void AddColumn(string property, string title, int width, bool readOnly = false)
    {
        functionGrid.Columns.Add(new DataGridViewTextBoxColumn { DataPropertyName = property, Name = property, HeaderText = title, Width = width, ReadOnly = readOnly, SortMode = DataGridViewColumnSortMode.NotSortable });
    }

    private Button MakeButton(string text, EventHandler click, int width = 112)
    {
        var button = new Button { Text = text, Width = width, Height = 33, Margin = new Padding(0, 0, 8, 0), FlatStyle = FlatStyle.Flat, BackColor = Color.White };
        button.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        button.Click += click;
        return button;
    }
    private static TextBox MakeTextBox() => new() { Dock = DockStyle.Fill, Margin = new Padding(3, 6, 12, 6) };
    private static NumericUpDown MakeDimension() => new() { Dock = DockStyle.Fill, DecimalPlaces = 2, Maximum = 1000000, Margin = new Padding(3, 6, 12, 6) };
    private static void AddField(TableLayoutPanel table, string label, Control control, int column, int row)
    {
        table.Controls.Add(new Label { Text = label, AutoSize = true, Anchor = AnchorStyles.Left, Margin = new Padding(0, 8, 6, 8) }, column, row);
        table.Controls.Add(control, column + 1, row);
    }
    private void AddFileRow(TableLayoutPanel table, int row, string label, TextBox path, string filter, EventHandler browse, EventHandler clear)
    {
        path.ReadOnly = true; path.Tag = filter;
        table.RowStyles.Add(new RowStyle(SizeType.Absolute, 43));
        AddField(table, label, path, 0, row);
        table.Controls.Add(MakeButton("选择…", browse, 77), 2, row);
        table.Controls.Add(MakeButton("清除", clear, 62), 3, row);
    }
}

