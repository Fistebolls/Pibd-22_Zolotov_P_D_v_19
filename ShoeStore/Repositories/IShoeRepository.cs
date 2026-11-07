using ShoeStore.Models;

namespace ShoeStore.Repositories;

public interface IShoeRepository
{
    IReadOnlyList<Shoe> GetAll();

    IReadOnlyList<Shoe> GetByManufacturerId(int manufacturerId);

    Shoe? GetById(int id);

    Shoe Create(Shoe shoe);

    void Update(Shoe shoe);

    void Delete(int id);
}
