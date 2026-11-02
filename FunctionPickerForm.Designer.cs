#nullable disable
namespace EplanPartBuilder;

partial class FunctionPickerForm
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
        pickerLayout = new TableLayoutPanel();
        searchHint = new Label();
        searchBox = new TextBox();
        results = new ListBox();
        descriptionLabel = new Label();
        pickerButtons = new FlowLayoutPanel();
        cancelButton = new Button();
        confirmButton = new Button();
        pickerLayout.SuspendLayout();
        pickerButtons.SuspendLayout();
        SuspendLayout();
        //
        // pickerLayout
        //
        pickerLayout.Name = "pickerLayout";
        pickerLayout.Dock = DockStyle.Fill;
        pickerLayout.ColumnCount = 1;
        pickerLayout.RowCount = 5;
        pickerLayout.Padding = new Padding(15);
        //
        // searchHint
        //
        searchHint.Name = "searchHint";
        searchHint.Text = "输入名称或编号，例如：线圈、PLC、数字输入、300.1.2";
        searchHint.Dock = DockStyle.Fill;
        //
        // searchBox
        //
        searchBox.Name = "searchBox";
        searchBox.Dock = DockStyle.Fill;
        //
        // results
        //
        results.Name = "results";
        results.Dock = DockStyle.Fill;
        results.IntegralHeight = false;
        results.DisplayMember = "Display";
        //
        // descriptionLabel
        //
        descriptionLabel.Name = "descriptionLabel";
        descriptionLabel.Dock = DockStyle.Fill;
        descriptionLabel.ForeColor = Color.DimGray;
        descriptionLabel.Text = "选择功能后显示类别和分组。";
        //
        // pickerButtons
        //
        pickerButtons.Name = "pickerButtons";
        pickerButtons.Dock = DockStyle.Fill;
        pickerButtons.FlowDirection = FlowDirection.RightToLeft;
        //
        // cancelButton
        //
        cancelButton.Name = "cancelButton";
        cancelButton.Text = "取消";
        cancelButton.Size = new Size(90, 33);
        cancelButton.Margin = new Padding(0, 0, 8, 0);
        cancelButton.FlatStyle = FlatStyle.Flat;
        cancelButton.BackColor = Color.White;
        cancelButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        cancelButton.DialogResult = DialogResult.Cancel;
        //
        // confirmButton
        //
        confirmButton.Name = "confirmButton";
        confirmButton.Text = "选用功能";
        confirmButton.Size = new Size(110, 33);
        confirmButton.Margin = new Padding(0, 0, 8, 0);
        confirmButton.FlatStyle = FlatStyle.Flat;
        confirmButton.BackColor = Color.White;
        confirmButton.FlatAppearance.BorderColor = Color.FromArgb(205, 213, 222);
        pickerLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
        pickerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 28F));
        pickerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 38F));
        pickerLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
        pickerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 45F));
        pickerLayout.RowStyles.Add(new RowStyle(SizeType.Absolute, 40F));
        confirmButton.Click += new EventHandler(ConfirmButton_Click);
        searchBox.TextChanged += new EventHandler(SearchBox_TextChanged);
        results.DoubleClick += new EventHandler(ConfirmButton_Click);
        results.SelectedIndexChanged += new EventHandler(Results_SelectedIndexChanged);
        pickerLayout.Controls.Add(searchHint, 0, 0);
        pickerLayout.Controls.Add(searchBox, 0, 1);
        pickerLayout.Controls.Add(results, 0, 2);
        pickerLayout.Controls.Add(descriptionLabel, 0, 3);
        pickerLayout.Controls.Add(pickerButtons, 0, 4);
        pickerButtons.Controls.Add(cancelButton);
        pickerButtons.Controls.Add(confirmButton);
        //
        // FunctionPickerForm
        //
        AutoScaleDimensions = new SizeF(96F, 96F);
        AutoScaleMode = AutoScaleMode.Dpi;
        ClientSize = new Size(740, 540);
        MinimumSize = new Size(600, 400);
        Font = new Font("Microsoft YaHei UI", 9.5F);
        Name = "FunctionPickerForm";
        Text = "选择 EPLAN 2.9 功能定义";
        StartPosition = FormStartPosition.CenterScreen;
        Controls.Add(pickerLayout);
        StartPosition = FormStartPosition.CenterParent;
        AcceptButton = confirmButton;
        CancelButton = cancelButton;
        pickerButtons.ResumeLayout(false);
        pickerButtons.PerformLayout();
        pickerLayout.ResumeLayout(false);
        pickerLayout.PerformLayout();
        ResumeLayout(false);
    }

    private TableLayoutPanel pickerLayout;
    private Label searchHint;
    private TextBox searchBox;
    private ListBox results;
    private Label descriptionLabel;
    private FlowLayoutPanel pickerButtons;
    private Button cancelButton;
    private Button confirmButton;
}
