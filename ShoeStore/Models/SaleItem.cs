using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStore.Models;

public class SaleItem
{
    protected SaleItem()
    {
    }

    [Column("sale_id")]
    [ForeignKey(nameof(Sale))]
    public int SaleId { get; private set; }

    [Column("shoe_id")]
    [ForeignKey(nameof(Shoe))]
    public int ShoeId { get; private set; }

    [Column("size", TypeName = "numeric(3,1)")]
    public decimal Size { get; private set; }

    [Column("quantity")]
    public int Quantity { get; private set; }

    [Column("sale_price", TypeName = "numeric(10,2)")]
    public decimal SalePrice { get; private set; }

    [NotMapped]
    public decimal Amount => Quantity * SalePrice;

    public Sale? Sale { get; private set; }

    public Shoe? Shoe { get; private set; }

    public static SaleItem Create(int saleId, int shoeId, decimal size, int quantity, decimal salePrice)
    {
        return new SaleItem
        {
            SaleId = Guard.NotNegative(saleId, nameof(saleId)),
            ShoeId = Guard.Positive(shoeId, nameof(shoeId)),
            Size = Guard.InRange(size, ShoeSizeRange.Min, ShoeSizeRange.Max, nameof(size)),
            Quantity = Guard.Positive(quantity, nameof(quantity)),
            SalePrice = Guard.Positive(salePrice, nameof(salePrice))
        };
    }
}
