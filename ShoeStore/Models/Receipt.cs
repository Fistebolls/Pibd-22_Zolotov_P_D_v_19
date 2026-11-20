using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStore.Models;

public class Receipt
{
    private readonly List<ReceiptItem> _items = new();

    protected Receipt()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; private set; }

    [Column("receipt_date", TypeName = "date")]
    public DateTime ReceiptDate { get; private set; }

    public IReadOnlyList<ReceiptItem> Items => _items;

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

        var receipt = new Receipt
        {
            Id = Guard.NotNegative(id, nameof(id)),
            ReceiptDate = receiptDate.Date
        };
        receipt._items.AddRange(itemList);

        return receipt;
    }
}
