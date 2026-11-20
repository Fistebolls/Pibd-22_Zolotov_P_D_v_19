namespace ShoeStore.Models;

public class Sale
{
    private Sale(int id, DateTime saleDate, IReadOnlyList<SaleItem> items)
    {
        Id = id;
        SaleDate = saleDate;
        Items = items;
    }

    public int Id { get; private set; }

    public DateTime SaleDate { get; private set; }

    public IReadOnlyList<SaleItem> Items { get; private set; }

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

        return new Sale(Guard.NotNegative(id, nameof(id)), saleDate.Date, itemList.AsReadOnly());
    }
}
