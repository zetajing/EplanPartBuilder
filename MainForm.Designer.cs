#nullable disable
namespace EplanPartBuilder;

partial class MainForm
{
    private System.ComponentModel.IContainer components = null;

    protected override void Dispose(bool disposing)
    {
        if (disposing && components != null) components.Dispose();
        base.Dispose(disposing);
    }

    // 固定控件直接声明和初始化，Visual Studio Designer 可读取和编辑。
    private void InitializeComponent()
    {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
        functionCodeColumn = new DataGridViewTextBoxColumn();
        functionConnectionsColumn = new DataGridViewTextBoxColumn();
        functionPlugColumn = new DataGridViewTextBoxColumn();
        functionChannelColumn = new DataGridViewTextBoxColumn();
        functionDescriptionColumn = new DataGridViewTextBoxColumn();
        functionCharacteristicsColumn = new DataGridViewTextBoxColumn();
        functionSymbolColumn = new DataGridViewTextBoxColumn();
        fileTypeColumn = new DataGridViewTextBoxColumn();
        fileNameColumn = new DataGridViewTextBoxColumn();
        filePathColumn = new DataGridViewTextBoxColumn();
        fileBytesColumn = new DataGridViewTextBoxColumn();
        attributeLocationColumn = new DataGridViewTextBoxColumn();
        attributeNameColumn = new DataGridViewTextBoxColumn();
        attributeValueColumn = new DataGridViewTextBoxColumn();
        pointPatternColumn = new DataGridViewTextBoxColumn();
        pointNameColumn = new DataGridViewTextBoxColumn();
        pointXColumn = new DataGridViewTextBoxColumn();
        pointYColumn = new DataGridViewTextBoxColumn();
        pointZColumn = new DataGridViewTextBoxColumn();
        pointXDirectionColumn = new DataGridViewTextBoxColumn();
        pointYDirectionColumn = new DataGridViewTextBoxColumn();
        pointZDirectionColumn = new DataGridViewTextBoxColumn();
        pointMinSectionColumn = new DataGridViewTextBoxColumn();
        pointMaxSectionColumn = new DataGridViewTextBoxColumn();
        shell = new TableLayoutPanel();
        headerPanel = new Panel();
        titleLabel = new Label();
        subtitleLabel = new Label();
        toolbar = new FlowLayoutPanel();
        newButton = new Button();
        openEdzButton = new Button();
        openFolderButton = new Button();
        openButton = new Button();
        saveButton = new Button();
        exampleButton = new Button();
        validateButton = new Button();
        exportButton = new Button();
        editorPanel = new Panel();
        editorTabs = new TabControl();
        detailsPage = new TabPage();
        detailsTable = new TableLayoutPanel();
        description = new TextBox();
        partNumberLabel = new Label();
        partNumber = new TextBox();
        manufacturerLabel = new Label();
        manufacturer = new TextBox();
        typeNumberLabel = new Label();
        typeNumber = new TextBox();
        orderNumberLabel = new Label();
        orderNumber = new TextBox();
        partNameLabel = new Label();
        partName = new TextBox();
        productGroupLabel = new Label();
        productGroup = new ComboBox();
        characteristicsLabel = new Label();
        characteristics = new TextBox();
        variantLabel = new Label();
        variant = new TextBox();
        partWidthLabel = new Label();
        partWidth = new NumericUpDown();
        partHeightLabel = new Label();
        partHeight = new NumericUpDown();
        partDepthLabel = new Label();
        partDepth = new NumericUpDown();
        descriptionLabel = new Label();
        detailsNote = new Label();
        filesPage = new TabPage();
        filesLayout = new TableLayoutPanel();
        filesTable = new TableLayoutPanel();
        schematicPathLabel = new Label();
        schematicPath = new TextBox();
        schematicPathBrowseButton = new Button();
        schematicPathClearButton = new Button();
        layoutPathLabel = new Label();
        layoutPath = new TextBox();
        layoutPathBrowseButton = new Button();
        layoutPathClearButton = new Button();
        stepPathLabel = new Label();
        stepPath = new TextBox();
        stepPathBrowseButton = new Button();
        stepPathClearButton = new Button();
        picturePathLabel = new Label();
        picturePath = new TextBox();
        picturePathBrowseButton = new Button();
        picturePathClearButton = new Button();
        documentPathLabel = new Label();
        documentPath = new TextBox();
        documentPathBrowseButton = new Button();
        documentPathClearButton = new Button();
        assetStatus = new Label();
        functionsPage = new TabPage();
        functionsLayout = new TableLayoutPanel();
        functionsNote = new Label();
        functionTools = new FlowLayoutPanel();
        addFunctionButton = new Button();
        changeFunctionButton = new Button();
        deleteFunctionButton = new Button();
        moveUpButton = new Button();
        moveDownButton = new Button();
        functionGrid = new DataGridView();
        technicalPage = new TabPage();
        technicalTable = new TableLayoutPanel();
        ratedCurrentLabel = new Label();
        ratedCurrent = new TextBox();
        voltageLabel = new Label();
        voltage = new TextBox();
        voltageTypeLabel = new Label();
        voltageType = new TextBox();
        powerDissipationLabel = new Label();
        powerDissipation = new TextBox();
        crossSectionLabel = new Label();
        crossSection = new TextBox();
        manufacturerNameLabel = new Label();
        manufacturerName = new TextBox();
        manufacturerCompanyLabel = new Label();
        manufacturerCompany = new TextBox();
        manufacturerEmailLabel = new Label();
        manufacturerEmail = new TextBox();
        manufacturerPhoneLabel = new Label();
        manufacturerPhone = new TextBox();
        technicalNote = new Label();
        originalPage = new TabPage();
        originalLayout = new TableLayoutPanel();
        packageSummary = new Label();
        originalTabs = new TabControl();
        originalFilesPage = new TabPage();
        originalFiles = new DataGridView();
        originalAttributesPage = new TabPage();
        originalAttributes = new DataGridView();
        connectionPointsPage = new TabPage();
        connectionPoints = new DataGridView();
        helpPage = new TabPage();
        helpTextBox = new TextBox();
        resultBox = new TextBox();
        status = new Label();
        shell.SuspendLayout();
        headerPanel.SuspendLayout();
        toolbar.SuspendLayout();
        editorPanel.SuspendLayout();
        editorTabs.SuspendLayout();
        detailsPage.SuspendLayout();
        detailsTable.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)partWidth).BeginInit();
        ((System.ComponentModel.ISupportInitialize)partHeight).BeginInit();
        ((System.ComponentModel.ISupportInitialize)partDepth).BeginInit();
        filesPage.SuspendLayout();
        filesLayout.SuspendLayout();
        filesTable.SuspendLayout();
        functionsPage.SuspendLayout();
        functionsLayout.SuspendLayout();
        functionTools.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)functionGrid).BeginInit();
        technicalPage.SuspendLayout();
        technicalTable.SuspendLayout();
        originalPage.SuspendLayout();
        originalLayout.SuspendLayout();
        originalTabs.SuspendLayout();
        originalFilesPage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)originalFiles).BeginInit();
        originalAttributesPage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)originalAttributes).BeginInit();
        connectionPointsPage.SuspendLayout();
        ((System.ComponentModel.ISupportInitialize)connectionPoints).BeginInit();
        helpPage.SuspendLayout();
        SuspendLayout();
        //
        // functionCodeColumn
        //
        functionCodeColumn.DataPropertyName = "Code";
        functionCodeColumn.HeaderText = "功能定义";
        functionCodeColumn.Name = "Code";
        functionCodeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        functionCodeColumn.Width = 280;
        functionCodeColumn.ReadOnly = true;
        //
        // functionConnectionsColumn
        //
        functionConnectionsColumn.DataPropertyName = "Connections";
        functionConnectionsColumn.HeaderText = "连接点（| 分隔）";
        functionConnectionsColumn.Name = "Connections";
        functionConnectionsColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        functionConnectionsColumn.Width = 155;
        //
        // functionPlugColumn
        //
        functionPlugColumn.DataPropertyName = "Plug";
        functionPlugColumn.HeaderText = "插头";
        functionPlugColumn.Name = "Plug";
        functionPlugColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        functionPlugColumn.Width = 75;
        //
        // functionChannelColumn
        //
        functionChannelColumn.DataPropertyName = "Channel";
        functionChannelColumn.HeaderText = "通道";
        functionChannelColumn.Name = "Channel";
        functionChannelColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        functionChannelColumn.Width = 75;
        //
        // functionDescriptionColumn
        //
        functionDescriptionColumn.DataPropertyName = "ConnectionDescriptions";
        functionDescriptionColumn.HeaderText = "连接点描述（| 分隔）";
        functionDescriptionColumn.Name = "ConnectionDescriptions";
        functionDescriptionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        functionDescriptionColumn.Width = 180;
        //
        // functionCharacteristicsColumn
        //
        functionCharacteristicsColumn.DataPropertyName = "Characteristics";
        functionCharacteristicsColumn.HeaderText = "技术参数";
        functionCharacteristicsColumn.Name = "Characteristics";
        functionCharacteristicsColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        functionCharacteristicsColumn.Width = 130;
        //
        // functionSymbolColumn
        //
        functionSymbolColumn.DataPropertyName = "Symbol";
        functionSymbolColumn.HeaderText = "符号库;类型;编号;变量";
        functionSymbolColumn.Name = "Symbol";
        functionSymbolColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        functionSymbolColumn.Width = 200;
        //
        // fileTypeColumn
        //
        fileTypeColumn.DataPropertyName = "类型";
        fileTypeColumn.HeaderText = "类型";
        fileTypeColumn.Name = "fileTypeColumn";
        fileTypeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        fileTypeColumn.Width = 80;
        //
        // fileNameColumn
        //
        fileNameColumn.DataPropertyName = "名称";
        fileNameColumn.HeaderText = "名称";
        fileNameColumn.Name = "fileNameColumn";
        fileNameColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        fileNameColumn.Width = 140;
        //
        // filePathColumn
        //
        filePathColumn.DataPropertyName = "文件路径";
        filePathColumn.HeaderText = "文件路径";
        filePathColumn.Name = "filePathColumn";
        filePathColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        filePathColumn.Width = 500;
        //
        // fileBytesColumn
        //
        fileBytesColumn.DataPropertyName = "字节数";
        fileBytesColumn.HeaderText = "字节数";
        fileBytesColumn.Name = "fileBytesColumn";
        fileBytesColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        fileBytesColumn.Width = 100;
        //
        // attributeLocationColumn
        //
        attributeLocationColumn.DataPropertyName = "位置";
        attributeLocationColumn.HeaderText = "位置";
        attributeLocationColumn.Name = "attributeLocationColumn";
        attributeLocationColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        attributeLocationColumn.Width = 85;
        //
        // attributeNameColumn
        //
        attributeNameColumn.DataPropertyName = "属性";
        attributeNameColumn.HeaderText = "属性";
        attributeNameColumn.Name = "attributeNameColumn";
        attributeNameColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        attributeNameColumn.Width = 280;
        //
        // attributeValueColumn
        //
        attributeValueColumn.DataPropertyName = "原始值";
        attributeValueColumn.HeaderText = "原始值";
        attributeValueColumn.Name = "attributeValueColumn";
        attributeValueColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        attributeValueColumn.Width = 650;
        //
        // pointPatternColumn
        //
        pointPatternColumn.DataPropertyName = "排列样式";
        pointPatternColumn.HeaderText = "排列样式";
        pointPatternColumn.Name = "pointPatternColumn";
        pointPatternColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointPatternColumn.Width = 235;
        //
        // pointNameColumn
        //
        pointNameColumn.DataPropertyName = "端子";
        pointNameColumn.HeaderText = "端子";
        pointNameColumn.Name = "pointNameColumn";
        pointNameColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointNameColumn.Width = 70;
        //
        // pointXColumn
        //
        pointXColumn.DataPropertyName = "X";
        pointXColumn.HeaderText = "X";
        pointXColumn.Name = "pointXColumn";
        pointXColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointXColumn.Width = 115;
        //
        // pointYColumn
        //
        pointYColumn.DataPropertyName = "Y";
        pointYColumn.HeaderText = "Y";
        pointYColumn.Name = "pointYColumn";
        pointYColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointYColumn.Width = 115;
        //
        // pointZColumn
        //
        pointZColumn.DataPropertyName = "Z";
        pointZColumn.HeaderText = "Z";
        pointZColumn.Name = "pointZColumn";
        pointZColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointZColumn.Width = 115;
        //
        // pointXDirectionColumn
        //
        pointXDirectionColumn.DataPropertyName = "X方向";
        pointXDirectionColumn.HeaderText = "X方向";
        pointXDirectionColumn.Name = "pointXDirectionColumn";
        pointXDirectionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointXDirectionColumn.Width = 80;
        //
        // pointYDirectionColumn
        //
        pointYDirectionColumn.DataPropertyName = "Y方向";
        pointYDirectionColumn.HeaderText = "Y方向";
        pointYDirectionColumn.Name = "pointYDirectionColumn";
        pointYDirectionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointYDirectionColumn.Width = 80;
        //
        // pointZDirectionColumn
        //
        pointZDirectionColumn.DataPropertyName = "Z方向";
        pointZDirectionColumn.HeaderText = "Z方向";
        pointZDirectionColumn.Name = "pointZDirectionColumn";
        pointZDirectionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointZDirectionColumn.Width = 80;
        //
        // pointMinSectionColumn
        //
        pointMinSectionColumn.DataPropertyName = "最小截面";
        pointMinSectionColumn.HeaderText = "最小截面";
        pointMinSectionColumn.Name = "pointMinSectionColumn";
        pointMinSectionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointMinSectionColumn.Width = 95;
        //
        // pointMaxSectionColumn
        //
        pointMaxSectionColumn.DataPropertyName = "最大截面";
        pointMaxSectionColumn.HeaderText = "最大截面";
        pointMaxSectionColumn.Name = "pointMaxSectionColumn";
        pointMaxSectionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
        pointMaxSectionColumn.Width = 95;
        //
        // shell
        //
        shell.ColumnCount = 1;
        shell.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        shell.Controls.Add(headerPanel, 0, 0);
        shell.Controls.Add(toolbar, 0, 1);
        shell.Controls.Add(editorPanel, 0, 2);
        shell.Controls.Add(resultBox, 0, 3);
        shell.Controls.Add(status, 0, 4);
        shell.Dock = DockStyle.Fill;
        shell.Location = new Point(0, 0);
        shell.Margin = new Padding(0);
        shell.Name = "shell";
        shell.RowCount = 5;
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 52F));
        shell.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 110F));
        shell.RowStyles.Add(new RowStyle(SizeType.Absolute, 30F));
        shell.Size = new Size(1180, 790);
        shell.TabIndex = 0;
        //
        // headerPanel
        //
        headerPanel.BackColor = Color.FromArgb(27, 43, 65);
        headerPanel.Controls.Add(titleLabel);
        headerPanel.Controls.Add(subtitleLabel);
        headerPanel.Dock = DockStyle.Fill;
        headerPanel.Location = new Point(0, 0);
        headerPanel.Margin = new Padding(0);
        headerPanel.Name = "headerPanel";
        headerPanel.Size = new Size(1180, 88);
        headerPanel.TabIndex = 0;
        //
        // titleLabel
        //
        titleLabel.AutoSize = true;
        titleLabel.Font = new Font("Microsoft YaHei UI", 21F, FontStyle.Bold);
        titleLabel.ForeColor = Color.White;
        titleLabel.Location = new Point(22, 12);
        titleLabel.Name = "titleLabel";
        titleLabel.Size = new Size(287, 37);
        titleLabel.TabIndex = 0;
        titleLabel.Text = "EPLAN 部件打包助手";
        //
        // subtitleLabel
        //
        subtitleLabel.AutoSize = true;
        subtitleLabel.ForeColor = Color.FromArgb(192, 207, 224);
        subtitleLabel.Location = new Point(25, 56);
        subtitleLabel.Name = "subtitleLabel";
        subtitleLabel.Size = new Size(426, 19);
        subtitleLabel.TabIndex = 1;
        subtitleLabel.Text = "打开已有部件包 → 修改资料或替换文件 → 保留其他数据 → 生成 EDZ。";
        //
        // toolbar
        //
        toolbar.Controls.Add(newButton);
        toolbar.Controls.Add(openEdzButton);
        toolbar.Controls.Add(openFolderButton);
        toolbar.Controls.Add(openButton);
        toolbar.Controls.Add(saveButton);
        toolbar.Controls.Add(exampleButton);
        toolbar.Controls.Add(validateButton);
        toolbar.Controls.Add(exportButton);
        toolbar.Dock = DockStyle.Fill;
        toolbar.Location = new Point(3, 91);
        toolbar.Name = "toolbar";
        toolbar.Padding = new Padding(14, 7, 10, 4);
        toolbar.Size = new Size(1174, 46);
        toolbar.TabIndex = 1;
        toolbar.WrapContents = false;
        //
        // newButton
        //
        newButton.BackColor = Color.White;
        newButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        newButton.FlatStyle = FlatStyle.Flat;
        newButton.Location = new Point(14, 7);
        newButton.Margin = new Padding(0, 0, 8, 0);
        newButton.Name = "newButton";
        newButton.Size = new Size(92, 33);
        newButton.TabIndex = 0;
        newButton.Text = "新建部件";
        newButton.UseVisualStyleBackColor = false;
        newButton.Click += NewButton_Click;
        //
        // openEdzButton
        //
        openEdzButton.BackColor = Color.White;
        openEdzButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        openEdzButton.FlatStyle = FlatStyle.Flat;
        openEdzButton.Location = new Point(114, 7);
        openEdzButton.Margin = new Padding(0, 0, 8, 0);
        openEdzButton.Name = "openEdzButton";
        openEdzButton.Size = new Size(105, 33);
        openEdzButton.TabIndex = 1;
        openEdzButton.Text = "打开 EDZ";
        openEdzButton.UseVisualStyleBackColor = false;
        openEdzButton.Click += OpenEdz_Click;
        //
        // openFolderButton
        //
        openFolderButton.BackColor = Color.White;
        openFolderButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        openFolderButton.FlatStyle = FlatStyle.Flat;
        openFolderButton.Location = new Point(227, 7);
        openFolderButton.Margin = new Padding(0, 0, 8, 0);
        openFolderButton.Name = "openFolderButton";
        openFolderButton.Size = new Size(105, 33);
        openFolderButton.TabIndex = 2;
        openFolderButton.Text = "打开目录";
        openFolderButton.UseVisualStyleBackColor = false;
        openFolderButton.Click += OpenPackageFolder_Click;
        //
        // openButton
        //
        openButton.BackColor = Color.White;
        openButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        openButton.FlatStyle = FlatStyle.Flat;
        openButton.Location = new Point(340, 7);
        openButton.Margin = new Padding(0, 0, 8, 0);
        openButton.Name = "openButton";
        openButton.Size = new Size(92, 33);
        openButton.TabIndex = 3;
        openButton.Text = "打开资料";
        openButton.UseVisualStyleBackColor = false;
        openButton.Click += OpenButton_Click;
        //
        // saveButton
        //
        saveButton.BackColor = Color.White;
        saveButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        saveButton.FlatStyle = FlatStyle.Flat;
        saveButton.Location = new Point(440, 7);
        saveButton.Margin = new Padding(0, 0, 8, 0);
        saveButton.Name = "saveButton";
        saveButton.Size = new Size(92, 33);
        saveButton.TabIndex = 4;
        saveButton.Text = "保存资料";
        saveButton.UseVisualStyleBackColor = false;
        saveButton.Click += SaveButton_Click;
        //
        // exampleButton
        //
        exampleButton.BackColor = Color.White;
        exampleButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        exampleButton.FlatStyle = FlatStyle.Flat;
        exampleButton.Location = new Point(540, 7);
        exampleButton.Margin = new Padding(0, 0, 8, 0);
        exampleButton.Name = "exampleButton";
        exampleButton.Size = new Size(105, 33);
        exampleButton.TabIndex = 5;
        exampleButton.Text = "继电器示例";
        exampleButton.UseVisualStyleBackColor = false;
        exampleButton.Click += ExampleButton_Click;
        //
        // validateButton
        //
        validateButton.BackColor = Color.White;
        validateButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        validateButton.FlatStyle = FlatStyle.Flat;
        validateButton.Location = new Point(653, 7);
        validateButton.Margin = new Padding(0, 0, 8, 0);
        validateButton.Name = "validateButton";
        validateButton.Size = new Size(92, 33);
        validateButton.TabIndex = 6;
        validateButton.Text = "检查资料";
        validateButton.UseVisualStyleBackColor = false;
        validateButton.Click += ValidateButton_Click;
        //
        // exportButton
        //
        exportButton.BackColor = Color.FromArgb(22, 112, 103);
        exportButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        exportButton.FlatStyle = FlatStyle.Flat;
        exportButton.ForeColor = Color.White;
        exportButton.Location = new Point(753, 7);
        exportButton.Margin = new Padding(0, 0, 8, 0);
        exportButton.Name = "exportButton";
        exportButton.Size = new Size(130, 33);
        exportButton.TabIndex = 7;
        exportButton.Text = "生成 EDZ";
        exportButton.UseVisualStyleBackColor = false;
        exportButton.Click += ExportButton_Click;
        //
        // editorPanel
        //
        editorPanel.Controls.Add(editorTabs);
        editorPanel.Dock = DockStyle.Fill;
        editorPanel.Location = new Point(3, 143);
        editorPanel.Name = "editorPanel";
        editorPanel.Padding = new Padding(16, 4, 16, 4);
        editorPanel.Size = new Size(1174, 504);
        editorPanel.TabIndex = 2;
        //
        // editorTabs
        //
        editorTabs.Controls.Add(detailsPage);
        editorTabs.Controls.Add(filesPage);
        editorTabs.Controls.Add(functionsPage);
        editorTabs.Controls.Add(technicalPage);
        editorTabs.Controls.Add(originalPage);
        editorTabs.Controls.Add(helpPage);
        editorTabs.Dock = DockStyle.Fill;
        editorTabs.Location = new Point(16, 4);
        editorTabs.Name = "editorTabs";
        editorTabs.SelectedIndex = 0;
        editorTabs.Size = new Size(1142, 496);
        editorTabs.TabIndex = 0;
        //
        // detailsPage
        //
        detailsPage.BackColor = Color.White;
        detailsPage.Controls.Add(detailsTable);
        detailsPage.Controls.Add(detailsNote);
        detailsPage.Location = new Point(4, 28);
        detailsPage.Name = "detailsPage";
        detailsPage.Padding = new Padding(18);
        detailsPage.Size = new Size(1134, 464);
        detailsPage.TabIndex = 0;
        detailsPage.Text = "1  部件资料";
        //
        // detailsTable
        //
        detailsTable.AutoSize = true;
        detailsTable.ColumnCount = 4;
        detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115F));
        detailsTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        detailsTable.Controls.Add(partNumberLabel, 0, 0);
        detailsTable.Controls.Add(partNumber, 1, 0);
        detailsTable.Controls.Add(manufacturerLabel, 2, 0);
        detailsTable.Controls.Add(manufacturer, 3, 0);
        detailsTable.Controls.Add(typeNumberLabel, 0, 1);
        detailsTable.Controls.Add(typeNumber, 1, 1);
        detailsTable.Controls.Add(orderNumberLabel, 2, 1);
        detailsTable.Controls.Add(orderNumber, 3, 1);
        detailsTable.Controls.Add(partNameLabel, 0, 2);
        detailsTable.Controls.Add(partName, 1, 2);
        detailsTable.Controls.Add(productGroupLabel, 2, 2);
        detailsTable.Controls.Add(productGroup, 3, 2);
        detailsTable.Controls.Add(characteristicsLabel, 0, 3);
        detailsTable.Controls.Add(characteristics, 1, 3);
        detailsTable.Controls.Add(variantLabel, 2, 3);
        detailsTable.Controls.Add(variant, 3, 3);
        detailsTable.Controls.Add(partWidthLabel, 0, 4);
        detailsTable.Controls.Add(partWidth, 1, 4);
        detailsTable.Controls.Add(partHeightLabel, 2, 4);
        detailsTable.Controls.Add(partHeight, 3, 4);
        detailsTable.Controls.Add(partDepthLabel, 0, 5);
        detailsTable.Controls.Add(partDepth, 1, 5);
        detailsTable.Controls.Add(descriptionLabel, 0, 6);
        detailsTable.Controls.Add(description, 1, 6);
        detailsTable.Dock = DockStyle.Top;
        detailsTable.Location = new Point(18, 18);
        detailsTable.Name = "detailsTable";
        detailsTable.RowCount = 7;
        detailsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        detailsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        detailsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        detailsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        detailsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        detailsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        detailsTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 88F));
        detailsTable.Size = new Size(1098, 304);
        detailsTable.TabIndex = 0;
        //
        // description
        //
        detailsTable.SetColumnSpan(description, 3);
        description.Dock = DockStyle.Fill;
        description.Location = new Point(113, 222);
        description.Margin = new Padding(3, 6, 12, 6);
        description.Multiline = true;
        description.Name = "description";
        description.ScrollBars = ScrollBars.Vertical;
        description.Size = new Size(973, 76);
        description.TabIndex = 23;
        //
        // partNumberLabel
        //
        partNumberLabel.Anchor = AnchorStyles.Left;
        partNumberLabel.AutoSize = true;
        partNumberLabel.Location = new Point(0, 8);
        partNumberLabel.Margin = new Padding(0, 8, 6, 8);
        partNumberLabel.Name = "partNumberLabel";
        partNumberLabel.Size = new Size(71, 19);
        partNumberLabel.TabIndex = 0;
        partNumberLabel.Text = "部件编号 *";
        //
        // partNumber
        //
        partNumber.Dock = DockStyle.Fill;
        partNumber.Location = new Point(113, 6);
        partNumber.Margin = new Padding(3, 6, 12, 6);
        partNumber.Name = "partNumber";
        partNumber.Size = new Size(421, 24);
        partNumber.TabIndex = 1;
        //
        // manufacturerLabel
        //
        manufacturerLabel.Anchor = AnchorStyles.Left;
        manufacturerLabel.AutoSize = true;
        manufacturerLabel.Location = new Point(546, 8);
        manufacturerLabel.Margin = new Padding(0, 8, 6, 8);
        manufacturerLabel.Name = "manufacturerLabel";
        manufacturerLabel.Size = new Size(58, 19);
        manufacturerLabel.TabIndex = 2;
        manufacturerLabel.Text = "制造商 *";
        //
        // manufacturer
        //
        manufacturer.Dock = DockStyle.Fill;
        manufacturer.Location = new Point(664, 6);
        manufacturer.Margin = new Padding(3, 6, 12, 6);
        manufacturer.Name = "manufacturer";
        manufacturer.Size = new Size(422, 24);
        manufacturer.TabIndex = 3;
        //
        // typeNumberLabel
        //
        typeNumberLabel.Anchor = AnchorStyles.Left;
        typeNumberLabel.AutoSize = true;
        typeNumberLabel.Location = new Point(0, 44);
        typeNumberLabel.Margin = new Padding(0, 8, 6, 8);
        typeNumberLabel.Name = "typeNumberLabel";
        typeNumberLabel.Size = new Size(35, 19);
        typeNumberLabel.TabIndex = 4;
        typeNumberLabel.Text = "型号";
        //
        // typeNumber
        //
        typeNumber.Dock = DockStyle.Fill;
        typeNumber.Location = new Point(113, 42);
        typeNumber.Margin = new Padding(3, 6, 12, 6);
        typeNumber.Name = "typeNumber";
        typeNumber.Size = new Size(421, 24);
        typeNumber.TabIndex = 5;
        //
        // orderNumberLabel
        //
        orderNumberLabel.Anchor = AnchorStyles.Left;
        orderNumberLabel.AutoSize = true;
        orderNumberLabel.Location = new Point(546, 44);
        orderNumberLabel.Margin = new Padding(0, 8, 6, 8);
        orderNumberLabel.Name = "orderNumberLabel";
        orderNumberLabel.Size = new Size(48, 19);
        orderNumberLabel.TabIndex = 6;
        orderNumberLabel.Text = "订货号";
        //
        // orderNumber
        //
        orderNumber.Dock = DockStyle.Fill;
        orderNumber.Location = new Point(664, 42);
        orderNumber.Margin = new Padding(3, 6, 12, 6);
        orderNumber.Name = "orderNumber";
        orderNumber.Size = new Size(422, 24);
        orderNumber.TabIndex = 7;
        //
        // partNameLabel
        //
        partNameLabel.Anchor = AnchorStyles.Left;
        partNameLabel.AutoSize = true;
        partNameLabel.Location = new Point(0, 80);
        partNameLabel.Margin = new Padding(0, 8, 6, 8);
        partNameLabel.Name = "partNameLabel";
        partNameLabel.Size = new Size(45, 19);
        partNameLabel.TabIndex = 8;
        partNameLabel.Text = "名称 *";
        //
        // partName
        //
        partName.Dock = DockStyle.Fill;
        partName.Location = new Point(113, 78);
        partName.Margin = new Padding(3, 6, 12, 6);
        partName.Name = "partName";
        partName.Size = new Size(421, 24);
        partName.TabIndex = 9;
        //
        // productGroupLabel
        //
        productGroupLabel.Anchor = AnchorStyles.Left;
        productGroupLabel.AutoSize = true;
        productGroupLabel.Location = new Point(546, 80);
        productGroupLabel.Margin = new Padding(0, 8, 6, 8);
        productGroupLabel.Name = "productGroupLabel";
        productGroupLabel.Size = new Size(48, 19);
        productGroupLabel.TabIndex = 10;
        productGroupLabel.Text = "产品组";
        //
        // productGroup
        //
        productGroup.Dock = DockStyle.Fill;
        productGroup.DropDownStyle = ComboBoxStyle.DropDownList;
        productGroup.Items.AddRange(new object[] { "未分类（0）", "继电器 / 接触器（2）", "端子（3）", "插头（4）", "常规设备（5）", "功率开关设备（6）", "电机（9）", "电源（20）", "PLC（26）" });
        productGroup.Location = new Point(664, 78);
        productGroup.Margin = new Padding(3, 6, 12, 6);
        productGroup.Name = "productGroup";
        productGroup.Size = new Size(422, 27);
        productGroup.TabIndex = 11;
        //
        // characteristicsLabel
        //
        characteristicsLabel.Anchor = AnchorStyles.Left;
        characteristicsLabel.AutoSize = true;
        characteristicsLabel.Location = new Point(0, 116);
        characteristicsLabel.Margin = new Padding(0, 8, 6, 8);
        characteristicsLabel.Name = "characteristicsLabel";
        characteristicsLabel.Size = new Size(61, 19);
        characteristicsLabel.TabIndex = 12;
        characteristicsLabel.Text = "技术参数";
        //
        // characteristics
        //
        characteristics.Dock = DockStyle.Fill;
        characteristics.Location = new Point(113, 114);
        characteristics.Margin = new Padding(3, 6, 12, 6);
        characteristics.Name = "characteristics";
        characteristics.Size = new Size(421, 24);
        characteristics.TabIndex = 13;
        //
        // variantLabel
        //
        variantLabel.Anchor = AnchorStyles.Left;
        variantLabel.AutoSize = true;
        variantLabel.Location = new Point(546, 116);
        variantLabel.Margin = new Padding(0, 8, 6, 8);
        variantLabel.Name = "variantLabel";
        variantLabel.Size = new Size(35, 19);
        variantLabel.TabIndex = 14;
        variantLabel.Text = "变量";
        //
        // variant
        //
        variant.Dock = DockStyle.Fill;
        variant.Location = new Point(664, 114);
        variant.Margin = new Padding(3, 6, 12, 6);
        variant.Name = "variant";
        variant.Size = new Size(422, 24);
        variant.TabIndex = 15;
        variant.Text = "1";
        //
        // partWidthLabel
        //
        partWidthLabel.Anchor = AnchorStyles.Left;
        partWidthLabel.AutoSize = true;
        partWidthLabel.Location = new Point(0, 152);
        partWidthLabel.Margin = new Padding(0, 8, 6, 8);
        partWidthLabel.Name = "partWidthLabel";
        partWidthLabel.Size = new Size(73, 19);
        partWidthLabel.TabIndex = 16;
        partWidthLabel.Text = "宽度 / mm";
        //
        // partWidth
        //
        partWidth.DecimalPlaces = 2;
        partWidth.Dock = DockStyle.Fill;
        partWidth.Location = new Point(113, 150);
        partWidth.Margin = new Padding(3, 6, 12, 6);
        partWidth.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        partWidth.Name = "partWidth";
        partWidth.Size = new Size(421, 24);
        partWidth.TabIndex = 17;
        //
        // partHeightLabel
        //
        partHeightLabel.Anchor = AnchorStyles.Left;
        partHeightLabel.AutoSize = true;
        partHeightLabel.Location = new Point(546, 152);
        partHeightLabel.Margin = new Padding(0, 8, 6, 8);
        partHeightLabel.Name = "partHeightLabel";
        partHeightLabel.Size = new Size(73, 19);
        partHeightLabel.TabIndex = 18;
        partHeightLabel.Text = "高度 / mm";
        //
        // partHeight
        //
        partHeight.DecimalPlaces = 2;
        partHeight.Dock = DockStyle.Fill;
        partHeight.Location = new Point(664, 150);
        partHeight.Margin = new Padding(3, 6, 12, 6);
        partHeight.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        partHeight.Name = "partHeight";
        partHeight.Size = new Size(422, 24);
        partHeight.TabIndex = 19;
        //
        // partDepthLabel
        //
        partDepthLabel.Anchor = AnchorStyles.Left;
        partDepthLabel.AutoSize = true;
        partDepthLabel.Location = new Point(0, 188);
        partDepthLabel.Margin = new Padding(0, 8, 6, 8);
        partDepthLabel.Name = "partDepthLabel";
        partDepthLabel.Size = new Size(73, 19);
        partDepthLabel.TabIndex = 20;
        partDepthLabel.Text = "深度 / mm";
        //
        // partDepth
        //
        partDepth.DecimalPlaces = 2;
        partDepth.Dock = DockStyle.Fill;
        partDepth.Location = new Point(113, 186);
        partDepth.Margin = new Padding(3, 6, 12, 6);
        partDepth.Maximum = new decimal(new int[] { 1000000, 0, 0, 0 });
        partDepth.Name = "partDepth";
        partDepth.Size = new Size(421, 24);
        partDepth.TabIndex = 21;
        //
        // descriptionLabel
        //
        descriptionLabel.Anchor = AnchorStyles.Left;
        descriptionLabel.AutoSize = true;
        descriptionLabel.Location = new Point(0, 250);
        descriptionLabel.Margin = new Padding(0, 8, 6, 8);
        descriptionLabel.Name = "descriptionLabel";
        descriptionLabel.Size = new Size(35, 19);
        descriptionLabel.TabIndex = 22;
        descriptionLabel.Text = "说明";
        //
        // detailsNote
        //
        detailsNote.Dock = DockStyle.Bottom;
        detailsNote.ForeColor = Color.FromArgb(85, 100, 120);
        detailsNote.Location = new Point(18, 406);
        detailsNote.Name = "detailsNote";
        detailsNote.Size = new Size(1098, 40);
        detailsNote.TabIndex = 1;
        detailsNote.Text = "* 为必填项。部件编号填写产品库编号，例如 DEMO.RELAY.24V；尺寸为 0 时表示尚未填写。";
        //
        // filesPage
        //
        filesPage.BackColor = Color.White;
        filesPage.Controls.Add(filesLayout);
        filesPage.Location = new Point(4, 28);
        filesPage.Name = "filesPage";
        filesPage.Padding = new Padding(18);
        filesPage.Size = new Size(154, 0);
        filesPage.TabIndex = 1;
        filesPage.Text = "2  宏与文件";
        //
        // filesLayout
        //
        filesLayout.ColumnCount = 1;
        filesLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        filesLayout.Controls.Add(filesTable, 0, 0);
        filesLayout.Controls.Add(assetStatus, 0, 1);
        filesLayout.Dock = DockStyle.Fill;
        filesLayout.Location = new Point(18, 18);
        filesLayout.Name = "filesLayout";
        filesLayout.RowCount = 3;
        filesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 230F));
        filesLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 130F));
        filesLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        filesLayout.Size = new Size(118, 0);
        filesLayout.TabIndex = 0;
        //
        // filesTable
        //
        filesTable.AutoSize = true;
        filesTable.ColumnCount = 4;
        filesTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 145F));
        filesTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        filesTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 85F));
        filesTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 70F));
        filesTable.Controls.Add(schematicPathLabel, 0, 0);
        filesTable.Controls.Add(schematicPath, 1, 0);
        filesTable.Controls.Add(schematicPathBrowseButton, 2, 0);
        filesTable.Controls.Add(schematicPathClearButton, 3, 0);
        filesTable.Controls.Add(layoutPathLabel, 0, 1);
        filesTable.Controls.Add(layoutPath, 1, 1);
        filesTable.Controls.Add(layoutPathBrowseButton, 2, 1);
        filesTable.Controls.Add(layoutPathClearButton, 3, 1);
        filesTable.Controls.Add(stepPathLabel, 0, 2);
        filesTable.Controls.Add(stepPath, 1, 2);
        filesTable.Controls.Add(stepPathBrowseButton, 2, 2);
        filesTable.Controls.Add(stepPathClearButton, 3, 2);
        filesTable.Controls.Add(picturePathLabel, 0, 3);
        filesTable.Controls.Add(picturePath, 1, 3);
        filesTable.Controls.Add(picturePathBrowseButton, 2, 3);
        filesTable.Controls.Add(picturePathClearButton, 3, 3);
        filesTable.Controls.Add(documentPathLabel, 0, 4);
        filesTable.Controls.Add(documentPath, 1, 4);
        filesTable.Controls.Add(documentPathBrowseButton, 2, 4);
        filesTable.Controls.Add(documentPathClearButton, 3, 4);
        filesTable.Dock = DockStyle.Top;
        filesTable.Location = new Point(3, 3);
        filesTable.Name = "filesTable";
        filesTable.RowCount = 5;
        filesTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        filesTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        filesTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        filesTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        filesTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        filesTable.Size = new Size(112, 215);
        filesTable.TabIndex = 0;
        //
        // schematicPathLabel
        //
        schematicPathLabel.Anchor = AnchorStyles.Left;
        schematicPathLabel.AutoSize = true;
        schematicPathLabel.Location = new Point(0, 12);
        schematicPathLabel.Margin = new Padding(0, 8, 6, 8);
        schematicPathLabel.Name = "schematicPathLabel";
        schematicPathLabel.Size = new Size(61, 19);
        schematicPathLabel.TabIndex = 0;
        schematicPathLabel.Text = "原理图宏";
        //
        // schematicPath
        //
        schematicPath.Dock = DockStyle.Fill;
        schematicPath.Location = new Point(148, 6);
        schematicPath.Margin = new Padding(3, 6, 12, 6);
        schematicPath.Name = "schematicPath";
        schematicPath.ReadOnly = true;
        schematicPath.Size = new Size(1, 24);
        schematicPath.TabIndex = 1;
        schematicPath.Tag = "EPLAN 宏|*.ema;*.ems";
        //
        // schematicPathBrowseButton
        //
        schematicPathBrowseButton.BackColor = Color.White;
        schematicPathBrowseButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        schematicPathBrowseButton.FlatStyle = FlatStyle.Flat;
        schematicPathBrowseButton.Location = new Point(-43, 0);
        schematicPathBrowseButton.Margin = new Padding(0, 0, 8, 0);
        schematicPathBrowseButton.Name = "schematicPathBrowseButton";
        schematicPathBrowseButton.Size = new Size(77, 33);
        schematicPathBrowseButton.TabIndex = 2;
        schematicPathBrowseButton.Text = "选择…";
        schematicPathBrowseButton.UseVisualStyleBackColor = false;
        schematicPathBrowseButton.Click += BrowseSchematic_Click;
        //
        // schematicPathClearButton
        //
        schematicPathClearButton.BackColor = Color.White;
        schematicPathClearButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        schematicPathClearButton.FlatStyle = FlatStyle.Flat;
        schematicPathClearButton.Location = new Point(42, 0);
        schematicPathClearButton.Margin = new Padding(0, 0, 8, 0);
        schematicPathClearButton.Name = "schematicPathClearButton";
        schematicPathClearButton.Size = new Size(62, 33);
        schematicPathClearButton.TabIndex = 3;
        schematicPathClearButton.Text = "清除";
        schematicPathClearButton.UseVisualStyleBackColor = false;
        schematicPathClearButton.Click += ClearSchematic_Click;
        //
        // layoutPathLabel
        //
        layoutPathLabel.Anchor = AnchorStyles.Left;
        layoutPathLabel.AutoSize = true;
        layoutPathLabel.Location = new Point(0, 55);
        layoutPathLabel.Margin = new Padding(0, 8, 6, 8);
        layoutPathLabel.Name = "layoutPathLabel";
        layoutPathLabel.Size = new Size(80, 19);
        layoutPathLabel.TabIndex = 4;
        layoutPathLabel.Text = "3D / 安装宏";
        //
        // layoutPath
        //
        layoutPath.Dock = DockStyle.Fill;
        layoutPath.Location = new Point(148, 49);
        layoutPath.Margin = new Padding(3, 6, 12, 6);
        layoutPath.Name = "layoutPath";
        layoutPath.ReadOnly = true;
        layoutPath.Size = new Size(1, 24);
        layoutPath.TabIndex = 5;
        layoutPath.Tag = "EPLAN 3D 宏|*.ema";
        //
        // layoutPathBrowseButton
        //
        layoutPathBrowseButton.BackColor = Color.White;
        layoutPathBrowseButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        layoutPathBrowseButton.FlatStyle = FlatStyle.Flat;
        layoutPathBrowseButton.Location = new Point(-43, 43);
        layoutPathBrowseButton.Margin = new Padding(0, 0, 8, 0);
        layoutPathBrowseButton.Name = "layoutPathBrowseButton";
        layoutPathBrowseButton.Size = new Size(77, 33);
        layoutPathBrowseButton.TabIndex = 6;
        layoutPathBrowseButton.Text = "选择…";
        layoutPathBrowseButton.UseVisualStyleBackColor = false;
        layoutPathBrowseButton.Click += BrowseLayout_Click;
        //
        // layoutPathClearButton
        //
        layoutPathClearButton.BackColor = Color.White;
        layoutPathClearButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        layoutPathClearButton.FlatStyle = FlatStyle.Flat;
        layoutPathClearButton.Location = new Point(42, 43);
        layoutPathClearButton.Margin = new Padding(0, 0, 8, 0);
        layoutPathClearButton.Name = "layoutPathClearButton";
        layoutPathClearButton.Size = new Size(62, 33);
        layoutPathClearButton.TabIndex = 7;
        layoutPathClearButton.Text = "清除";
        layoutPathClearButton.UseVisualStyleBackColor = false;
        layoutPathClearButton.Click += ClearLayout_Click;
        //
        // stepPathLabel
        //
        stepPathLabel.Anchor = AnchorStyles.Left;
        stepPathLabel.AutoSize = true;
        stepPathLabel.Location = new Point(0, 98);
        stepPathLabel.Margin = new Padding(0, 8, 6, 8);
        stepPathLabel.Name = "stepPathLabel";
        stepPathLabel.Size = new Size(117, 19);
        stepPathLabel.TabIndex = 8;
        stepPathLabel.Text = "外部文档 2 / STEP";
        //
        // stepPath
        //
        stepPath.Dock = DockStyle.Fill;
        stepPath.Location = new Point(148, 92);
        stepPath.Margin = new Padding(3, 6, 12, 6);
        stepPath.Name = "stepPath";
        stepPath.ReadOnly = true;
        stepPath.Size = new Size(1, 24);
        stepPath.TabIndex = 9;
        stepPath.Tag = "模型或文档|*.step;*.stp;*.pdf";
        //
        // stepPathBrowseButton
        //
        stepPathBrowseButton.BackColor = Color.White;
        stepPathBrowseButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        stepPathBrowseButton.FlatStyle = FlatStyle.Flat;
        stepPathBrowseButton.Location = new Point(-43, 86);
        stepPathBrowseButton.Margin = new Padding(0, 0, 8, 0);
        stepPathBrowseButton.Name = "stepPathBrowseButton";
        stepPathBrowseButton.Size = new Size(77, 33);
        stepPathBrowseButton.TabIndex = 10;
        stepPathBrowseButton.Text = "选择…";
        stepPathBrowseButton.UseVisualStyleBackColor = false;
        stepPathBrowseButton.Click += BrowseStep_Click;
        //
        // stepPathClearButton
        //
        stepPathClearButton.BackColor = Color.White;
        stepPathClearButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        stepPathClearButton.FlatStyle = FlatStyle.Flat;
        stepPathClearButton.Location = new Point(42, 86);
        stepPathClearButton.Margin = new Padding(0, 0, 8, 0);
        stepPathClearButton.Name = "stepPathClearButton";
        stepPathClearButton.Size = new Size(62, 33);
        stepPathClearButton.TabIndex = 11;
        stepPathClearButton.Text = "清除";
        stepPathClearButton.UseVisualStyleBackColor = false;
        stepPathClearButton.Click += ClearStep_Click;
        //
        // picturePathLabel
        //
        picturePathLabel.Anchor = AnchorStyles.Left;
        picturePathLabel.AutoSize = true;
        picturePathLabel.Location = new Point(0, 141);
        picturePathLabel.Margin = new Padding(0, 8, 6, 8);
        picturePathLabel.Name = "picturePathLabel";
        picturePathLabel.Size = new Size(61, 19);
        picturePathLabel.TabIndex = 12;
        picturePathLabel.Text = "产品图片";
        //
        // picturePath
        //
        picturePath.Dock = DockStyle.Fill;
        picturePath.Location = new Point(148, 135);
        picturePath.Margin = new Padding(3, 6, 12, 6);
        picturePath.Name = "picturePath";
        picturePath.ReadOnly = true;
        picturePath.Size = new Size(1, 24);
        picturePath.TabIndex = 13;
        picturePath.Tag = "图片|*.png;*.jpg;*.jpeg;*.bmp";
        //
        // picturePathBrowseButton
        //
        picturePathBrowseButton.BackColor = Color.White;
        picturePathBrowseButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        picturePathBrowseButton.FlatStyle = FlatStyle.Flat;
        picturePathBrowseButton.Location = new Point(-43, 129);
        picturePathBrowseButton.Margin = new Padding(0, 0, 8, 0);
        picturePathBrowseButton.Name = "picturePathBrowseButton";
        picturePathBrowseButton.Size = new Size(77, 33);
        picturePathBrowseButton.TabIndex = 14;
        picturePathBrowseButton.Text = "选择…";
        picturePathBrowseButton.UseVisualStyleBackColor = false;
        picturePathBrowseButton.Click += BrowsePicture_Click;
        //
        // picturePathClearButton
        //
        picturePathClearButton.BackColor = Color.White;
        picturePathClearButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        picturePathClearButton.FlatStyle = FlatStyle.Flat;
        picturePathClearButton.Location = new Point(42, 129);
        picturePathClearButton.Margin = new Padding(0, 0, 8, 0);
        picturePathClearButton.Name = "picturePathClearButton";
        picturePathClearButton.Size = new Size(62, 33);
        picturePathClearButton.TabIndex = 15;
        picturePathClearButton.Text = "清除";
        picturePathClearButton.UseVisualStyleBackColor = false;
        picturePathClearButton.Click += ClearPicture_Click;
        //
        // documentPathLabel
        //
        documentPathLabel.Anchor = AnchorStyles.Left;
        documentPathLabel.AutoSize = true;
        documentPathLabel.Location = new Point(0, 184);
        documentPathLabel.Margin = new Padding(0, 8, 6, 8);
        documentPathLabel.Name = "documentPathLabel";
        documentPathLabel.Size = new Size(61, 19);
        documentPathLabel.TabIndex = 16;
        documentPathLabel.Text = "产品手册";
        //
        // documentPath
        //
        documentPath.Dock = DockStyle.Fill;
        documentPath.Location = new Point(148, 178);
        documentPath.Margin = new Padding(3, 6, 12, 6);
        documentPath.Name = "documentPath";
        documentPath.ReadOnly = true;
        documentPath.Size = new Size(1, 24);
        documentPath.TabIndex = 17;
        documentPath.Tag = "PDF 文档|*.pdf";
        //
        // documentPathBrowseButton
        //
        documentPathBrowseButton.BackColor = Color.White;
        documentPathBrowseButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        documentPathBrowseButton.FlatStyle = FlatStyle.Flat;
        documentPathBrowseButton.Location = new Point(-43, 172);
        documentPathBrowseButton.Margin = new Padding(0, 0, 8, 0);
        documentPathBrowseButton.Name = "documentPathBrowseButton";
        documentPathBrowseButton.Size = new Size(77, 33);
        documentPathBrowseButton.TabIndex = 18;
        documentPathBrowseButton.Text = "选择…";
        documentPathBrowseButton.UseVisualStyleBackColor = false;
        documentPathBrowseButton.Click += BrowseDocument_Click;
        //
        // documentPathClearButton
        //
        documentPathClearButton.BackColor = Color.White;
        documentPathClearButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        documentPathClearButton.FlatStyle = FlatStyle.Flat;
        documentPathClearButton.Location = new Point(42, 172);
        documentPathClearButton.Margin = new Padding(0, 0, 8, 0);
        documentPathClearButton.Name = "documentPathClearButton";
        documentPathClearButton.Size = new Size(62, 33);
        documentPathClearButton.TabIndex = 19;
        documentPathClearButton.Text = "清除";
        documentPathClearButton.UseVisualStyleBackColor = false;
        documentPathClearButton.Click += ClearDocument_Click;
        //
        // assetStatus
        //
        assetStatus.BackColor = Color.FromArgb(255, 247, 226);
        assetStatus.Dock = DockStyle.Top;
        assetStatus.ForeColor = Color.FromArgb(115, 78, 17);
        assetStatus.Location = new Point(3, 230);
        assetStatus.Name = "assetStatus";
        assetStatus.Padding = new Padding(10);
        assetStatus.Size = new Size(112, 120);
        assetStatus.TabIndex = 1;
        assetStatus.Text = "文件用途\r\n原理图 .ema/.ems 与 3D .ema 分别关联到部件的不同宏属性。\r\nSTEP/STP 作为源文件附件保存，不会自动变成 3D 宏。要用于三维布局，请先在 EPLAN 2.9 中转换为 .ema。\r\n仅有 3D 外形还不代表已经定义安装方向、连接点或布线数据。";
        //
        // functionsPage
        //
        functionsPage.BackColor = Color.White;
        functionsPage.Controls.Add(functionsLayout);
        functionsPage.Location = new Point(4, 28);
        functionsPage.Name = "functionsPage";
        functionsPage.Padding = new Padding(10);
        functionsPage.Size = new Size(154, 0);
        functionsPage.TabIndex = 2;
        functionsPage.Text = "3  功能与端子";
        //
        // functionsLayout
        //
        functionsLayout.ColumnCount = 1;
        functionsLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        functionsLayout.Controls.Add(functionsNote, 0, 0);
        functionsLayout.Controls.Add(functionTools, 0, 1);
        functionsLayout.Controls.Add(functionGrid, 0, 2);
        functionsLayout.Dock = DockStyle.Fill;
        functionsLayout.Location = new Point(10, 10);
        functionsLayout.Name = "functionsLayout";
        functionsLayout.RowCount = 3;
        functionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 42F));
        functionsLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 43F));
        functionsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        functionsLayout.Size = new Size(134, 0);
        functionsLayout.TabIndex = 0;
        //
        // functionsNote
        //
        functionsNote.Dock = DockStyle.Fill;
        functionsNote.Location = new Point(3, 0);
        functionsNote.Name = "functionsNote";
        functionsNote.Size = new Size(128, 42);
        functionsNote.TabIndex = 0;
        functionsNote.Text = "每行表示一个电气功能。多个连接点用 | 分隔，例如 A1|A2。PLC 的插头和通道分别填写。\r\n宏不会在本软件中解析；功能模板请按实际产品填写，也可留空后在 EPLAN 中分配。";
        //
        // functionTools
        //
        functionTools.Controls.Add(addFunctionButton);
        functionTools.Controls.Add(changeFunctionButton);
        functionTools.Controls.Add(deleteFunctionButton);
        functionTools.Controls.Add(moveUpButton);
        functionTools.Controls.Add(moveDownButton);
        functionTools.Dock = DockStyle.Fill;
        functionTools.Location = new Point(3, 45);
        functionTools.Name = "functionTools";
        functionTools.Size = new Size(128, 37);
        functionTools.TabIndex = 1;
        functionTools.WrapContents = false;
        //
        // addFunctionButton
        //
        addFunctionButton.BackColor = Color.White;
        addFunctionButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        addFunctionButton.FlatStyle = FlatStyle.Flat;
        addFunctionButton.Location = new Point(0, 0);
        addFunctionButton.Margin = new Padding(0, 0, 8, 0);
        addFunctionButton.Name = "addFunctionButton";
        addFunctionButton.Size = new Size(112, 33);
        addFunctionButton.TabIndex = 0;
        addFunctionButton.Text = "添加功能";
        addFunctionButton.UseVisualStyleBackColor = false;
        addFunctionButton.Click += AddFunction_Click;
        //
        // changeFunctionButton
        //
        changeFunctionButton.BackColor = Color.White;
        changeFunctionButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        changeFunctionButton.FlatStyle = FlatStyle.Flat;
        changeFunctionButton.Location = new Point(120, 0);
        changeFunctionButton.Margin = new Padding(0, 0, 8, 0);
        changeFunctionButton.Name = "changeFunctionButton";
        changeFunctionButton.Size = new Size(112, 33);
        changeFunctionButton.TabIndex = 1;
        changeFunctionButton.Text = "更换功能";
        changeFunctionButton.UseVisualStyleBackColor = false;
        changeFunctionButton.Click += ChangeFunction_Click;
        //
        // deleteFunctionButton
        //
        deleteFunctionButton.BackColor = Color.White;
        deleteFunctionButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        deleteFunctionButton.FlatStyle = FlatStyle.Flat;
        deleteFunctionButton.Location = new Point(240, 0);
        deleteFunctionButton.Margin = new Padding(0, 0, 8, 0);
        deleteFunctionButton.Name = "deleteFunctionButton";
        deleteFunctionButton.Size = new Size(112, 33);
        deleteFunctionButton.TabIndex = 2;
        deleteFunctionButton.Text = "删除行";
        deleteFunctionButton.UseVisualStyleBackColor = false;
        deleteFunctionButton.Click += DeleteFunction_Click;
        //
        // moveUpButton
        //
        moveUpButton.BackColor = Color.White;
        moveUpButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        moveUpButton.FlatStyle = FlatStyle.Flat;
        moveUpButton.Location = new Point(360, 0);
        moveUpButton.Margin = new Padding(0, 0, 8, 0);
        moveUpButton.Name = "moveUpButton";
        moveUpButton.Size = new Size(70, 33);
        moveUpButton.TabIndex = 3;
        moveUpButton.Text = "上移";
        moveUpButton.UseVisualStyleBackColor = false;
        moveUpButton.Click += MoveFunctionUp_Click;
        //
        // moveDownButton
        //
        moveDownButton.BackColor = Color.White;
        moveDownButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        moveDownButton.FlatStyle = FlatStyle.Flat;
        moveDownButton.Location = new Point(438, 0);
        moveDownButton.Margin = new Padding(0, 0, 8, 0);
        moveDownButton.Name = "moveDownButton";
        moveDownButton.Size = new Size(70, 33);
        moveDownButton.TabIndex = 4;
        moveDownButton.Text = "下移";
        moveDownButton.UseVisualStyleBackColor = false;
        moveDownButton.Click += MoveFunctionDown_Click;
        //
        // functionGrid
        //
        functionGrid.AutoGenerateColumns = false;
        functionGrid.Columns.AddRange(new DataGridViewColumn[] { functionCodeColumn, functionConnectionsColumn, functionPlugColumn, functionChannelColumn, functionDescriptionColumn, functionCharacteristicsColumn, functionSymbolColumn });
        functionGrid.ScrollBars = ScrollBars.Both;
        functionGrid.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        functionGrid.AllowUserToAddRows = false;
        functionGrid.AllowUserToDeleteRows = false;
        functionGrid.AllowUserToResizeRows = false;
        functionGrid.BackgroundColor = Color.White;
        functionGrid.Dock = DockStyle.Fill;
        functionGrid.Location = new Point(3, 88);
        functionGrid.MultiSelect = false;
        functionGrid.Name = "functionGrid";
        functionGrid.RowHeadersWidth = 42;
        functionGrid.RowTemplate.Height = 32;
        functionGrid.SelectionMode = DataGridViewSelectionMode.CellSelect;
        functionGrid.Size = new Size(128, 1);
        functionGrid.TabIndex = 2;
        functionGrid.CellFormatting += FunctionGrid_CellFormatting;
        functionGrid.DataError += FunctionGrid_DataError;
        functionGrid.RowPostPaint += FunctionGrid_RowPostPaint;
        //
        // technicalPage
        //
        technicalPage.BackColor = Color.White;
        technicalPage.Controls.Add(technicalTable);
        technicalPage.Controls.Add(technicalNote);
        technicalPage.Location = new Point(4, 28);
        technicalPage.Name = "technicalPage";
        technicalPage.Padding = new Padding(18);
        technicalPage.Size = new Size(154, 0);
        technicalPage.TabIndex = 3;
        technicalPage.Text = "4  技术与厂家";
        //
        // technicalTable
        //
        technicalTable.AutoSize = true;
        technicalTable.ColumnCount = 4;
        technicalTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110F));
        technicalTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        technicalTable.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 115F));
        technicalTable.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
        technicalTable.Controls.Add(ratedCurrentLabel, 0, 0);
        technicalTable.Controls.Add(ratedCurrent, 1, 0);
        technicalTable.Controls.Add(voltageLabel, 2, 0);
        technicalTable.Controls.Add(voltage, 3, 0);
        technicalTable.Controls.Add(voltageTypeLabel, 0, 1);
        technicalTable.Controls.Add(voltageType, 1, 1);
        technicalTable.Controls.Add(powerDissipationLabel, 2, 1);
        technicalTable.Controls.Add(powerDissipation, 3, 1);
        technicalTable.Controls.Add(crossSectionLabel, 0, 2);
        technicalTable.Controls.Add(crossSection, 1, 2);
        technicalTable.Controls.Add(manufacturerNameLabel, 0, 3);
        technicalTable.Controls.Add(manufacturerName, 1, 3);
        technicalTable.Controls.Add(manufacturerCompanyLabel, 2, 3);
        technicalTable.Controls.Add(manufacturerCompany, 3, 3);
        technicalTable.Controls.Add(manufacturerEmailLabel, 0, 4);
        technicalTable.Controls.Add(manufacturerEmail, 1, 4);
        technicalTable.Controls.Add(manufacturerPhoneLabel, 2, 4);
        technicalTable.Controls.Add(manufacturerPhone, 3, 4);
        technicalTable.Dock = DockStyle.Top;
        technicalTable.Location = new Point(18, 18);
        technicalTable.Name = "technicalTable";
        technicalTable.RowCount = 5;
        technicalTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        technicalTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        technicalTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        technicalTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        technicalTable.RowStyles.Add(new RowStyle(SizeType.Absolute, 36F));
        technicalTable.Size = new Size(118, 180);
        technicalTable.TabIndex = 0;
        //
        // ratedCurrentLabel
        //
        ratedCurrentLabel.Anchor = AnchorStyles.Left;
        ratedCurrentLabel.AutoSize = true;
        ratedCurrentLabel.Location = new Point(0, 8);
        ratedCurrentLabel.Margin = new Padding(0, 8, 6, 8);
        ratedCurrentLabel.Name = "ratedCurrentLabel";
        ratedCurrentLabel.Size = new Size(61, 19);
        ratedCurrentLabel.TabIndex = 0;
        ratedCurrentLabel.Text = "额定电流";
        //
        // ratedCurrent
        //
        ratedCurrent.Dock = DockStyle.Fill;
        ratedCurrent.Location = new Point(113, 6);
        ratedCurrent.Margin = new Padding(3, 6, 12, 6);
        ratedCurrent.Name = "ratedCurrent";
        ratedCurrent.Size = new Size(1, 24);
        ratedCurrent.TabIndex = 1;
        //
        // voltageLabel
        //
        voltageLabel.Anchor = AnchorStyles.Left;
        voltageLabel.AutoSize = true;
        voltageLabel.Location = new Point(57, 8);
        voltageLabel.Margin = new Padding(0, 8, 6, 8);
        voltageLabel.Name = "voltageLabel";
        voltageLabel.Size = new Size(35, 19);
        voltageLabel.TabIndex = 2;
        voltageLabel.Text = "电压";
        //
        // voltage
        //
        voltage.Dock = DockStyle.Fill;
        voltage.Location = new Point(175, 6);
        voltage.Margin = new Padding(3, 6, 12, 6);
        voltage.Name = "voltage";
        voltage.Size = new Size(1, 24);
        voltage.TabIndex = 3;
        //
        // voltageTypeLabel
        //
        voltageTypeLabel.Anchor = AnchorStyles.Left;
        voltageTypeLabel.AutoSize = true;
        voltageTypeLabel.Location = new Point(0, 44);
        voltageTypeLabel.Margin = new Padding(0, 8, 6, 8);
        voltageTypeLabel.Name = "voltageTypeLabel";
        voltageTypeLabel.Size = new Size(61, 19);
        voltageTypeLabel.TabIndex = 4;
        voltageTypeLabel.Text = "电压类型";
        //
        // voltageType
        //
        voltageType.Dock = DockStyle.Fill;
        voltageType.Location = new Point(113, 42);
        voltageType.Margin = new Padding(3, 6, 12, 6);
        voltageType.Name = "voltageType";
        voltageType.Size = new Size(1, 24);
        voltageType.TabIndex = 5;
        //
        // powerDissipationLabel
        //
        powerDissipationLabel.Anchor = AnchorStyles.Left;
        powerDissipationLabel.AutoSize = true;
        powerDissipationLabel.Location = new Point(57, 44);
        powerDissipationLabel.Margin = new Padding(0, 8, 6, 8);
        powerDissipationLabel.Name = "powerDissipationLabel";
        powerDissipationLabel.Size = new Size(35, 19);
        powerDissipationLabel.TabIndex = 6;
        powerDissipationLabel.Text = "功耗";
        //
        // powerDissipation
        //
        powerDissipation.Dock = DockStyle.Fill;
        powerDissipation.Location = new Point(175, 42);
        powerDissipation.Margin = new Padding(3, 6, 12, 6);
        powerDissipation.Name = "powerDissipation";
        powerDissipation.Size = new Size(1, 24);
        powerDissipation.TabIndex = 7;
        //
        // crossSectionLabel
        //
        crossSectionLabel.Anchor = AnchorStyles.Left;
        crossSectionLabel.AutoSize = true;
        crossSectionLabel.Location = new Point(0, 80);
        crossSectionLabel.Margin = new Padding(0, 8, 6, 8);
        crossSectionLabel.Name = "crossSectionLabel";
        crossSectionLabel.Size = new Size(74, 19);
        crossSectionLabel.TabIndex = 8;
        crossSectionLabel.Text = "连接横截面";
        //
        // crossSection
        //
        crossSection.Dock = DockStyle.Fill;
        crossSection.Location = new Point(113, 78);
        crossSection.Margin = new Padding(3, 6, 12, 6);
        crossSection.Name = "crossSection";
        crossSection.Size = new Size(1, 24);
        crossSection.TabIndex = 9;
        //
        // manufacturerNameLabel
        //
        manufacturerNameLabel.Anchor = AnchorStyles.Left;
        manufacturerNameLabel.AutoSize = true;
        manufacturerNameLabel.Location = new Point(0, 116);
        manufacturerNameLabel.Margin = new Padding(0, 8, 6, 8);
        manufacturerNameLabel.Name = "manufacturerNameLabel";
        manufacturerNameLabel.Size = new Size(61, 19);
        manufacturerNameLabel.TabIndex = 10;
        manufacturerNameLabel.Text = "厂家全称";
        //
        // manufacturerName
        //
        manufacturerName.Dock = DockStyle.Fill;
        manufacturerName.Location = new Point(113, 114);
        manufacturerName.Margin = new Padding(3, 6, 12, 6);
        manufacturerName.Name = "manufacturerName";
        manufacturerName.Size = new Size(1, 24);
        manufacturerName.TabIndex = 11;
        //
        // manufacturerCompanyLabel
        //
        manufacturerCompanyLabel.Anchor = AnchorStyles.Left;
        manufacturerCompanyLabel.AutoSize = true;
        manufacturerCompanyLabel.Location = new Point(57, 116);
        manufacturerCompanyLabel.Margin = new Padding(0, 8, 6, 8);
        manufacturerCompanyLabel.Name = "manufacturerCompanyLabel";
        manufacturerCompanyLabel.Size = new Size(61, 19);
        manufacturerCompanyLabel.TabIndex = 12;
        manufacturerCompanyLabel.Text = "公司名称";
        //
        // manufacturerCompany
        //
        manufacturerCompany.Dock = DockStyle.Fill;
        manufacturerCompany.Location = new Point(175, 114);
        manufacturerCompany.Margin = new Padding(3, 6, 12, 6);
        manufacturerCompany.Name = "manufacturerCompany";
        manufacturerCompany.Size = new Size(1, 24);
        manufacturerCompany.TabIndex = 13;
        //
        // manufacturerEmailLabel
        //
        manufacturerEmailLabel.Anchor = AnchorStyles.Left;
        manufacturerEmailLabel.AutoSize = true;
        manufacturerEmailLabel.Location = new Point(0, 152);
        manufacturerEmailLabel.Margin = new Padding(0, 8, 6, 8);
        manufacturerEmailLabel.Name = "manufacturerEmailLabel";
        manufacturerEmailLabel.Size = new Size(61, 19);
        manufacturerEmailLabel.TabIndex = 14;
        manufacturerEmailLabel.Text = "厂家邮箱";
        //
        // manufacturerEmail
        //
        manufacturerEmail.Dock = DockStyle.Fill;
        manufacturerEmail.Location = new Point(113, 150);
        manufacturerEmail.Margin = new Padding(3, 6, 12, 6);
        manufacturerEmail.Name = "manufacturerEmail";
        manufacturerEmail.Size = new Size(1, 24);
        manufacturerEmail.TabIndex = 15;
        //
        // manufacturerPhoneLabel
        //
        manufacturerPhoneLabel.Anchor = AnchorStyles.Left;
        manufacturerPhoneLabel.AutoSize = true;
        manufacturerPhoneLabel.Location = new Point(57, 152);
        manufacturerPhoneLabel.Margin = new Padding(0, 8, 6, 8);
        manufacturerPhoneLabel.Name = "manufacturerPhoneLabel";
        manufacturerPhoneLabel.Size = new Size(61, 19);
        manufacturerPhoneLabel.TabIndex = 16;
        manufacturerPhoneLabel.Text = "厂家电话";
        //
        // manufacturerPhone
        //
        manufacturerPhone.Dock = DockStyle.Fill;
        manufacturerPhone.Location = new Point(175, 150);
        manufacturerPhone.Margin = new Padding(3, 6, 12, 6);
        manufacturerPhone.Name = "manufacturerPhone";
        manufacturerPhone.Size = new Size(1, 24);
        manufacturerPhone.TabIndex = 17;
        //
        // technicalNote
        //
        technicalNote.Dock = DockStyle.Bottom;
        technicalNote.ForeColor = Color.DimGray;
        technicalNote.Location = new Point(18, -83);
        technicalNote.Name = "technicalNote";
        technicalNote.Size = new Size(118, 65);
        technicalNote.TabIndex = 1;
        technicalNote.Text = "参数按原包显示，可填写 8 A、400/415 V 等带单位的值。\r\n修改厂家代号不会自动清空原来的全称、邮箱等资料，请按实际厂家逐项检查。\r\n其他厂家字段、技术参数和自定义属性在原包中保留，可在“原包数据”页查看。";
        //
        // originalPage
        //
        originalPage.BackColor = Color.White;
        originalPage.Controls.Add(originalLayout);
        originalPage.Location = new Point(4, 28);
        originalPage.Name = "originalPage";
        originalPage.Padding = new Padding(10);
        originalPage.Size = new Size(154, 0);
        originalPage.TabIndex = 4;
        originalPage.Text = "5  原包数据";
        //
        // originalLayout
        //
        originalLayout.ColumnCount = 1;
        originalLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        originalLayout.Controls.Add(packageSummary, 0, 0);
        originalLayout.Controls.Add(originalTabs, 0, 1);
        originalLayout.Dock = DockStyle.Fill;
        originalLayout.Location = new Point(10, 10);
        originalLayout.Name = "originalLayout";
        originalLayout.RowCount = 2;
        originalLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85F));
        originalLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        originalLayout.Size = new Size(134, 0);
        originalLayout.TabIndex = 0;
        //
        // packageSummary
        //
        packageSummary.Dock = DockStyle.Fill;
        packageSummary.ForeColor = Color.FromArgb(87, 70, 35);
        packageSummary.Location = new Point(3, 0);
        packageSummary.Name = "packageSummary";
        packageSummary.Size = new Size(128, 85);
        packageSummary.TabIndex = 0;
        packageSummary.Text = "打开已有部件包后，这里显示来源、部件编号和版本信息。";
        //
        // originalTabs
        //
        originalTabs.Controls.Add(originalFilesPage);
        originalTabs.Controls.Add(originalAttributesPage);
        originalTabs.Controls.Add(connectionPointsPage);
        originalTabs.Dock = DockStyle.Fill;
        originalTabs.Location = new Point(3, 88);
        originalTabs.Name = "originalTabs";
        originalTabs.SelectedIndex = 0;
        originalTabs.Size = new Size(128, 1);
        originalTabs.TabIndex = 1;
        //
        // originalFilesPage
        //
        originalFilesPage.Controls.Add(originalFiles);
        originalFilesPage.Location = new Point(4, 28);
        originalFilesPage.Name = "originalFilesPage";
        originalFilesPage.Padding = new Padding(3);
        originalFilesPage.Size = new Size(120, 0);
        originalFilesPage.TabIndex = 0;
        originalFilesPage.Text = "包内文件";
        //
        // originalFiles
        //
        originalFiles.AutoGenerateColumns = false;
        originalFiles.Columns.AddRange(new DataGridViewColumn[] { fileTypeColumn, fileNameColumn, filePathColumn, fileBytesColumn });
        originalFiles.ScrollBars = ScrollBars.Both;
        originalFiles.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        originalFiles.AllowUserToAddRows = false;
        originalFiles.AllowUserToDeleteRows = false;
        originalFiles.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        originalFiles.BackgroundColor = Color.White;
        originalFiles.Dock = DockStyle.Fill;
        originalFiles.Location = new Point(3, 3);
        originalFiles.Name = "originalFiles";
        originalFiles.ReadOnly = true;
        originalFiles.AllowUserToResizeRows = false;
        originalFiles.RowTemplate.Height = 26;
        originalFiles.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        originalFiles.MouseEnter += ReadOnlyGrid_MouseEnter;
        originalFiles.RowHeadersVisible = false;
        originalFiles.Size = new Size(114, 0);
        originalFiles.TabIndex = 0;
        //
        // originalAttributesPage
        //
        originalAttributesPage.Controls.Add(originalAttributes);
        originalAttributesPage.Location = new Point(4, 26);
        originalAttributesPage.Name = "originalAttributesPage";
        originalAttributesPage.Padding = new Padding(3);
        originalAttributesPage.Size = new Size(166, 0);
        originalAttributesPage.TabIndex = 1;
        originalAttributesPage.Text = "原始属性";
        //
        // originalAttributes
        //
        originalAttributes.AutoGenerateColumns = false;
        originalAttributes.Columns.AddRange(new DataGridViewColumn[] { attributeLocationColumn, attributeNameColumn, attributeValueColumn });
        originalAttributes.ScrollBars = ScrollBars.Both;
        originalAttributes.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        originalAttributes.AllowUserToAddRows = false;
        originalAttributes.AllowUserToDeleteRows = false;
        originalAttributes.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        originalAttributes.BackgroundColor = Color.White;
        originalAttributes.Dock = DockStyle.Fill;
        originalAttributes.Location = new Point(3, 3);
        originalAttributes.Name = "originalAttributes";
        originalAttributes.ReadOnly = true;
        originalAttributes.AllowUserToResizeRows = false;
        originalAttributes.RowTemplate.Height = 26;
        originalAttributes.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        originalAttributes.MouseEnter += ReadOnlyGrid_MouseEnter;
        originalAttributes.RowHeadersVisible = false;
        originalAttributes.Size = new Size(160, 0);
        originalAttributes.TabIndex = 0;
        //
        // connectionPointsPage
        //
        connectionPointsPage.Controls.Add(connectionPoints);
        connectionPointsPage.Location = new Point(4, 26);
        connectionPointsPage.Name = "connectionPointsPage";
        connectionPointsPage.Padding = new Padding(3);
        connectionPointsPage.Size = new Size(166, 0);
        connectionPointsPage.TabIndex = 2;
        connectionPointsPage.Text = "连接点排列样式";
        //
        // connectionPoints
        //
        connectionPoints.AutoGenerateColumns = false;
        connectionPoints.Columns.AddRange(new DataGridViewColumn[] { pointPatternColumn, pointNameColumn, pointXColumn, pointYColumn, pointZColumn, pointXDirectionColumn, pointYDirectionColumn, pointZDirectionColumn, pointMinSectionColumn, pointMaxSectionColumn });
        connectionPoints.ScrollBars = ScrollBars.Both;
        connectionPoints.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.None;
        connectionPoints.AllowUserToAddRows = false;
        connectionPoints.AllowUserToDeleteRows = false;
        connectionPoints.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None;
        connectionPoints.BackgroundColor = Color.White;
        connectionPoints.Dock = DockStyle.Fill;
        connectionPoints.Location = new Point(3, 3);
        connectionPoints.Name = "connectionPoints";
        connectionPoints.ReadOnly = true;
        connectionPoints.AllowUserToResizeRows = false;
        connectionPoints.RowTemplate.Height = 26;
        connectionPoints.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        connectionPoints.MouseEnter += ReadOnlyGrid_MouseEnter;
        connectionPoints.RowHeadersVisible = false;
        connectionPoints.Size = new Size(160, 0);
        connectionPoints.TabIndex = 0;
        //
        // helpPage
        //
        helpPage.BackColor = Color.White;
        helpPage.Controls.Add(helpTextBox);
        helpPage.Location = new Point(4, 28);
        helpPage.Name = "helpPage";
        helpPage.Padding = new Padding(18);
        helpPage.Size = new Size(154, 0);
        helpPage.TabIndex = 5;
        helpPage.Text = "使用说明";
        //
        // helpTextBox
        //
        helpTextBox.BackColor = Color.White;
        helpTextBox.BorderStyle = BorderStyle.None;
        helpTextBox.Dock = DockStyle.Fill;
        helpTextBox.Location = new Point(18, 18);
        helpTextBox.Multiline = true;
        helpTextBox.Name = "helpTextBox";
        helpTextBox.ReadOnly = true;
        helpTextBox.ScrollBars = ScrollBars.Vertical;
        helpTextBox.Size = new Size(118, 0);
        helpTextBox.TabIndex = 0;
        helpTextBox.Text = resources.GetString("helpTextBox.Text");
        //
        // resultBox
        //
        resultBox.BackColor = Color.White;
        resultBox.Dock = DockStyle.Fill;
        resultBox.Location = new Point(20, 654);
        resultBox.Margin = new Padding(20, 4, 20, 4);
        resultBox.Multiline = true;
        resultBox.Name = "resultBox";
        resultBox.ReadOnly = true;
        resultBox.ScrollBars = ScrollBars.Vertical;
        resultBox.Size = new Size(1140, 102);
        resultBox.TabIndex = 3;
        resultBox.Text = "就绪。先填写部件资料，再选择宏与文件。";
        //
        // status
        //
        status.Dock = DockStyle.Fill;
        status.ForeColor = Color.FromArgb(85, 100, 120);
        status.Location = new Point(3, 760);
        status.Name = "status";
        status.Padding = new Padding(20, 3, 0, 0);
        status.Size = new Size(1174, 30);
        status.TabIndex = 4;
        status.Text = "新建资料 · 尚未保存";
        //
        // MainForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        BackColor = Color.FromArgb(244, 246, 249);
        ClientSize = new Size(1180, 790);
        Controls.Add(shell);
        Font = new Font("Microsoft YaHei UI", 9.5F);
        MinimumSize = new Size(1060, 720);
        Name = "MainForm";
        StartPosition = FormStartPosition.CenterScreen;
        Text = "EPLAN 部件包编辑助手 · 1.1";
        FormClosing += MainForm_FormClosing;
        shell.ResumeLayout(false);
        shell.PerformLayout();
        headerPanel.ResumeLayout(false);
        headerPanel.PerformLayout();
        toolbar.ResumeLayout(false);
        editorPanel.ResumeLayout(false);
        editorTabs.ResumeLayout(false);
        detailsPage.ResumeLayout(false);
        detailsPage.PerformLayout();
        detailsTable.ResumeLayout(false);
        detailsTable.PerformLayout();
        ((System.ComponentModel.ISupportInitialize)partWidth).EndInit();
        ((System.ComponentModel.ISupportInitialize)partHeight).EndInit();
        ((System.ComponentModel.ISupportInitialize)partDepth).EndInit();
        filesPage.ResumeLayout(false);
        filesLayout.ResumeLayout(false);
        filesLayout.PerformLayout();
        filesTable.ResumeLayout(false);
        filesTable.PerformLayout();
        functionsPage.ResumeLayout(false);
        functionsLayout.ResumeLayout(false);
        functionTools.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)functionGrid).EndInit();
        technicalPage.ResumeLayout(false);
        technicalPage.PerformLayout();
        technicalTable.ResumeLayout(false);
        technicalTable.PerformLayout();
        originalPage.ResumeLayout(false);
        originalLayout.ResumeLayout(false);
        originalTabs.ResumeLayout(false);
        originalFilesPage.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)originalFiles).EndInit();
        originalAttributesPage.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)originalAttributes).EndInit();
        connectionPointsPage.ResumeLayout(false);
        ((System.ComponentModel.ISupportInitialize)connectionPoints).EndInit();
        helpPage.ResumeLayout(false);
        helpPage.PerformLayout();
        ResumeLayout(false);
    }

    private TableLayoutPanel shell;
    private Panel headerPanel;
    private Label titleLabel;
    private Label subtitleLabel;
    private FlowLayoutPanel toolbar;
    private Button newButton;
    private Button openEdzButton;
    private Button openFolderButton;
    private Button openButton;
    private Button saveButton;
    private Button exampleButton;
    private Button validateButton;
    private Button exportButton;
    private Panel editorPanel;
    private TabControl editorTabs;
    private TabPage detailsPage;
    private TabPage filesPage;
    private TabPage functionsPage;
    private TabPage technicalPage;
    private TabPage originalPage;
    private TabPage helpPage;
    private TableLayoutPanel detailsTable;
    private Label partNumberLabel;
    private TextBox partNumber;
    private Label manufacturerLabel;
    private TextBox manufacturer;
    private Label typeNumberLabel;
    private TextBox typeNumber;
    private Label orderNumberLabel;
    private TextBox orderNumber;
    private Label partNameLabel;
    private TextBox partName;
    private Label productGroupLabel;
    private ComboBox productGroup;
    private Label characteristicsLabel;
    private TextBox characteristics;
    private Label variantLabel;
    private TextBox variant;
    private Label partWidthLabel;
    private NumericUpDown partWidth;
    private Label partHeightLabel;
    private NumericUpDown partHeight;
    private Label partDepthLabel;
    private NumericUpDown partDepth;
    private Label descriptionLabel;
    private TextBox description;
    private Label detailsNote;
    private TableLayoutPanel filesLayout;
    private TableLayoutPanel filesTable;
    private Label schematicPathLabel;
    private TextBox schematicPath;
    private Button schematicPathBrowseButton;
    private Button schematicPathClearButton;
    private Label layoutPathLabel;
    private TextBox layoutPath;
    private Button layoutPathBrowseButton;
    private Button layoutPathClearButton;
    private Label stepPathLabel;
    private TextBox stepPath;
    private Button stepPathBrowseButton;
    private Button stepPathClearButton;
    private Label picturePathLabel;
    private TextBox picturePath;
    private Button picturePathBrowseButton;
    private Button picturePathClearButton;
    private Label documentPathLabel;
    private TextBox documentPath;
    private Button documentPathBrowseButton;
    private Button documentPathClearButton;
    private Label assetStatus;
    private TableLayoutPanel functionsLayout;
    private Label functionsNote;
    private FlowLayoutPanel functionTools;
    private Button addFunctionButton;
    private Button changeFunctionButton;
    private Button deleteFunctionButton;
    private Button moveUpButton;
    private Button moveDownButton;
    private DataGridView functionGrid;
    private DataGridViewTextBoxColumn functionCodeColumn;
    private DataGridViewTextBoxColumn functionConnectionsColumn;
    private DataGridViewTextBoxColumn functionPlugColumn;
    private DataGridViewTextBoxColumn functionChannelColumn;
    private DataGridViewTextBoxColumn functionDescriptionColumn;
    private DataGridViewTextBoxColumn functionCharacteristicsColumn;
    private DataGridViewTextBoxColumn functionSymbolColumn;
    private TableLayoutPanel technicalTable;
    private Label ratedCurrentLabel;
    private TextBox ratedCurrent;
    private Label voltageLabel;
    private TextBox voltage;
    private Label voltageTypeLabel;
    private TextBox voltageType;
    private Label powerDissipationLabel;
    private TextBox powerDissipation;
    private Label crossSectionLabel;
    private TextBox crossSection;
    private Label manufacturerNameLabel;
    private TextBox manufacturerName;
    private Label manufacturerCompanyLabel;
    private TextBox manufacturerCompany;
    private Label manufacturerEmailLabel;
    private TextBox manufacturerEmail;
    private Label manufacturerPhoneLabel;
    private TextBox manufacturerPhone;
    private Label technicalNote;
    private TableLayoutPanel originalLayout;
    private Label packageSummary;
    private TabControl originalTabs;
    private TabPage originalFilesPage;
    private TabPage originalAttributesPage;
    private TabPage connectionPointsPage;
    private DataGridView originalFiles;
    private DataGridViewTextBoxColumn fileTypeColumn;
    private DataGridViewTextBoxColumn fileNameColumn;
    private DataGridViewTextBoxColumn filePathColumn;
    private DataGridViewTextBoxColumn fileBytesColumn;
    private DataGridView originalAttributes;
    private DataGridViewTextBoxColumn attributeLocationColumn;
    private DataGridViewTextBoxColumn attributeNameColumn;
    private DataGridViewTextBoxColumn attributeValueColumn;
    private DataGridView connectionPoints;
    private DataGridViewTextBoxColumn pointPatternColumn;
    private DataGridViewTextBoxColumn pointNameColumn;
    private DataGridViewTextBoxColumn pointXColumn;
    private DataGridViewTextBoxColumn pointYColumn;
    private DataGridViewTextBoxColumn pointZColumn;
    private DataGridViewTextBoxColumn pointXDirectionColumn;
    private DataGridViewTextBoxColumn pointYDirectionColumn;
    private DataGridViewTextBoxColumn pointZDirectionColumn;
    private DataGridViewTextBoxColumn pointMinSectionColumn;
    private DataGridViewTextBoxColumn pointMaxSectionColumn;
    private TextBox helpTextBox;
    private TextBox resultBox;
    private Label status;
}
