using ShoeStore.Models;

namespace ShoeStore.Repositories.Stubs;

public class ReceiptRepositoryStub : IReceiptRepository
{
    private readonly List<Receipt> _receipts = new();
    private int _nextId = 1;

    public IReadOnlyList<Receipt> GetAll()
    {
        return _receipts.ToList();
    }

    public Receipt? GetById(int id)
    {
        return _receipts.FirstOrDefault(receipt => receipt.Id == id);
    }

    public Receipt Create(Receipt receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);

        var id = _nextId++;
        var items = receipt.Items.Select(item => ReceiptItem.Create(id, item.ShoeId, item.Size, item.Quantity));
        var created = Receipt.Create(id, receipt.ReceiptDate, items);
        _receipts.Add(created);

        return created;
    }
}
