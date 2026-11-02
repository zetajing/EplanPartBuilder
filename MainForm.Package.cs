using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace EplanPartBuilder;

public partial class MainForm
{
    private static string PackageHelpText => """
        编辑已有部件包

        1. 点击“打开 EDZ”，或“打开目录”选择包含 manifest.xml 的已解开部件目录。
        2. 包中有多个零部件 / 变量时，选择要编辑的一项，其余记录一起保留。
        3. 查看部件资料、技术与厂家、原包数据；原包数据页显示文件、原始属性和连接点坐标。
        4. 修改指定字段，或在“宏与文件”中选择替换文件。清除表示移除该项关联。
        5. 检查资料后生成新的 EDZ。原包目录和输入 EDZ 保持原样。

        数据保留

        未编辑的 XML、宏和附件按原文件保留；修改中文名称或说明时，保留原有其他语言。
        未编辑的功能属性、连接点排列样式、自定义属性定义、其他部件及变量会保留。
        部件资料属于整个部件；技术参数和功能模板属于所选择的变量。
        修改连接点编号后，原连接点坐标仍保留，需要在 EPLAN 中核对对应关系。
        改厂家代号不会自动清除旧厂家的其他资料；请逐项检查“技术与厂家”和“原始属性”。

        保存与移动

        保存为 .epart.json 时，原包快照与替换文件保存在同名的 .assets 文件夹中。
        移动或备份资料时，JSON 和 .assets 文件夹必须一起保留。网址按原引用保留，不会自动下载。

        宏和版本

        保留输入文件的原始版本；2024 宏重新打包后仍是 2024 宏，不自动转换为 2.9。
        STEP/STP 作为外部文档附件，不自动转换为 3D 宏。可在 EPLAN 中转换为 .ema 后替换。
        生成的包需要在与原宏匹配的 EPLAN 测试数据库中导入，核对原理图、安装和布线。

        新建部件

        未打开原包时，可继续填写资料并选择用户提供的宏，生成一个零部件、一个变量的基础包。
        新增功能可搜索 EPLAN 2.9 功能定义；较新包中已有的其他功能编号按原值保留。

        本版边界

        可选中编辑零部件变量，部件组和模块只保留，不直接编辑。
        原包数据页是只读核对页面，连接点几何和未列入表单的扩展字段不直接修改。
        不解析或重绘宏、不自动提取功能模板、不做宏版本转换。
        """.ReplaceLineEndings();

    private readonly Dictionary<string, TextBox> additionalFields = new();
    private Label packageSummary = null!;
    private DataGridView originalFiles = null!;
    private DataGridView originalAttributes = null!;
    private DataGridView connectionPoints = null!;

