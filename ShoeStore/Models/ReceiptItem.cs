namespace ShoeStore.Models;

public class ReceiptItem
{
    private ReceiptItem(int receiptId, int shoeId, decimal size, int quantity)
    {
        ReceiptId = receiptId;
        ShoeId = shoeId;
        Size = size;
        Quantity = quantity;
    }

    public int ReceiptId { get; private set; }

    public int ShoeId { get; private set; }

    public decimal Size { get; private set; }

    public int Quantity { get; private set; }

    public static ReceiptItem Create(int receiptId, int shoeId, decimal size, int quantity)
    {
        return new ReceiptItem(
            Guard.NotNegative(receiptId, nameof(receiptId)),
            Guard.Positive(shoeId, nameof(shoeId)),
            Guard.InRange(size, ShoeSizeRange.Min, ShoeSizeRange.Max, nameof(size)),
            Guard.Positive(quantity, nameof(quantity)));
    }
}
