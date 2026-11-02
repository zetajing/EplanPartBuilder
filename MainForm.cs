using System.Text.Json;

namespace EplanPartBuilder;

public partial class MainForm : Form
{
    private readonly List<FunctionDefinition> catalog = new();
    private bool runtimeInitialized;
    private PartProject project = new();
    private string currentPath = "";
    private string savedState = "";
    private bool exporting;

    public MainForm()
    {
        InitializeComponent();
    }

    protected override void OnLoad(EventArgs e)
    {
        base.OnLoad(e);
        // 设计器只显示 Designer 中的固定控件，不读取本机部件资料。
        if (DesignMode || System.ComponentModel.LicenseManager.UsageMode == System.ComponentModel.LicenseUsageMode.Designtime || runtimeInitialized) return;
        catalog.AddRange(FunctionCatalog.Load());
        productGroup.DataSource = new List<ProductGroupItem>
        {
            new(0, "未分类（0）"), new(2, "继电器 / 接触器（2）"),
            new(3, "端子（3）"), new(4, "插头（4）"), new(5, "常规设备（5）"),
            new(6, "功率开关设备（6）"), new(9, "电机（9）"),
            new(20, "电源（20）"), new(26, "PLC（26）")
        };
        productGroup.DisplayMember = nameof(ProductGroupItem.Name);
        productGroup.ValueMember = nameof(ProductGroupItem.Id);
        runtimeInitialized = true;
        ShowProject(new PartProject(), "");
    }

    private void ShowProject(PartProject value, string path)
    {
        // 只有读取成功后才切换界面；保存状态用独立字符串记录，不受网格直接编辑影响。
        project = value;
        currentPath = path;
        partNumber.Text = value.PartNumber; manufacturer.Text = value.Manufacturer;
        typeNumber.Text = value.TypeNumber; orderNumber.Text = value.OrderNumber;
        partName.Text = value.Name; description.Text = value.Description;
        characteristics.Text = value.Characteristics; variant.Text = value.Variant;
        var groups = (List<ProductGroupItem>)productGroup.DataSource!;
        if (!groups.Any(g => g.Id == value.ProductGroup))
        {
            groups = new List<ProductGroupItem>(groups) { new(value.ProductGroup, "原包产品组（" + value.ProductGroup + "）") };
            productGroup.DataSource = groups;
        }
        // 直接按列表位置选择，避免加载原包后组合框只更新 Value、文本未刷新的情况。
        productGroup.SelectedIndex = groups.FindIndex(g => g.Id == value.ProductGroup);
        productGroup.Refresh();
        partWidth.Value = value.Width; partHeight.Value = value.Height; partDepth.Value = value.Depth;
        schematicPath.Text = value.SchematicMacro; layoutPath.Text = value.LayoutMacro;
        stepPath.Text = value.StepFile; picturePath.Text = value.Picture; documentPath.Text = value.Document;
        functionGrid.DataSource = value.Functions;
        ShowAdditionalFields(value);
        RefreshPackageDetails(value);
        savedState = SerializeCurrent();
        status.Text = path.Length == 0 ? "新建资料 · 尚未保存" : path;
    }

    private PartProject ReadProject()
    {
        // 先提交正在编辑的单元格，避免导出的端子比界面少最后一次修改。
        if (!functionGrid.EndEdit()) throw new InvalidDataException("请先完成当前单元格的输入。");
        BindingContext?[project.Functions]?.EndCurrentEdit();
        project.PartNumber = partNumber.Text; project.Manufacturer = manufacturer.Text;
        project.TypeNumber = typeNumber.Text; project.OrderNumber = orderNumber.Text;
        project.Name = partName.Text; project.Description = description.Text;
        project.Characteristics = characteristics.Text; project.Variant = variant.Text;
        project.ProductGroup = productGroup.SelectedValue is int id ? id : 0;
        project.Width = partWidth.Value; project.Height = partHeight.Value; project.Depth = partDepth.Value;
        project.SchematicMacro = schematicPath.Text; project.LayoutMacro = layoutPath.Text;
        project.StepFile = stepPath.Text; project.Picture = picturePath.Text; project.Document = documentPath.Text;
        ReadAdditionalFields(project);
        return project;
    }

    private string SerializeCurrent() => JsonSerializer.Serialize(ReadProject());

