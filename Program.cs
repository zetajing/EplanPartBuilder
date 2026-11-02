namespace EplanPartBuilder;

internal static class Program
{
    // 普通启动打开窗口；命令行入口用于离线验证交付文件。
    [STAThread]
    private static int Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        try
        {
            if (args.Length > 0 && args[0] == "--self-check")
                return SelfCheck.Run(args.Length > 1 ? args[1] : Path.Combine(AppContext.BaseDirectory, "checks"));
            if (args.Length > 2 && args[0] == "--package-check")
                return PackageChecks.Run(args[1], args[2]);

            if (args.Length > 1 && args[0] == "--render-check")
            {
                using var form = new MainForm();
                form.Show();
                if (args.Length > 2) form.PreparePreview(int.Parse(args[2]));
                if (args.Length > 3) form.LoadPackageForCheck(args[3], int.Parse(args[2]));
                Application.DoEvents();
                using var bitmap = new Bitmap(form.Width, form.Height);
                form.DrawToBitmap(bitmap, new Rectangle(0, 0, form.Width, form.Height));
                bitmap.Save(Path.GetFullPath(args[1]));
                form.SkipClosePrompt = true;
                form.Close();
                return 0;
            }

            if (args.Length > 1 && args[0] == "--ui-check")
            {
                Directory.CreateDirectory(args[1]);
                using var form = new MainForm();
                form.Show();
                Application.DoEvents();
                form.VerifyEditor(args[1]);
                if (args.Length > 2) form.VerifyPackageEditor(args[2], args[1]);
                form.SkipClosePrompt = true;
                form.Close();
                File.WriteAllText(Path.Combine(args[1], "ui-report.txt"), "PASS：端子单元格编辑提交、功能移动、界面保存与重新读取、保存状态检查。" + (args.Length > 2 ? "\r\nPASS：真实包界面加载不改数据、改值保存、再次导出和原宏完整性。" : ""));
                return 0;
            }

            Application.Run(new MainForm());
            return 0;
        }
        catch (Exception ex)
        {
            if (args.Length > 0)
                File.WriteAllText(Path.Combine(Path.GetTempPath(), "EplanPartBuilder-error.txt"), ex.ToString());
            else
                MessageBox.Show(ex.Message, "启动失败", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return 1;
        }
    }
}
