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

            if (source != null) CheckPackage(form, tabs, folder, source);
            Application.DoEvents();
            using var image = new Bitmap(form.Width, form.Height);
            form.DrawToBitmap(image, new Rectangle(0, 0, form.Width, form.Height));
            image.Save(Path.Combine(folder, "main-window.png"));
            File.WriteAllText(Path.Combine(folder, "ui-report.txt"),
                "PASS：端子编辑提交、功能移动、保存与恢复、保存状态、主窗口渲染。" +
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
        Field<Dictionary<string, TextBox>>(form, "additionalFields")["RatedCurrent"].Text = "8 A";
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
