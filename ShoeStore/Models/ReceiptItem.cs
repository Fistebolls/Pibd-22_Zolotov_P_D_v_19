using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStore.Models;

public class ReceiptItem
{
    protected ReceiptItem()
    {
    }

    [Column("receipt_id")]
    [ForeignKey(nameof(Receipt))]
    public int ReceiptId { get; private set; }

    [Column("shoe_id")]
    [ForeignKey(nameof(Shoe))]
    public int ShoeId { get; private set; }

    [Column("size", TypeName = "numeric(3,1)")]
    public decimal Size { get; private set; }

    [Column("quantity")]
    public int Quantity { get; private set; }

    public Receipt? Receipt { get; private set; }

    public Shoe? Shoe { get; private set; }

    public static ReceiptItem Create(int receiptId, int shoeId, decimal size, int quantity)
    {
        return new ReceiptItem
        {
            ReceiptId = Guard.NotNegative(receiptId, nameof(receiptId)),
            ShoeId = Guard.Positive(shoeId, nameof(shoeId)),
            Size = Guard.InRange(size, ShoeSizeRange.Min, ShoeSizeRange.Max, nameof(size)),
            Quantity = Guard.Positive(quantity, nameof(quantity))
        };
    }
}
