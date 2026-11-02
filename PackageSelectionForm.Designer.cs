#nullable disable
namespace EplanPartBuilder;

partial class PackageSelectionForm
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
        components = new System.ComponentModel.Container();
        selectionLayout = new TableLayoutPanel();
        selectionHint = new Label();
        list = new ListBox();
        selectButton = new Button();
        selectionLayout.SuspendLayout();
        SuspendLayout();
        //
        // selectionLayout
        //
        selectionLayout.Name = "selectionLayout";
        selectionLayout.Dock = DockStyle.Fill;
        selectionLayout.ColumnCount = 1;
        selectionLayout.RowCount = 3;
        selectionLayout.Padding = new Padding(15);
        //
        // selectionHint
        //
        selectionHint.Name = "selectionHint";
        selectionHint.Text = "选择一个部件变量编辑，其余部件和变量保留在输出包中。";
        selectionHint.Dock = DockStyle.Fill;
        //
        // list
        //
        list.Name = "list";
        list.Dock = DockStyle.Fill;
        list.IntegralHeight = false;
        //
        // selectButton
        //
        selectButton.Name = "selectButton";
        selectButton.Text = "打开所选项";
        selectButton.Size = new Size(130, 33);
        selectButton.Margin = new Padding(0, 0, 8, 0);
        selectButton.FlatStyle = FlatStyle.Flat;
        selectButton.BackColor = Color.White;
        selectButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        selectButton.Dock = DockStyle.Right;
        selectButton.DialogResult = DialogResult.OK;
        selectionLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        selectionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 35F));
        selectionLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        selectionLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        selectionLayout.Controls.Add(selectionHint, 0, 0);
        selectionLayout.Controls.Add(list, 0, 1);
        selectionLayout.Controls.Add(selectButton, 0, 2);
        //
        // PackageSelectionForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(760, 380);
        MinimumSize = new Size(600, 300);
        Font = new Font("Microsoft YaHei UI", 9.5F);
        Name = "PackageSelectionForm";
        Text = "选择要编辑的部件和变量";
        StartPosition = FormStartPosition.CenterScreen;
        Controls.Add(selectionLayout);
        StartPosition = FormStartPosition.CenterParent;
        AcceptButton = selectButton;
        selectionLayout.ResumeLayout(false);
        selectionLayout.PerformLayout();
        ResumeLayout(false);
    }

    private TableLayoutPanel selectionLayout;
    private Label selectionHint;
    private ListBox list;
    private Button selectButton;
}
