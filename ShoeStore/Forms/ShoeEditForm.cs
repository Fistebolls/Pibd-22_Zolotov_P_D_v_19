using ShoeStore.Enums;
using ShoeStore.Models;
using ShoeStore.Repositories;

namespace ShoeStore.Forms;

public class ShoeEditForm : Form
{
    private readonly TextBox _nameTextBox = new() { MaxLength = 100 };

    private readonly ComboBox _manufacturerComboBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    private readonly NumericUpDown _priceNumeric = new()
    {
        DecimalPlaces = 2,
        Increment = 100m,
        Maximum = 10_000_000m,
        ThousandsSeparator = true
    };

    private readonly ComboBox _categoryComboBox = new() { DropDownStyle = ComboBoxStyle.DropDownList };

    private readonly Dictionary<Season, CheckBox> _seasonCheckBoxes = Enum.GetValues<Season>()
        .Where(season => season != Season.None)
        .ToDictionary(season => season, season => new CheckBox { Text = season.GetDescription(), AutoSize = true });

    private int _editedId;

    public ShoeEditForm(IManufacturerRepository manufacturerRepository)
    {
        _manufacturerComboBox.DisplayMember = nameof(Manufacturer.Name);
        _manufacturerComboBox.ValueMember = nameof(Manufacturer.Id);
        _manufacturerComboBox.DataSource = manufacturerRepository.GetAll().ToList();

        _categoryComboBox.DisplayMember = nameof(KeyValuePair<ShoeCategory, string>.Value);
        _categoryComboBox.ValueMember = nameof(KeyValuePair<ShoeCategory, string>.Key);
        _categoryComboBox.DataSource = Enum.GetValues<ShoeCategory>()
            .Select(category => new KeyValuePair<ShoeCategory, string>(category, category.GetDescription()))
            .ToList();

        var seasonsPanel = new FlowLayoutPanel { AutoSize = true };
        seasonsPanel.Controls.AddRange(_seasonCheckBoxes.Values.ToArray<Control>());

        var saveButton = FormHelper.CreateButton("Сохранить");
        saveButton.Click += OnSaveClick;

        var cancelButton = FormHelper.CreateButton("Отмена");
        cancelButton.DialogResult = DialogResult.Cancel;

        FormHelper.ConfigureEditDialog(
            this,
            "Новая модель обуви",
            saveButton,
            cancelButton,
            ("Наименование:", _nameTextBox),
            ("Фабрика-изготовитель:", _manufacturerComboBox),
            ("Цена:", _priceNumeric),
            ("Категория:", _categoryComboBox),
            ("Сезон:", seasonsPanel));
    }

    public Shoe? Result { get; private set; }

    public void LoadForEdit(Shoe shoe)
    {
        ArgumentNullException.ThrowIfNull(shoe);

        _editedId = shoe.Id;
        _nameTextBox.Text = shoe.Name;
        _manufacturerComboBox.SelectedValue = shoe.ManufacturerId;
        _priceNumeric.Value = shoe.Price;
        _categoryComboBox.SelectedValue = shoe.Category;

        foreach (var (season, checkBox) in _seasonCheckBoxes)
        {
            checkBox.Checked = shoe.Seasons.HasFlag(season);
        }

        Text = "Редактирование модели обуви";
    }

    private void OnSaveClick(object? sender, EventArgs e)
    {
        var name = _nameTextBox.Text.Trim();

        if (name.Length == 0)
        {
            FormHelper.ShowWarning("Введите наименование обуви.");
            _nameTextBox.Focus();
            return;
        }

        if (_manufacturerComboBox.SelectedValue is not int manufacturerId)
        {
            FormHelper.ShowWarning("Выберите фабрику-изготовителя. Если список пуст, сначала добавьте фабрику в справочнике.");
            _manufacturerComboBox.Focus();
            return;
        }

        if (_priceNumeric.Value <= 0)
        {
            FormHelper.ShowWarning("Укажите цену больше нуля.");
            _priceNumeric.Focus();
            return;
        }

        if (_categoryComboBox.SelectedValue is not ShoeCategory category)
        {
            FormHelper.ShowWarning("Выберите категорию обуви.");
            _categoryComboBox.Focus();
            return;
        }

        var seasons = _seasonCheckBoxes
            .Where(pair => pair.Value.Checked)
            .Aggregate(Season.None, (current, pair) => current | pair.Key);

        if (seasons == Season.None)
        {
            FormHelper.ShowWarning("Отметьте хотя бы один сезон.");
            return;
        }

        Result = Shoe.Create(_editedId, name, manufacturerId, _priceNumeric.Value, category, seasons);
        DialogResult = DialogResult.OK;
    }
}