    private void BuildPackagePages(TabControl tabs)
    {
        var technical = new TabPage("4  技术与厂家") { BackColor = Color.White, Padding = new Padding(18) };
        var table = new TableLayoutPanel { Dock = DockStyle.Top, AutoSize = true, ColumnCount = 4 };
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 110)); table.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        Add("RatedCurrent", "额定电流", 0, 0); Add("Voltage", "电压", 2, 0);
        Add("VoltageType", "电压类型", 0, 1); Add("PowerDissipation", "功耗", 2, 1);
        Add("CrossSection", "连接横截面", 0, 2);
        Add("ManufacturerName", "厂家全称", 0, 3); Add("ManufacturerCompany", "公司名称", 2, 3);
        Add("ManufacturerEmail", "厂家邮箱", 0, 4); Add("ManufacturerPhone", "厂家电话", 2, 4);
        technical.Controls.Add(table);
        technical.Controls.Add(new Label { Dock = DockStyle.Bottom, Height = 65, ForeColor = Color.DimGray,
            Text = "参数按原包显示，可填写 8 A、400/415 V 等带单位的值。\r\n修改厂家代号不会自动清空原来的全称、邮箱等资料，请按实际厂家逐项检查。\r\n其他厂家字段、技术参数和自定义属性在原包中保留，可在“原包数据”页查看。" });
        tabs.TabPages.Insert(3, technical);

        var original = new TabPage("5  原包数据") { BackColor = Color.White, Padding = new Padding(10) };
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, RowCount = 2, ColumnCount = 1 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 85)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        packageSummary = new Label { Dock = DockStyle.Fill, ForeColor = Color.FromArgb(87, 70, 35) };
        layout.Controls.Add(packageSummary, 0, 0);
        var views = new TabControl { Dock = DockStyle.Fill };
        originalFiles = View("包内文件"); originalAttributes = View("原始属性"); connectionPoints = View("连接点排列样式");
        layout.Controls.Add(views, 0, 1); original.Controls.Add(layout); tabs.TabPages.Insert(4, original);

        void Add(string property, string label, int column, int row)
        {
            var box = MakeTextBox(); additionalFields.Add(property, box); AddField(table, label, box, column, row);
        }
        DataGridView View(string name)
        {
            var page = new TabPage(name);
            var grid = new DataGridView { Dock = DockStyle.Fill, ReadOnly = true, AllowUserToAddRows = false, AllowUserToDeleteRows = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.DisplayedCells, BackgroundColor = Color.White, RowHeadersVisible = false };
            page.Controls.Add(grid); views.TabPages.Add(page); return grid;
        }
    }

    private void ShowAdditionalFields(PartProject value)
    {
        foreach (var pair in additionalFields) pair.Value.Text = (string)typeof(PartProject).GetProperty(pair.Key)!.GetValue(value)!;
    }
    private void ReadAdditionalFields(PartProject value)
    {
        foreach (var pair in additionalFields) typeof(PartProject).GetProperty(pair.Key)!.SetValue(value, pair.Value.Text);
    }

    private async void OpenEdz_Click(object? sender, EventArgs e)
    {
        if (!CheckUnsaved()) return;
        using var dialog = new OpenFileDialog { Title = "打开已有 EDZ 部件包", Filter = "EPLAN 部件包|*.edz|所有文件|*.*" };
        if (dialog.ShowDialog(this) == DialogResult.OK) await OpenPackageAsync(dialog.FileName);
    }
    private async void OpenPackageFolder_Click(object? sender, EventArgs e)
    {
        if (!CheckUnsaved()) return;
        using var dialog = new FolderBrowserDialog { Description = "选择包含 manifest.xml 的已解开部件目录", UseDescriptionForTitle = true };
        if (dialog.ShowDialog(this) == DialogResult.OK) await OpenPackageAsync(dialog.SelectedPath);
    }

    private async Task OpenPackageAsync(string source)
    {
        exporting = true; toolbar.Enabled = false; editorPanel.Enabled = false; UseWaitCursor = true;
        status.Text = "正在读取部件包并建立独立快照…";
        try
        {
            List<PartProject> choices = await Task.Run(() => PackageStore.Open(source));
            PartProject selected = choices[0];
            if (choices.Count > 1)
            {
                using var picker = new PackageSelectionForm(choices);
                if (picker.ShowDialog(this) != DialogResult.OK) return;
                selected = choices[picker.SelectedIndex];
            }
            ShowProject(selected, "");
            savedState = "";
            resultBox.Text = "原包已打开。修改指定字段或选择替换文件，然后生成新的 EDZ。\r\n连接点排列样式、自定义属性、未选中的部件和变量会保留；原包版本保持不变。";
            status.Text = "已打开：" + source;
        }
        catch (Exception ex) { ShowError("原包打开失败，当前资料保持不变", ex); }
        finally { exporting = false; toolbar.Enabled = true; editorPanel.Enabled = true; UseWaitCursor = false; }
    }

    private void RefreshPackageDetails(PartProject value)
    {
        originalFiles.DataSource = null; originalAttributes.DataSource = null; connectionPoints.DataSource = null;
        if (value.Package == null) { packageSummary.Text = "尚未打开原包。可选择“打开 EDZ”或“打开目录”；新建部件仍按基础流程生成。"; return; }
        var source = value.Package;
        var manifest = PackageStore.LoadXml(Path.Combine(source.Folder, "manifest.xml"));
        var package = manifest.Root!.Element("packages")!.Elements("package").ElementAt(source.PackageIndex);
        var files = new DataTable(); files.Columns.Add("类型"); files.Columns.Add("名称"); files.Columns.Add("文件路径"); files.Columns.Add("字节数");
        foreach (var item in manifest.Descendants("item"))
        {
            string path = PackageStore.ItemPath(source.Folder, item);
            files.Rows.Add((string?)item.Attribute("type"), (string?)item.Attribute("name"), (string?)item.Attribute("locator"), new FileInfo(path).Length.ToString());
        }
        originalFiles.DataSource = files;
        var partItem = package.Descendants("item").Single(i => (string?)i.Attribute("name") == "part");
        var document = PackageStore.LoadXml(PackageStore.ItemPath(source.Folder, partItem));
        var article = document.Root!.Elements("part").ElementAt(source.PartIndex);
        var variantElement = article.Elements("variant").ElementAt(source.VariantIndex);
        var attrs = new DataTable(); attrs.Columns.Add("位置"); attrs.Columns.Add("属性"); attrs.Columns.Add("原始值");
        AddAttributes("部件", article); AddAttributes("变量", variantElement);
        foreach (var child in article.Elements().Where(e => e.Name != "variant")) attrs.Rows.Add("附加记录", child.Name.ToString(), child.ToString());
        var manufacturerItem = package.Descendants("item").FirstOrDefault(i => (string?)i.Attribute("name") == "manufacturer");
        if (manufacturerItem != null)
            foreach (var address in PackageStore.LoadXml(PackageStore.ItemPath(source.Folder, manufacturerItem)).Descendants("address")) AddAttributes("厂家", address);
        originalAttributes.DataSource = attrs;
        var points = new DataTable();
        foreach (string column in new[] { "排列样式", "端子", "X", "Y", "Z", "X方向", "Y方向", "Z方向", "最小截面", "最大截面" }) points.Columns.Add(column);
        foreach (var item in package.Descendants("item").Where(i => (string?)i.Attribute("name") == "connectionpoints"))
        {
            var cpp = PackageStore.LoadXml(PackageStore.ItemPath(source.Folder, item));
            foreach (var terminal in cpp.Descendants("terminal"))
                foreach (var point in terminal.Elements("terminalPosition"))
                    points.Rows.Add(PackageStore.Text(terminal, "P_PART_TERMINAL_NAME"), PackageStore.Text(point, "name"), PackageStore.Text(point, "xpos"), PackageStore.Text(point, "ypos"), PackageStore.Text(point, "zpos"), PackageStore.Text(point, "xdir"), PackageStore.Text(point, "ydir"), PackageStore.Text(point, "zdir"), PackageStore.Text(point, "mincrosssection"), PackageStore.Text(point, "maxcrosssection"));
        }
        connectionPoints.DataSource = points;
        string version = PackageStore.Text(document.Root, "version");
        string macroVersion = "";
        string layout = PackageStore.Baseline(value).LayoutMacro;
        if (File.Exists(layout))
        {
            try
            {
                using var reader = XmlReader.Create(layout, new XmlReaderSettings { DtdProcessing = DtdProcessing.Prohibit, XmlResolver = null });
                if (reader.MoveToContent() == XmlNodeType.Element) macroVersion = reader.GetAttribute("Version") ?? "";
            }
            catch (XmlException) { macroVersion = "无法直接识别，文件按原样保留"; }
        }
        packageSummary.Text = $"原始来源：{source.OriginalSource}\r\n部件：{value.PartNumber} / 变量 {value.Variant}；数据版本：{version}；安装宏版本：{macroVersion}\r\n保留原版本和未编辑记录。原包为 2024 等较新版本时，重新打包不会自动变成 2.9 兼容宏。";
        void AddAttributes(string location, XElement element)
        { foreach (var attribute in element.Attributes()) attrs.Rows.Add(location, attribute.Name.ToString(), attribute.Value); }
    }

    internal void LoadPackageForCheck(string source, int page)
    {
        ShowProject(PackageStore.Open(source)[0], "");
        editorPanel.Controls.OfType<TabControl>().Single().SelectedIndex = page;
        var current = ReadProject();
        var baseline = PackageStore.Baseline(current);
        var snapshot = System.Text.Json.JsonSerializer.Deserialize<PartProject>(System.Text.Json.JsonSerializer.Serialize(current))!;
        snapshot.Package = null;
        if (System.Text.Json.JsonSerializer.Serialize(snapshot) != System.Text.Json.JsonSerializer.Serialize(baseline))
            throw new InvalidOperationException("原包加载后，界面提交改变了未编辑字段。");
        if (string.IsNullOrWhiteSpace(productGroup.GetItemText(productGroup.SelectedItem)))
            throw new InvalidOperationException("原包产品组没有显示名称。");
    }

    internal void VerifyPackageEditor(string source, string folder)
    {
        LoadPackageForCheck(source, 0);
        string originalMacro = project.LayoutMacro;
        partName.Text = "界面保存检查";
        additionalFields["RatedCurrent"].Text = "8 A";
        currentPath = Path.Combine(folder, "ui-imported.epart.json");
        if (!SaveCurrent()) throw new InvalidOperationException("导入资料界面保存失败。");
        string destination = Path.Combine(folder, "ui-imported.edz");
        EdzExporter.Export(ReadProject(), destination, catalog);
        var readback = PackageStore.Open(destination).Single();
        if (readback.Name != "界面保存检查" || readback.RatedCurrent != "8 A")
            throw new InvalidOperationException("界面修改没有保存在输出 EDZ。");
        using var first = File.OpenRead(originalMacro);
        using var second = File.OpenRead(readback.LayoutMacro);
        if (!System.Security.Cryptography.SHA256.HashData(first).SequenceEqual(System.Security.Cryptography.SHA256.HashData(second)))
            throw new InvalidOperationException("界面编辑改变了未替换的原始宏。");
    }
}

