using System.Data;
using System.Xml;
using System.Xml.Linq;

namespace EplanPartBuilder;

public partial class MainForm
{
    private void ReadOnlyGrid_MouseEnter(object? sender, EventArgs e)
    {
        // 鼠标进入只读表格时接收滚轮，避免焦点留在上一个分页而无法滚动。
        if (sender is DataGridView grid && !grid.ContainsFocus) grid.Focus();
    }

    // 这里只填写数据，所有控件及表格列都在 Designer 中声明。
    private void ShowAdditionalFields(PartProject value)
    {
        ratedCurrent.Text = value.RatedCurrent;
        voltage.Text = value.Voltage;
        voltageType.Text = value.VoltageType;
        powerDissipation.Text = value.PowerDissipation;
        crossSection.Text = value.CrossSection;
        manufacturerName.Text = value.ManufacturerName;
        manufacturerCompany.Text = value.ManufacturerCompany;
        manufacturerEmail.Text = value.ManufacturerEmail;
        manufacturerPhone.Text = value.ManufacturerPhone;
    }

    private void ReadAdditionalFields(PartProject value)
    {
        value.RatedCurrent = ratedCurrent.Text;
        value.Voltage = voltage.Text;
        value.VoltageType = voltageType.Text;
        value.PowerDissipation = powerDissipation.Text;
        value.CrossSection = crossSection.Text;
        value.ManufacturerName = manufacturerName.Text;
        value.ManufacturerCompany = manufacturerCompany.Text;
        value.ManufacturerEmail = manufacturerEmail.Text;
        value.ManufacturerPhone = manufacturerPhone.Text;
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

}
