namespace EplanPartBuilder;

public partial class PackageSelectionForm : Form
{
    [System.ComponentModel.Browsable(false)]
    [System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)]
    public int SelectedIndex => list.SelectedIndex;

    public PackageSelectionForm()
    {
        // 固定界面由 Designer 负责，运行时只向列表填入数据。
        InitializeComponent();
    }

    public PackageSelectionForm(IReadOnlyList<PartProject> choices) : this()
    {
        foreach (PartProject choice in choices)
            list.Items.Add(choice.PartNumber + " / 变量 " + choice.Variant + " / " + choice.Name);
        if (list.Items.Count > 0) list.SelectedIndex = 0;
    }
}
