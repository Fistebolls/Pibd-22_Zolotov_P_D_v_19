using ShoeStore.Models;

namespace ShoeStore.Repositories;

public interface ISaleRepository
{
    IReadOnlyList<Sale> GetAll();

    Sale? GetById(int id);

    Sale Create(Sale sale);
}
