using System.Reflection;
using System.Security.Cryptography;
using System.Text.Json;

namespace EplanPartBuilder.Checks;

internal static class UiChecks
{
    public static int Run(string folder, string? source)
    {
        Directory.CreateDirectory(folder);
        using var form = new MainForm();
        try
        {
            // 无参数构造即可得到全部固定控件和列，设计器不需要运行资料加载代码。
            CheckDesignerColumns(form, "functionGrid", 7);
            CheckDesignerColumns(form, "originalFiles", 4);
            CheckDesignerColumns(form, "originalAttributes", 3);
            CheckDesignerColumns(form, "connectionPoints", 10);
            using (var picker = new FunctionPickerForm())
                Require(picker.Controls.Find("searchBox", true).Length == 1, "功能窗口缺少固定搜索框。");
            using (var picker = new PackageSelectionForm())
                Require(picker.Controls.Find("list", true).Length == 1, "部件窗口缺少固定列表。");
            form.Show();
            Application.DoEvents();
            Call(form, "ShowProject", PartProject.RelayExample(), "");
            var tabs = Field<Panel>(form, "editorPanel").Controls.OfType<TabControl>().Single();
            tabs.SelectedIndex = 2;
            var grid = Field<DataGridView>(form, "functionGrid");
            grid.CurrentCell = grid.Rows[0].Cells["Connections"];
            grid.BeginEdit(false);
            if (grid.EditingControl is not TextBox editor) throw new InvalidOperationException("端子单元格无法进入编辑状态。");
            editor.Text = "A1|A9";
            var model = (PartProject)Call(form, "ReadProject")!;
            Require(model.Functions[0].Connections == "A1|A9", "最后编辑的单元格没有提交。");
            Call(form, "MoveFunction", 1);
            Require(model.Functions[1].Connections == "A1|A9", "功能移动失败。");
            SetField(form, "currentPath", Path.Combine(folder, "ui-saved.epart.json"));
            Require((bool)Call(form, "SaveCurrent")!, "界面保存失败。");
            Require(ProjectStore.Load(Path.Combine(folder, "ui-saved.epart.json")).Functions[1].Connections == "A1|A9", "界面保存缺少最后编辑的单元格。");
            Require((bool)Call(form, "CheckUnsaved")!, "刚保存的数据错误地标记为未保存。");

            CheckConnectionPointScrolling(form, tabs, folder);
            if (source != null) CheckPackage(form, tabs, folder, source);
            Application.DoEvents();
            using var image = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(image, new Rectangle(0, 0, form.Width, form.Height));
            image.Save(Path.Combine(folder, "main-window.png"));
            File.WriteAllText(Path.Combine(folder, "ui-report.txt"),
                "PASS：Designer 固定控件和列、端子编辑提交、功能移动、保存与恢复、保存状态、主窗口渲染。\r\n" +
                "PASS：200 行连接点在最小窗口和大窗口中可用滚动条、滚轮滚动，末行可见，超宽列可横向滚动。" +
                (source == null ? "" : "\r\nPASS：真实包界面加载不改字段、产品组显示、界面修改与再次导出、原宏完整性。"));
            return 0;
        }
        finally
        {
            // 清空检查资料后正常关闭，不在正式窗体中保留跳过保存提示的开关。
            Call(form, "ShowProject", new PartProject(), "");
            form.Close();
        }
    }

