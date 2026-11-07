using ShoeStore.Enums;
using ShoeStore.Models;
using ShoeStore.Repositories;

namespace ShoeStore.Forms;

public class ShoeListForm : Form
{
    private readonly IShoeRepository _shoeRepository;
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IReceiptRepository _receiptRepository;
    private readonly ISaleRepository _saleRepository;
    private readonly Func<ShoeEditForm> _editFormFactory;
    private readonly DataGridView _shoesGrid = FormHelper.CreateReadOnlyGrid();

    public ShoeListForm(
        IShoeRepository shoeRepository,
        IManufacturerRepository manufacturerRepository,
        IReceiptRepository receiptRepository,
        ISaleRepository saleRepository,
        Func<ShoeEditForm> editFormFactory)
    {
        _shoeRepository = shoeRepository;
        _manufacturerRepository = manufacturerRepository;
        _receiptRepository = receiptRepository;
        _saleRepository = saleRepository;
        _editFormFactory = editFormFactory;

        InitializeLayout();
        RefreshGrid();
    }

    private sealed record ShoeRow(
        int Id,
        string Name,
        string Manufacturer,
        decimal Price,
        string Category,
        string Seasons);

    private void InitializeLayout()
    {
        Text = "Обувь (ассортимент)";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(860, 460);
        MinimumSize = new Size(640, 320);

        FormHelper.AddColumn(_shoesGrid, nameof(ShoeRow.Id), "Номер по ассортименту");
        FormHelper.AddColumn(_shoesGrid, nameof(ShoeRow.Name), "Наименование");
        FormHelper.AddColumn(_shoesGrid, nameof(ShoeRow.Manufacturer), "Фабрика-изготовитель");
        FormHelper.AddColumn(_shoesGrid, nameof(ShoeRow.Price), "Цена", "N2");
        FormHelper.AddColumn(_shoesGrid, nameof(ShoeRow.Category), "Категория");
        FormHelper.AddColumn(_shoesGrid, nameof(ShoeRow.Seasons), "Сезон");

        var addButton = FormHelper.CreateButton("Добавить");
        addButton.Click += OnAddClick;

        var editButton = FormHelper.CreateButton("Изменить");
        editButton.Click += OnEditClick;

        var deleteButton = FormHelper.CreateButton("Удалить");
        deleteButton.Click += OnDeleteClick;

        Controls.Add(FormHelper.CreateListLayout(_shoesGrid, addButton, editButton, deleteButton));
    }

    private void RefreshGrid()
    {
        var manufacturerNames = _manufacturerRepository.GetAll().ToDictionary(manufacturer => manufacturer.Id, manufacturer => manufacturer.Name);

        _shoesGrid.DataSource = _shoeRepository.GetAll()
            .Select(shoe => new ShoeRow(
                shoe.Id,
                shoe.Name,
                manufacturerNames.GetValueOrDefault(shoe.ManufacturerId, "—"),
                shoe.Price,
                shoe.Category.GetDescription(),
                shoe.Seasons.GetDescription()))
            .ToList();
    }

    private Shoe? GetSelectedShoe()
    {
        return _shoesGrid.CurrentRow?.DataBoundItem is ShoeRow row ? _shoeRepository.GetById(row.Id) : null;
    }

    private bool HasMovements(int shoeId)
    {
        return _receiptRepository.GetAll().Any(receipt => receipt.Items.Any(item => item.ShoeId == shoeId))
            || _saleRepository.GetAll().Any(sale => sale.Items.Any(item => item.ShoeId == shoeId));
    }

    private void OnAddClick(object? sender, EventArgs e)
    {
        using var form = _editFormFactory();

        if (form.ShowDialog(this) != DialogResult.OK || form.Result is null)
        {
            return;
        }

        _shoeRepository.Create(form.Result);
        RefreshGrid();
    }

    private void OnEditClick(object? sender, EventArgs e)
    {
        var shoe = GetSelectedShoe();

        if (shoe is null)
        {
            FormHelper.ShowWarning("Выберите модель обуви в списке.");
            return;
        }

        using var form = _editFormFactory();
        form.LoadForEdit(shoe);

        if (form.ShowDialog(this) != DialogResult.OK || form.Result is null)
        {
            return;
        }

        _shoeRepository.Update(form.Result);
        RefreshGrid();
    }

    private void OnDeleteClick(object? sender, EventArgs e)
    {
        var shoe = GetSelectedShoe();

        if (shoe is null)
        {
            FormHelper.ShowWarning("Выберите модель обуви в списке.");
            return;
        }

        if (HasMovements(shoe.Id))
        {
            FormHelper.ShowWarning("Нельзя удалить модель: по ней есть поступления или продажи.");
            return;
        }

        if (!FormHelper.Confirm($"Удалить модель «{shoe.Name}»?"))
        {
            return;
        }

        _shoeRepository.Delete(shoe.Id);
        RefreshGrid();
    }
}
