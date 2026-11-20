using ShoeStore.Models;
using ShoeStore.Repositories;

namespace ShoeStore.Forms;

public class ManufacturerListForm : Form
{
    private readonly IManufacturerRepository _manufacturerRepository;
    private readonly IShoeRepository _shoeRepository;
    private readonly Func<ManufacturerEditForm> _editFormFactory;
    private readonly DataGridView _manufacturersGrid = FormHelper.CreateReadOnlyGrid();

    public ManufacturerListForm(
        IManufacturerRepository manufacturerRepository,
        IShoeRepository shoeRepository,
        Func<ManufacturerEditForm> editFormFactory)
    {
        _manufacturerRepository = manufacturerRepository;
        _shoeRepository = shoeRepository;
        _editFormFactory = editFormFactory;

        InitializeLayout();
        RefreshGrid();
    }

    private void InitializeLayout()
    {
        Text = "Фабрики-изготовители";
        StartPosition = FormStartPosition.CenterParent;
        ClientSize = new Size(640, 420);
        MinimumSize = new Size(480, 300);

        FormHelper.AddColumn(_manufacturersGrid, nameof(Manufacturer.Id), "Номер");
        FormHelper.AddColumn(_manufacturersGrid, nameof(Manufacturer.Name), "Название");
        FormHelper.AddColumn(_manufacturersGrid, nameof(Manufacturer.Country), "Страна");

        var addButton = FormHelper.CreateButton("Добавить");
        addButton.Click += OnAddClick;

        var editButton = FormHelper.CreateButton("Изменить");
        editButton.Click += OnEditClick;

        var deleteButton = FormHelper.CreateButton("Удалить");
        deleteButton.Click += OnDeleteClick;

        Controls.Add(FormHelper.CreateListLayout(_manufacturersGrid, addButton, editButton, deleteButton));
    }

    private void RefreshGrid()
    {
        _manufacturersGrid.DataSource = _manufacturerRepository.GetAll().ToList();
    }

    private Manufacturer? GetSelectedManufacturer()
    {
        return _manufacturersGrid.CurrentRow?.DataBoundItem as Manufacturer;
    }

    private void OnAddClick(object? sender, EventArgs e)
    {
        using var form = _editFormFactory();

        if (form.ShowDialog(this) != DialogResult.OK || form.Result is null)
        {
            return;
        }

        _manufacturerRepository.Create(form.Result);
        RefreshGrid();
    }

    private void OnEditClick(object? sender, EventArgs e)
    {
        var manufacturer = GetSelectedManufacturer();

        if (manufacturer is null)
        {
            FormHelper.ShowWarning("Выберите фабрику в списке.");
            return;
        }

        using var form = _editFormFactory();
        form.LoadForEdit(manufacturer);

        if (form.ShowDialog(this) != DialogResult.OK || form.Result is null)
        {
            return;
        }

        _manufacturerRepository.Update(form.Result);
        RefreshGrid();
    }

    private void OnDeleteClick(object? sender, EventArgs e)
    {
        var manufacturer = GetSelectedManufacturer();

        if (manufacturer is null)
        {
            FormHelper.ShowWarning("Выберите фабрику в списке.");
            return;
        }

        if (_shoeRepository.GetByManufacturerId(manufacturer.Id).Count > 0)
        {
            FormHelper.ShowWarning("Нельзя удалить фабрику: в ассортименте есть ее модели обуви.");
            return;
        }

        if (!FormHelper.Confirm($"Удалить фабрику «{manufacturer.Name}»?"))
        {
            return;
        }

        _manufacturerRepository.Delete(manufacturer.Id);
        RefreshGrid();
    }
}