    private static void CheckConnectionPointScrolling(MainForm form, TabControl tabs, string folder)
    {
        // 多于一屏的连接点用于检查滚动条、滚轮和末行显示，不修改真实样包。
        var points = new System.Data.DataTable();
        foreach (string name in new[] { "排列样式", "端子", "X", "Y", "Z", "X方向", "Y方向", "Z方向", "最小截面", "最大截面" })
            points.Columns.Add(name);
        for (int i = 1; i <= 200; i++)
            points.Rows.Add("SIE.6ES7 测试排列样式", i, "19.123456", "91.123456", "21.123456", "0", "1", "0", "0.3", "2");
        var grid = Field<DataGridView>(form, "connectionPoints");
        grid.DataSource = points;
        tabs.SelectedIndex = 4;
        Field<TabControl>(form, "originalTabs").SelectedIndex = 2;
        Application.DoEvents();
        foreach (Size size in new[] { form.MinimumSize, new Size(1280, 900) })
        {
            form.Size = size;
            Application.DoEvents();
            var bar = grid.Controls.OfType<VScrollBar>().Single();
            Require(bar.Visible && bar.Enabled, "连接点表格的垂直滚动条不可用。");
            typeof(Control).GetMethod("OnMouseEnter", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(grid, new object[] { EventArgs.Empty });
            Require(grid.ContainsFocus, "鼠标进入连接点表格后没有接收焦点。");
            grid.FirstDisplayedScrollingRowIndex = 0;
            typeof(DataGridView).GetMethod("OnMouseWheel", BindingFlags.Instance | BindingFlags.NonPublic)!
                .Invoke(grid, new object[] { new MouseEventArgs(MouseButtons.None, 0, 30, 40, -120) });
            Require(grid.FirstDisplayedScrollingRowIndex > 0, "连接点表格不能通过滚轮向下滚动。");
            grid.FirstDisplayedScrollingRowIndex = grid.Rows.Count - 1;
            Require(grid.Rows[grid.Rows.Count - 1].Displayed, "连接点最后一行不能滚入视区。");
            if (grid.Columns.GetColumnsWidth(DataGridViewElementStates.Visible) > grid.ClientSize.Width)
            {
                var horizontal = grid.Controls.OfType<HScrollBar>().Single();
                Require(horizontal.Visible && horizontal.Enabled, "连接点表格的水平滚动条不可用。");
                grid.HorizontalScrollingOffset = 150;
                Require(grid.HorizontalScrollingOffset > 0, "连接点表格不能横向滚动。");
            }
        }
        using var image = new Bitmap(form.Width, form.Height);
        form.DrawToBitmap(image, new Rectangle(Point.Empty, form.Size));
        image.Save(Path.Combine(folder, "connection-points-last-row.png"));
        tabs.SelectedIndex = 0;
    }

    private static void CheckDesignerColumns(MainForm form, string name, int count)
    {
        var grid = Field<DataGridView>(form, name);
        Require(!grid.AutoGenerateColumns && grid.Columns.Count == count, name + " 的列没有完整保存在 Designer 中。");
    }

    private static void CheckPackage(MainForm form, TabControl tabs, string folder, string source)
    {
        Call(form, "ShowProject", PackageStore.Open(source)[0], "");
        tabs.SelectedIndex = 0;
        var model = (PartProject)Call(form, "ReadProject")!;
        var baseline = PackageStore.Baseline(model);
        var copy = JsonSerializer.Deserialize<PartProject>(JsonSerializer.Serialize(model))!;
        copy.Package = null;
        Require(JsonSerializer.Serialize(copy) == JsonSerializer.Serialize(baseline), "界面提交改变了未编辑的原包字段。");
        var group = Field<ComboBox>(form, "productGroup");
        Require(!string.IsNullOrWhiteSpace(group.GetItemText(group.SelectedItem)), "产品组没有显示名称。");

        string originalMacro = model.LayoutMacro;
        Field<TextBox>(form, "partName").Text = "界面保存检查";
        Field<TextBox>(form, "ratedCurrent").Text = "8 A";
        SetField(form, "currentPath", Path.Combine(folder, "ui-imported.epart.json"));
        Require((bool)Call(form, "SaveCurrent")!, "导入资料界面保存失败。");
        string path = Path.Combine(folder, "ui-imported.edz");
        EdzExporter.Export((PartProject)Call(form, "ReadProject")!, path, FunctionCatalog.Load());
        var readback = PackageStore.Open(path).Single();
        Require(readback.Name == "界面保存检查" && readback.RatedCurrent == "8 A", "界面修改没有写入输出 EDZ。");
        using var first = File.OpenRead(originalMacro);
        using var second = File.OpenRead(readback.LayoutMacro);
        Require(SHA256.HashData(first).SequenceEqual(SHA256.HashData(second)), "界面编辑改变了未替换的原宏。");
    }

    // 检查项目调用现有的窗口成员，正式窗口不需要暴露测试专用接口。
    private static object? Call(MainForm form, string name, params object[] args)
        => typeof(MainForm).GetMethod(name, BindingFlags.Instance | BindingFlags.NonPublic)!.Invoke(form, args);
    private static T Field<T>(MainForm form, string name)
        => (T)typeof(MainForm).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.GetValue(form)!;
    private static void SetField(MainForm form, string name, object value)
        => typeof(MainForm).GetField(name, BindingFlags.Instance | BindingFlags.NonPublic)!.SetValue(form, value);
    private static void Require(bool passed, string message)
    {
        if (!passed) throw new InvalidOperationException(message);
    }
}
