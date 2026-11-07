using ShoeStore.Models;

namespace ShoeStore.Repositories;

public interface IReceiptRepository
{
    IReadOnlyList<Receipt> GetAll();

    Receipt? GetById(int id);

    Receipt Create(Receipt receipt);
}
