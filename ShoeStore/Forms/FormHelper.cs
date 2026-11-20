namespace ShoeStore.Forms;

internal static class FormHelper
{
    private const string WarningCaption = "Проверка данных";
    private const string ConfirmCaption = "Подтверждение";

    public static DataGridView CreateReadOnlyGrid()
    {
        return new DataGridView
        {
            Dock = DockStyle.Fill,
            AutoGenerateColumns = false,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            AllowUserToResizeRows = false,
            MultiSelect = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = SystemColors.Window
        };
    }

    public static void AddColumn(DataGridView grid, string propertyName, string headerText, string? format = null)
    {
        var column = new DataGridViewTextBoxColumn
        {
            Name = "column" + propertyName,
            DataPropertyName = propertyName,
            HeaderText = headerText
        };

        if (format is not null)
        {
            column.DefaultCellStyle.Format = format;
        }

        grid.Columns.Add(column);
    }

    public static Button CreateButton(string text)
    {
        return new Button
        {
            Text = text,
            AutoSize = true,
            MinimumSize = new Size(110, 30)
        };
    }

    public static Label CreateLabel(string text)
    {
        return new Label
        {
            Text = text,
            AutoSize = true,
            TextAlign = ContentAlignment.MiddleLeft,
            Margin = new Padding(3, 6, 12, 6)
        };
    }

    public static TableLayoutPanel CreateListLayout(DataGridView grid, params Button[] buttons)
    {
        var buttonsPanel = new FlowLayoutPanel
        {
            Dock = DockStyle.Fill,
            AutoSize = true,
            FlowDirection = FlowDirection.LeftToRight
        };
        buttonsPanel.Controls.AddRange(buttons);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 2,
            Padding = new Padding(10)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(grid, 0, 0);
        layout.Controls.Add(buttonsPanel, 0, 1);

        return layout;
    }

    public static void ConfigureEditDialog(
        Form form,
        string title,
        Button saveButton,
        Button cancelButton,
        params (string Label, Control Field)[] fields)
    {
        form.Text = title;
        form.FormBorderStyle = FormBorderStyle.FixedDialog;
        form.MaximizeBox = false;
        form.MinimizeBox = false;
        form.StartPosition = FormStartPosition.CenterParent;
        form.AutoSize = true;
        form.AutoSizeMode = AutoSizeMode.GrowAndShrink;
        form.AcceptButton = saveButton;
        form.CancelButton = cancelButton;

        var layout = new TableLayoutPanel
        {
            AutoSize = true,
            AutoSizeMode = AutoSizeMode.GrowAndShrink,
            ColumnCount = 2,
            Padding = new Padding(12)
        };

        foreach (var (label, field) in fields)
        {
            field.Width = 280;
            layout.Controls.Add(CreateLabel(label));
            layout.Controls.Add(field);
        }

        var buttonsPanel = new FlowLayoutPanel
        {
            AutoSize = true,
            FlowDirection = FlowDirection.RightToLeft,
            Anchor = AnchorStyles.Right
        };
        buttonsPanel.Controls.Add(cancelButton);
        buttonsPanel.Controls.Add(saveButton);

        layout.Controls.Add(buttonsPanel);
        layout.SetColumnSpan(buttonsPanel, 2);
        form.Controls.Add(layout);
    }

    public static void ShowWarning(string message)
    {
        MessageBox.Show(message, WarningCaption, MessageBoxButtons.OK, MessageBoxIcon.Warning);
    }

    public static bool Confirm(string message)
    {
        return MessageBox.Show(message, ConfirmCaption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes;
    }
}
