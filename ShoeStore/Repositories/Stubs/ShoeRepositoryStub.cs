using ShoeStore.Models;

namespace ShoeStore.Repositories.Stubs;

public class ShoeRepositoryStub : IShoeRepository
{
    private readonly List<Shoe> _shoes = new();
    private int _nextId = 1;

    public IReadOnlyList<Shoe> GetAll()
    {
        return _shoes.ToList();
    }

    public IReadOnlyList<Shoe> GetByManufacturerId(int manufacturerId)
    {
        return _shoes.Where(shoe => shoe.ManufacturerId == manufacturerId).ToList();
    }

    public Shoe? GetById(int id)
    {
        return _shoes.FirstOrDefault(shoe => shoe.Id == id);
    }

    public Shoe Create(Shoe shoe)
    {
        ArgumentNullException.ThrowIfNull(shoe);

        var created = Shoe.Create(_nextId++, shoe.Name, shoe.ManufacturerId, shoe.Price, shoe.Category, shoe.Seasons);
        _shoes.Add(created);

        return created;
    }

    public void Update(Shoe shoe)
    {
        ArgumentNullException.ThrowIfNull(shoe);

        var index = _shoes.FindIndex(item => item.Id == shoe.Id);

        if (index < 0)
        {
            throw new KeyNotFoundException($"Обувь с номером {shoe.Id} не найдена.");
        }

        _shoes[index] = shoe;
    }

    public void Delete(int id)
    {
        _shoes.RemoveAll(shoe => shoe.Id == id);
    }
}
