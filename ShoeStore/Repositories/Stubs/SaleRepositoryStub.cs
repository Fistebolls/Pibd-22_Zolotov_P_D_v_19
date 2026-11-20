using ShoeStore.Models;

namespace ShoeStore.Repositories.Stubs;

public class SaleRepositoryStub : ISaleRepository
{
    private readonly List<Sale> _sales = new();
    private int _nextId = 1;

    public IReadOnlyList<Sale> GetAll()
    {
        return _sales.ToList();
    }

    public Sale? GetById(int id)
    {
        return _sales.FirstOrDefault(sale => sale.Id == id);
    }

    public Sale Create(Sale sale)
    {
        ArgumentNullException.ThrowIfNull(sale);

        var id = _nextId++;
        var items = sale.Items.Select(item => SaleItem.Create(id, item.ShoeId, item.Size, item.Quantity, item.SalePrice));
        var created = Sale.Create(id, sale.SaleDate, items);
        _sales.Add(created);

        return created;
    }
}
