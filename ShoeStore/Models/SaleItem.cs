namespace ShoeStore.Models;

public class SaleItem
{
    private SaleItem(int saleId, int shoeId, decimal size, int quantity, decimal salePrice)
    {
        SaleId = saleId;
        ShoeId = shoeId;
        Size = size;
        Quantity = quantity;
        SalePrice = salePrice;
    }

    public int SaleId { get; private set; }

    public int ShoeId { get; private set; }

    public decimal Size { get; private set; }

    public int Quantity { get; private set; }

    public decimal SalePrice { get; private set; }

    public decimal Amount => Quantity * SalePrice;

    public static SaleItem Create(int saleId, int shoeId, decimal size, int quantity, decimal salePrice)
    {
        return new SaleItem(
            Guard.NotNegative(saleId, nameof(saleId)),
            Guard.Positive(shoeId, nameof(shoeId)),
            Guard.InRange(size, ShoeSizeRange.Min, ShoeSizeRange.Max, nameof(size)),
            Guard.Positive(quantity, nameof(quantity)),
            Guard.Positive(salePrice, nameof(salePrice)));
    }
}
