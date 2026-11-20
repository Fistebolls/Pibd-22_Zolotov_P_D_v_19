using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStore.Models;

public class Sale
{
    private readonly List<SaleItem> _items = new();

    protected Sale()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; private set; }

    [Column("sale_date", TypeName = "date")]
    public DateTime SaleDate { get; private set; }

    public IReadOnlyList<SaleItem> Items => _items;

    public static Sale Create(DateTime saleDate, IEnumerable<SaleItem> items)
    {
        return Create(0, saleDate, items);
    }

    public static Sale Create(int id, DateTime saleDate, IEnumerable<SaleItem> items)
    {
        ArgumentNullException.ThrowIfNull(items);

        if (saleDate == default)
        {
            throw new ArgumentException("Дата продажи не указана.", nameof(saleDate));
        }

        var itemList = items.ToList();

        if (itemList.Count == 0 || itemList.Any(item => item is null))
        {
            throw new ArgumentException("Продажа должна содержать хотя бы одну позицию.", nameof(items));
        }

        var sale = new Sale
        {
            Id = Guard.NotNegative(id, nameof(id)),
            SaleDate = saleDate.Date
        };
        sale._items.AddRange(itemList);

        return sale;
    }
}
