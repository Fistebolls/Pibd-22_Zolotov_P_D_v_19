namespace ShoeStore.Forms;

public class MainForm : Form
{
    private readonly Func<ManufacturerListForm> _manufacturerListFormFactory;
    private readonly Func<ShoeListForm> _shoeListFormFactory;
    private readonly Func<ReceiptListForm> _receiptListFormFactory;
    private readonly Func<SaleListForm> _saleListFormFactory;

    public MainForm(
        Func<ManufacturerListForm> manufacturerListFormFactory,
        Func<ShoeListForm> shoeListFormFactory,
        Func<ReceiptListForm> receiptListFormFactory,
        Func<SaleListForm> saleListFormFactory)
    {
        _manufacturerListFormFactory = manufacturerListFormFactory;
        _shoeListFormFactory = shoeListFormFactory;
        _receiptListFormFactory = receiptListFormFactory;
        _saleListFormFactory = saleListFormFactory;

        InitializeLayout();
    }

    private void InitializeLayout()
    {
        Text = "Учет работы обувного магазина";
        StartPosition = FormStartPosition.CenterScreen;
        ClientSize = new Size(900, 560);

        var referencesMenuItem = new ToolStripMenuItem("Справочники");
        referencesMenuItem.DropDownItems.Add("Фабрики-изготовители", null, (_, _) => ShowDialogForm(_manufacturerListFormFactory));
        referencesMenuItem.DropDownItems.Add("Обувь (ассортимент)", null, (_, _) => ShowDialogForm(_shoeListFormFactory));

        var operationsMenuItem = new ToolStripMenuItem("Операции");
        operationsMenuItem.DropDownItems.Add("Поступление товара", null, (_, _) => ShowDialogForm(_receiptListFormFactory));
        operationsMenuItem.DropDownItems.Add("Продажа товара", null, (_, _) => ShowDialogForm(_saleListFormFactory));

        var reportsMenuItem = new ToolStripMenuItem("Отчеты");

        var menuStrip = new MenuStrip();
        menuStrip.Items.Add(referencesMenuItem);
        menuStrip.Items.Add(operationsMenuItem);
        menuStrip.Items.Add(reportsMenuItem);

        MainMenuStrip = menuStrip;
        Controls.Add(menuStrip);
    }

    private void ShowDialogForm(Func<Form> formFactory)
    {
        using var form = formFactory();
        form.ShowDialog(this);
    }
}
