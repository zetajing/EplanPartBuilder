using EplanPartBuilder;

namespace EplanPartBuilder.Checks;

internal static class Program
{
    // 开发检查入口与正常启动程序分开，输出目录由调用者指定。
    [STAThread]
    private static int Main(string[] args)
    {
        ApplicationConfiguration.Initialize();
        try
        {
            if (args.Length == 2 && args[0] == "--self-check")
                return SelfCheck.Run(args[1]);
            if (args.Length == 3 && args[0] == "--package-check")
                return PackageChecks.Run(args[1], args[2]);
            if (args.Length is 2 or 3 && args[0] == "--ui-check")
                return UiChecks.Run(args[1], args.Length == 3 ? args[2] : null);

            Console.WriteLine("用法：--self-check <输出目录> | --package-check <样包目录> <输出目录> | --ui-check <输出目录> [样包目录]");
            return 1;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex);
            return 1;
        }
    }
}
