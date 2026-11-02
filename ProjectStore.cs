using System.Text.Json;

namespace EplanPartBuilder;

public static class ProjectStore
{
    private static readonly JsonSerializerOptions Options = new() { WriteIndented = true };

    public static PartProject Load(string path)
    {
        // 先完整读取并检查，再交给界面替换当前资料，读取失败不会清空正在编辑的内容。
        var project = JsonSerializer.Deserialize<PartProject>(File.ReadAllText(path), Options)
                      ?? throw new InvalidDataException("资料内容为空。");
        if (project.FileVersion != 1 || project.Functions == null || project.Functions.Any(f => f == null))
            throw new InvalidDataException("资料版本或功能模板格式不受支持。");
        CheckStrings(project);
        foreach (FunctionRow row in project.Functions) CheckStrings(row);
        if (project.Width < 0 || project.Width > 1000000 || project.Height < 0 || project.Height > 1000000 || project.Depth < 0 || project.Depth > 1000000)
            throw new InvalidDataException("尺寸应在 0 到 1000000 mm 之间。");
        if (project.Package == null && !new[] { 0, 2, 3, 4, 5, 6, 9, 20, 26 }.Contains(project.ProductGroup))
            throw new InvalidDataException("资料中的产品组暂不支持，未加载该文件。");
        PackageStore.RestoreSavedPaths(project, path);
        if (project.Package != null)
        {
            EdzExporter.VerifyManifest(project.Package.Folder, PackageStore.LoadXml(Path.Combine(project.Package.Folder, "manifest.xml")));
            _ = PackageStore.Baseline(project);
        }
        return project;
    }

    public static PartProject Save(string path, PartProject project)
    {
        // 先写同目录临时文件，成功后替换目标，避免写入中断破坏原资料。
        PartProject saved = PackageStore.Persist(path, project);
        var fileData = JsonSerializer.Deserialize<PartProject>(JsonSerializer.Serialize(saved))!;
        PackageStore.MakeSavedPathsRelative(fileData, path);
        AtomicFile.Write(path, temp => File.WriteAllText(temp, JsonSerializer.Serialize(fileData, Options)));
        return saved;
    }

    private static void CheckStrings(object value)
    {
        // 拒绝损坏 JSON 中的 null 文本，避免加载后才在界面或导出过程中报空引用。
        foreach (var property in value.GetType().GetProperties())
            if (property.PropertyType == typeof(string) && property.GetValue(value) == null)
                throw new InvalidDataException("资料中的文本字段为空值：" + property.Name);
    }
}

public static class AtomicFile
{
    public static void Write(string destination, Action<string> write)
    {
        destination = Path.GetFullPath(destination);
        Directory.CreateDirectory(Path.GetDirectoryName(destination)!);
        string temp = Path.Combine(Path.GetDirectoryName(destination)!, "." + Guid.NewGuid().ToString("N") + ".tmp");
        try
        {
            write(temp);
            File.Move(temp, destination, true);
        }
        finally
        {
            if (File.Exists(temp)) File.Delete(temp);
        }
    }
}
