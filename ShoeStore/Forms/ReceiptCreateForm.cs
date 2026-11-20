using System.ComponentModel;
using ShoeStore.Models;
using ShoeStore.Repositories;

namespace ShoeStore.Forms;

public class ReceiptCreateForm : Form
{
    private readonly DateTimePicker _receiptDatePicker = new() { Format = DateTimePickerFormat.Short, Width = 120 };
    private readonly ComboBox _shoeComboBox = new() { DropDownStyle = ComboBoxStyle.DropDownList, Width = 260 };

    private readonly NumericUpDown _sizeNumeric = new()
    {
        DecimalPlaces = 1,
        Increment = 0.5m,
        Minimum = ShoeSizeRange.Min,
        Maximum = ShoeSizeRange.Max,
        Value = 38m,
        Width = 70
    };

    private readonly NumericUpDown _quantityNumeric = new() { Minimum = 0, Maximum = 10000, Width = 80 };
    private readonly DataGridView _itemsGrid = FormHelper.CreateReadOnlyGrid();
    private readonly BindingList<ReceiptItemRow> _items = new();

    public ReceiptCreateForm(IShoeRepository shoeRepository)
    {
        _shoeComboBox.DisplayMember = nameof(ShoeOption.Title);
        _shoeComboBox.DataSource = shoeRepository.GetAll().Select(ShoeOption.From).ToList();

        InitializeLayout();
    }

    public Receipt? Result { get; private set; }

    private sealed record ReceiptItemRow(int ShoeId, string ShoeName, decimal Size, int Quantity);

    private void InitializeLayout()
    {
        Text = "Новое поступление товара";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(780, 520);
        MinimumSize = new Size(760, 420);

        FormHelper.AddColumn(_itemsGrid, nameof(ReceiptItemRow.ShoeName), "Обувь");
        FormHelper.AddColumn(_itemsGrid, nameof(ReceiptItemRow.Size), "Размер", "0.0");
        FormHelper.AddColumn(_itemsGrid, nameof(ReceiptItemRow.Quantity), "Количество пар");
        _itemsGrid.DataSource = _items;

        var addItemButton = FormHelper.CreateButton("Добавить позицию");
        addItemButton.Click += OnAddItemClick;

        var removeItemButton = FormHelper.CreateButton("Удалить позицию");
        removeItemButton.Click += OnRemoveItemClick;

        var saveButton = FormHelper.CreateButton("Сохранить");
        saveButton.Click += OnSaveClick;

        var cancelButton = FormHelper.CreateButton("Отмена");
        cancelButton.DialogResult = DialogResult.Cancel;
        CancelButton = cancelButton;

        var headerPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        headerPanel.Controls.Add(FormHelper.CreateLabel("Дата поступления:"));
        headerPanel.Controls.Add(_receiptDatePicker);

        var itemPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        itemPanel.Controls.Add(FormHelper.CreateLabel("Обувь:"));
        itemPanel.Controls.Add(_shoeComboBox);
        itemPanel.Controls.Add(FormHelper.CreateLabel("Размер:"));
        itemPanel.Controls.Add(_sizeNumeric);
        itemPanel.Controls.Add(FormHelper.CreateLabel("Количество пар:"));
        itemPanel.Controls.Add(_quantityNumeric);
        itemPanel.Controls.Add(addItemButton);

        var buttonsPanel = new FlowLayoutPanel { Dock = DockStyle.Fill, AutoSize = true };
        buttonsPanel.Controls.Add(removeItemButton);
        buttonsPanel.Controls.Add(saveButton);
        buttonsPanel.Controls.Add(cancelButton);

        var layout = new TableLayoutPanel
        {
            Dock = DockStyle.Fill,
            ColumnCount = 1,
            RowCount = 4,
            Padding = new Padding(10)
        };
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.RowStyles.Add(new RowStyle(SizeType.Percent, 100));
        layout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
        layout.Controls.Add(headerPanel, 0, 0);
        layout.Controls.Add(itemPanel, 0, 1);
        layout.Controls.Add(_itemsGrid, 0, 2);
        layout.Controls.Add(buttonsPanel, 0, 3);

        Controls.Add(layout);
    }

    private void OnAddItemClick(object? sender, EventArgs e)
    {
        if (_shoeComboBox.SelectedItem is not ShoeOption shoe)
        {
            FormHelper.ShowWarning("Выберите обувь. Если список пуст, сначала добавьте обувь в справочнике.");
            return;
        }

        var quantity = (int)_quantityNumeric.Value;

        if (quantity <= 0)
        {
            FormHelper.ShowWarning("Укажите количество пар.");
            _quantityNumeric.Focus();
            return;
        }

        var size = _sizeNumeric.Value;
        var existing = _items.FirstOrDefault(row => row.ShoeId == shoe.Id && row.Size == size);

        if (existing is null)
        {
            _items.Add(new ReceiptItemRow(shoe.Id, shoe.Title, size, quantity));
        }
        else
        {
            _items[_items.IndexOf(existing)] = existing with { Quantity = existing.Quantity + quantity };
        }

        _quantityNumeric.Value = 0;
    }

    private void OnRemoveItemClick(object? sender, EventArgs e)
    {
        if (_itemsGrid.CurrentRow?.DataBoundItem is not ReceiptItemRow row)
        {
            FormHelper.ShowWarning("Выберите позицию в таблице.");
            return;
        }

        _items.Remove(row);
    }

    private void OnSaveClick(object? sender, EventArgs e)
    {
        if (_items.Count == 0)
        {
            FormHelper.ShowWarning("Добавьте хотя бы одну позицию поступления.");
            return;
        }

        var items = _items.Select(row => ReceiptItem.Create(0, row.ShoeId, row.Size, row.Quantity));

        Result = Receipt.Create(_receiptDatePicker.Value.Date, items);
        DialogResult = DialogResult.OK;
    }
}
