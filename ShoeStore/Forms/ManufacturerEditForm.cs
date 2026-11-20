using ShoeStore.Models;

namespace ShoeStore.Forms;

public class ManufacturerEditForm : Form
{
    private readonly TextBox _nameTextBox = new() { MaxLength = 100 };
    private readonly TextBox _countryTextBox = new() { MaxLength = 60 };
    private int _editedId;

    public ManufacturerEditForm()
    {
        var saveButton = FormHelper.CreateButton("Сохранить");
        saveButton.Click += OnSaveClick;

        var cancelButton = FormHelper.CreateButton("Отмена");
        cancelButton.DialogResult = DialogResult.Cancel;

        FormHelper.ConfigureEditDialog(
            this,
            "Новая фабрика-изготовитель",
            saveButton,
            cancelButton,
            ("Название:", _nameTextBox),
            ("Страна:", _countryTextBox));
    }

    public Manufacturer? Result { get; private set; }

    public void LoadForEdit(Manufacturer manufacturer)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        _editedId = manufacturer.Id;
        _nameTextBox.Text = manufacturer.Name;
        _countryTextBox.Text = manufacturer.Country;
        Text = "Редактирование фабрики-изготовителя";
    }

    private void OnSaveClick(object? sender, EventArgs e)
    {
        var name = _nameTextBox.Text.Trim();
        var country = _countryTextBox.Text.Trim();

        if (name.Length == 0)
        {
            FormHelper.ShowWarning("Введите название фабрики.");
            _nameTextBox.Focus();
            return;
        }

        if (country.Length == 0)
        {
            FormHelper.ShowWarning("Введите страну.");
            _countryTextBox.Focus();
            return;
        }

        Result = Manufacturer.Create(_editedId, name, country);
        DialogResult = DialogResult.OK;
    }
}
