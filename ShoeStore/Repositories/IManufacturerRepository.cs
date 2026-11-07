using ShoeStore.Models;

namespace ShoeStore.Repositories;

public interface IManufacturerRepository
{
    IReadOnlyList<Manufacturer> GetAll();

    Manufacturer? GetById(int id);

    Manufacturer Create(Manufacturer manufacturer);

    void Update(Manufacturer manufacturer);

    void Delete(int id);
}