    private bool CheckUnsaved()
    {
        if (SerializeCurrent() == savedState) return true;
        var answer = MessageBox.Show(this, "当前资料有未保存的修改，是否先保存？", "未保存的资料", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
        if (answer == DialogResult.Cancel) return false;
        return answer != DialogResult.Yes || SaveCurrent();
    }

    private bool SaveCurrent()
    {
        string destination = currentPath;
        if (destination.Length == 0)
        {
            using var dialog = new SaveFileDialog { Title = "保存部件资料", Filter = "部件资料|*.epart.json", DefaultExt = "epart.json", AddExtension = true, FileName = SafeDialogName(partNumber.Text, "新建部件") + ".epart.json" };
            if (dialog.ShowDialog(this) != DialogResult.OK) return false;
            destination = dialog.FileName;
        }
        try
        {
            PartProject saved = ProjectStore.Save(destination, ReadProject());
            ShowProject(saved, destination);
            status.Text = "已保存：" + destination;
            return true;
        }
        catch (Exception ex) { ShowError("保存失败", ex); return false; }
    }

    private void NewButton_Click(object? sender, EventArgs e)
    {
        if (CheckUnsaved()) { ShowProject(new PartProject(), ""); resultBox.Text = "已新建空白部件资料。"; }
    }
    private void ExampleButton_Click(object? sender, EventArgs e)
    {
        if (CheckUnsaved())
        {
            ShowProject(PartProject.RelayExample(), "");
            // 示例属于新内容，关闭时应提醒用户保存。
            savedState = "";
            resultBox.Text = "已载入假设继电器示例。实际参数、连接点和宏请按真实产品替换。";
        }
    }
    private void SaveButton_Click(object? sender, EventArgs e) => SaveCurrent();

    private void OpenButton_Click(object? sender, EventArgs e)
    {
        if (!CheckUnsaved()) return;
        using var dialog = new OpenFileDialog { Filter = "部件资料|*.epart.json;*.json", Title = "打开部件资料" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        try
        {
            PartProject loaded = ProjectStore.Load(dialog.FileName);
            ShowProject(loaded, dialog.FileName);
            resultBox.Text = "资料已打开。导出前请检查原始宏和附件的路径是否仍然有效。";
        }
        catch (Exception ex) { ShowError("打开失败，当前资料仍然保留", ex); }
    }

    private void ValidateButton_Click(object? sender, EventArgs e) => CheckCurrent();

    private bool CheckCurrent()
    {
        List<string> errors = PartValidator.Validate(ReadProject(), catalog);
        if (errors.Count > 0)
        {
            resultBox.Text = $"发现 {errors.Count} 项需要修改：\r\n" + string.Join("\r\n", errors);
            return false;
        }
        resultBox.Text = "资料检查通过。\r\n" + DescribeAssets();
        return true;
    }

    private async void ExportButton_Click(object? sender, EventArgs e)
    {
        if (!CheckCurrent()) return;
        using var dialog = new SaveFileDialog { Title = "生成 EDZ 部件包", Filter = "EPLAN 部件交换包|*.edz", DefaultExt = "edz", AddExtension = true, FileName = SafeDialogName(project.PartNumber, "part") + ".edz" };
        if (dialog.ShowDialog(this) != DialogResult.OK) return;
        // 导出使用一份快照，后台运行压缩；期间禁止编辑和关闭，防止导出中途改变源数据。
        PartProject snapshot = JsonSerializer.Deserialize<PartProject>(SerializeCurrent())!;
        exporting = true; toolbar.Enabled = false; editorPanel.Enabled = false;
        status.Text = "正在复制文件、生成清单并检查 EDZ，请稍候…";
        UseWaitCursor = true;
        try
        {
            ExportResult result = await Task.Run(() => EdzExporter.Export(snapshot, dialog.FileName, catalog));
            resultBox.Text = $"EDZ 已生成：{result.Path}\r\n当前编辑：{snapshot.PartNumber} / 变量 {snapshot.Variant}。\r\n{result.Notes}\r\n请在与原宏版本匹配的 EPLAN 测试数据库中导入，核对宏和三维布局。";
            status.Text = "打包完成 · 清单引用和压缩完整性检查通过";
        }
        catch (Exception ex) { ShowError("EDZ 生成失败", ex); status.Text = "导出失败，请查看下方信息。"; }
        finally
        {
            exporting = false; toolbar.Enabled = true; editorPanel.Enabled = true; UseWaitCursor = false;
        }
    }

    private string DescribeAssets()
    {
        string layout = layoutPath.Text.Length == 0 ? "未选择 3D / 安装宏" : "已选择 3D / 安装宏";
        string step = stepPath.Text.Length == 0 ? "未附带 STEP" : "STEP 作为源文件附件，不自动转换";
        return $"功能模板：{project.Functions.Count} 行；{layout}；{step}。" + (project.Package == null ? "" : "\r\n已有原包：保留未编辑字段、关联记录及原版本。外部链接按原值保留。");
    }

    private void Browse(TextBox target)
    {
        using var dialog = new OpenFileDialog { Title = "选择输入文件", Filter = target.Tag?.ToString() ?? "所有文件|*.*", CheckFileExists = true };
        if (File.Exists(target.Text)) dialog.FileName = target.Text;
        if (dialog.ShowDialog(this) == DialogResult.OK) target.Text = dialog.FileName;
    }
    private void BrowseSchematic_Click(object? sender, EventArgs e) => Browse(schematicPath);
    private void BrowseLayout_Click(object? sender, EventArgs e) => Browse(layoutPath);
    private void BrowseStep_Click(object? sender, EventArgs e) => Browse(stepPath);
    private void BrowsePicture_Click(object? sender, EventArgs e) => Browse(picturePath);
    private void BrowseDocument_Click(object? sender, EventArgs e) => Browse(documentPath);
    private void ClearSchematic_Click(object? sender, EventArgs e) => schematicPath.Clear();
    private void ClearLayout_Click(object? sender, EventArgs e) => layoutPath.Clear();
    private void ClearStep_Click(object? sender, EventArgs e) => stepPath.Clear();
    private void ClearPicture_Click(object? sender, EventArgs e) => picturePath.Clear();
    private void ClearDocument_Click(object? sender, EventArgs e) => documentPath.Clear();

    private void AddFunction_Click(object? sender, EventArgs e)
    {
        using var picker = new FunctionPickerForm(catalog);
        if (picker.ShowDialog(this) == DialogResult.OK && picker.SelectedDefinition != null)
        {
            project.Functions.Add(new FunctionRow { Code = picker.SelectedDefinition.Code });
            functionGrid.CurrentCell = functionGrid.Rows[project.Functions.Count - 1].Cells[1];
        }
    }
    private void ChangeFunction_Click(object? sender, EventArgs e)
    {
        int row = functionGrid.CurrentCell?.RowIndex ?? -1;
        if (row < 0) return;
        using var picker = new FunctionPickerForm(catalog);
        if (picker.ShowDialog(this) == DialogResult.OK && picker.SelectedDefinition != null)
        {
            project.Functions[row].Code = picker.SelectedDefinition.Code;
            project.Functions.ResetItem(row);
        }
    }
    private void DeleteFunction_Click(object? sender, EventArgs e)
    {
        int row = functionGrid.CurrentCell?.RowIndex ?? -1;
        if (row >= 0) project.Functions.RemoveAt(row);
    }
    private void MoveFunctionUp_Click(object? sender, EventArgs e) => MoveFunction(-1);
    private void MoveFunctionDown_Click(object? sender, EventArgs e) => MoveFunction(1);
    private void MoveFunction(int direction)
    {
        functionGrid.EndEdit();
        int oldRow = functionGrid.CurrentCell?.RowIndex ?? -1;
        int newRow = oldRow + direction;
        if (oldRow < 0 || newRow < 0 || newRow >= project.Functions.Count) return;
        FunctionRow row = project.Functions[oldRow];
        project.Functions.RemoveAt(oldRow); project.Functions.Insert(newRow, row);
        functionGrid.CurrentCell = functionGrid.Rows[newRow].Cells[0];
    }
    private void FunctionGrid_CellFormatting(object? sender, DataGridViewCellFormattingEventArgs e)
    {
        if (e.ColumnIndex != 0 || e.Value is not string code) return;
        FunctionDefinition? definition = catalog.FirstOrDefault(f => f.Code == code);
        e.Value = definition?.Display ?? code;
        e.FormattingApplied = true;
    }
    private void FunctionGrid_RowPostPaint(object? sender, DataGridViewRowPostPaintEventArgs e)
    {
        TextRenderer.DrawText(e.Graphics, (e.RowIndex + 1).ToString(), functionGrid.Font, new Rectangle(e.RowBounds.Left, e.RowBounds.Top, functionGrid.RowHeadersWidth - 5, e.RowBounds.Height), Color.Gray, TextFormatFlags.Right | TextFormatFlags.VerticalCenter);
    }
    private void FunctionGrid_DataError(object? sender, DataGridViewDataErrorEventArgs e)
    {
        e.ThrowException = false;
        resultBox.Text = "单元格输入无效，请检查该行内容。";
    }
    private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
    {
        if (!runtimeInitialized) return;
        if (exporting) { e.Cancel = true; return; }
        e.Cancel = !CheckUnsaved();
    }
    private void ShowError(string title, Exception ex)
    {
        resultBox.Text = title + "：\r\n" + ex.Message;
        MessageBox.Show(this, ex.Message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
    }
    private static string SafeDialogName(string text, string fallback)
    {
        string name = new(text.Select(c => Path.GetInvalidFileNameChars().Contains(c) ? '_' : c).ToArray());
        return string.IsNullOrWhiteSpace(name) ? fallback : name.Trim();
    }

    private sealed record ProductGroupItem(int Id, string Name);
}
