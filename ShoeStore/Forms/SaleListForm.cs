using ShoeStore.Models;
using ShoeStore.Repositories;

namespace ShoeStore.Forms;

public class SaleListForm : Form
{
    private readonly ISaleRepository _saleRepository;
    private readonly IShoeRepository _shoeRepository;
    private readonly Func<SaleCreateForm> _createFormFactory;
    private readonly DataGridView _salesGrid = FormHelper.CreateReadOnlyGrid();

    public SaleListForm(
        ISaleRepository saleRepository,
        IShoeRepository shoeRepository,
        Func<SaleCreateForm> createFormFactory)
    {
        _saleRepository = saleRepository;
        _shoeRepository = shoeRepository;
        _createFormFactory = createFormFactory;

        InitializeLayout();
        RefreshGrid();
    }

    private sealed record SaleRow(
        int SaleId,
        DateTime SaleDate,
        string ShoeName,
        decimal Size,
        int Quantity,
        decimal SalePrice,
        decimal Amount);

    private void InitializeLayout()
    {
        Text = "Продажа товара";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(860, 440);
        MinimumSize = new Size(700, 300);

        FormHelper.AddColumn(_salesGrid, nameof(SaleRow.SaleId), "Номер продажи");
        FormHelper.AddColumn(_salesGrid, nameof(SaleRow.SaleDate), "Дата продажи", "dd.MM.yyyy");
        FormHelper.AddColumn(_salesGrid, nameof(SaleRow.ShoeName), "Обувь");
        FormHelper.AddColumn(_salesGrid, nameof(SaleRow.Size), "Размер", "0.0");
        FormHelper.AddColumn(_salesGrid, nameof(SaleRow.Quantity), "Количество пар");
        FormHelper.AddColumn(_salesGrid, nameof(SaleRow.SalePrice), "Цена", "N2");
        FormHelper.AddColumn(_salesGrid, nameof(SaleRow.Amount), "Сумма", "N2");

        var addButton = FormHelper.CreateButton("Добавить");
        addButton.Click += OnAddClick;

        Controls.Add(FormHelper.CreateListLayout(_salesGrid, addButton));
    }

    private void RefreshGrid()
    {
        var shoeNames = _shoeRepository.GetAll().ToDictionary(shoe => shoe.Id, shoe => shoe.Name);

        _salesGrid.DataSource = _saleRepository.GetAll()
            .SelectMany(sale => sale.Items.Select(item => new SaleRow(
                sale.Id,
                sale.SaleDate,
                shoeNames.GetValueOrDefault(item.ShoeId, "—"),
                item.Size,
                item.Quantity,
                item.SalePrice,
                item.Amount)))
            .OrderByDescending(row => row.SaleDate)
            .ThenByDescending(row => row.SaleId)
            .ToList();
    }

    private void OnAddClick(object? sender, EventArgs e)
    {
        using var form = _createFormFactory();

        if (form.ShowDialog(this) != DialogResult.OK || form.Result is null)
        {
            return;
        }

        _saleRepository.Create(form.Result);
        RefreshGrid();
    }
}
