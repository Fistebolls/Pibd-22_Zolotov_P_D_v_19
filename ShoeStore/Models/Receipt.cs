namespace ShoeStore.Models;

public class Receipt
{
    private Receipt(int id, DateTime receiptDate, IReadOnlyList<ReceiptItem> items)
    {
        Id = id;
        ReceiptDate = receiptDate;
        Items = items;
    }

    public int Id { get; private set; }

    public DateTime ReceiptDate { get; private set; }

    public IReadOnlyList<ReceiptItem> Items { get; private set; }

    public static Receipt Create(DateTime receiptDate, IEnumerable<ReceiptItem> items)
    {
        return Create(0, receiptDate, items);
    }

    public static Receipt Create(int id, DateTime receiptDate, IEnumerable<ReceiptItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (receiptDate == default)
        {
            throw new ArgumentException("Дата поступления не указана.", nameof(receiptDate));
        }

        var itemList = items.ToList();

        if (itemList.Count == 0 || itemList.Any(item => item is null))
        {
            throw new ArgumentException("Поступление должно содержать хотя бы одну позицию.", nameof(items));
        }

        return new Receipt(Guard.NotNegative(id, nameof(id)), receiptDate.Date, itemList.AsReadOnly());
    }
}
