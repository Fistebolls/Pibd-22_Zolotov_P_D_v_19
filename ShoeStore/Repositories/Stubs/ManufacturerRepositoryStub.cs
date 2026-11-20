using ShoeStore.Models;

namespace ShoeStore.Repositories.Stubs;

public class ManufacturerRepositoryStub : IManufacturerRepository
{
    private readonly List<Manufacturer> _manufacturers = new();
    private int _nextId = 1;

    public IReadOnlyList<Manufacturer> GetAll()
    {
        return _manufacturers.ToList();
    }

    public Manufacturer? GetById(int id)
    {
        return _manufacturers.FirstOrDefault(manufacturer => manufacturer.Id == id);
    }

    public Manufacturer Create(Manufacturer manufacturer)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        var created = Manufacturer.Create(_nextId++, manufacturer.Name, manufacturer.Country);
        _manufacturers.Add(created);

        return created;
    }

    public void Update(Manufacturer manufacturer)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);

        var index = _manufacturers.FindIndex(item => item.Id == manufacturer.Id);

        if (index < 0)
        {
            throw new KeyNotFoundException($"Фабрика с номером {manufacturer.Id} не найдена.");
        }

        _manufacturers[index] = manufacturer;
    }

    public void Delete(int id)
    {
        _manufacturers.RemoveAll(manufacturer => manufacturer.Id == id);
    }
}