internal sealed class PackageSelectionForm : Form
{
    private readonly ListBox list;
    public int SelectedIndex => list.SelectedIndex;
    public PackageSelectionForm(IReadOnlyList<PartProject> choices)
    {
        Text = "选择要编辑的部件和变量"; ClientSize = new Size(760, 380); StartPosition = FormStartPosition.CenterParent;
        Font = new Font("Microsoft YaHei UI", 10);
        var layout = new TableLayoutPanel { Dock = DockStyle.Fill, Padding = new Padding(15), RowCount = 3, ColumnCount = 1 };
        layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35)); layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100)); layout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40));
        layout.Controls.Add(new Label { Text = "选择一个部件变量编辑，其余部件和变量保留在输出包中。", Dock = DockStyle.Fill }, 0, 0);
        list = new ListBox { Dock = DockStyle.Fill, IntegralHeight = false };
        foreach (var choice in choices) list.Items.Add(choice.PartNumber + " / 变量 " + choice.Variant + " / " + choice.Name);
        list.SelectedIndex = 0; layout.Controls.Add(list, 0, 1);
        var button = new Button { Text = "打开所选项", Dock = DockStyle.Right, Width = 130, DialogResult = DialogResult.OK };
        layout.Controls.Add(button, 0, 2); Controls.Add(layout); AcceptButton = button;
    }
}
