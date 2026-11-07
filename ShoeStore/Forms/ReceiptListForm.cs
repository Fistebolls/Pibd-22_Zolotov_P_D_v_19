using ShoeStore.Models;
using ShoeStore.Repositories;

namespace ShoeStore.Forms;

public class ReceiptListForm : Form
{
    private readonly IReceiptRepository _receiptRepository;
    private readonly IShoeRepository _shoeRepository;
    private readonly Func<ReceiptCreateForm> _createFormFactory;
    private readonly DataGridView _receiptsGrid = FormHelper.CreateReadOnlyGrid();

    public ReceiptListForm(
        IReceiptRepository receiptRepository,
        IShoeRepository shoeRepository,
        Func<ReceiptCreateForm> createFormFactory)
    {
        _receiptRepository = receiptRepository;
        _shoeRepository = shoeRepository;
        _createFormFactory = createFormFactory;

        InitializeLayout();
        RefreshGrid();
    }

    private sealed record ReceiptRow(
        int ReceiptId,
        DateTime ReceiptDate,
        string ShoeName,
        decimal Size,
        int Quantity);

    private void InitializeLayout()
    {
        Text = "Поступление товара";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(760, 440);
        MinimumSize = new Size(600, 300);

        FormHelper.AddColumn(_receiptsGrid, nameof(ReceiptRow.ReceiptId), "Номер поступления");
        FormHelper.AddColumn(_receiptsGrid, nameof(ReceiptRow.ReceiptDate), "Дата поступления", "dd.MM.yyyy");
        FormHelper.AddColumn(_receiptsGrid, nameof(ReceiptRow.ShoeName), "Обувь");
        FormHelper.AddColumn(_receiptsGrid, nameof(ReceiptRow.Size), "Размер", "0.0");
        FormHelper.AddColumn(_receiptsGrid, nameof(ReceiptRow.Quantity), "Количество пар");

        var addButton = FormHelper.CreateButton("Добавить");
        addButton.Click += OnAddClick;

        Controls.Add(FormHelper.CreateListLayout(_receiptsGrid, addButton));
    }

    private void RefreshGrid()
    {
        var shoeNames = _shoeRepository.GetAll().ToDictionary(shoe => shoe.Id, shoe => shoe.Name);

        _receiptsGrid.DataSource = _receiptRepository.GetAll()
            .SelectMany(receipt => receipt.Items.Select(item => new ReceiptRow(
                receipt.Id,
                receipt.ReceiptDate,
                shoeNames.GetValueOrDefault(item.ShoeId, "—"),
                item.Size,
                item.Quantity)))
            .OrderByDescending(row => row.ReceiptDate)
            .ThenByDescending(row => row.ReceiptId)
            .ToList();
    }

    private void OnAddClick(object? sender, EventArgs e)
    {
        using var form = _createFormFactory();

        if (form.ShowDialog(this) != DialogResult.OK || form.Result is null)
        {
            return;
        }

        _receiptRepository.Create(form.Result);
        RefreshGrid();
    }
}
