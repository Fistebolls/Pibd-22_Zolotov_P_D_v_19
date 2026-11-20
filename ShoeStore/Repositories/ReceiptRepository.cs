using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Repositories;

public class ReceiptRepository : RepositoryBase, IReceiptRepository
{
    public ReceiptRepository(DataContext context, ILogger logger)
        : base(context, logger, typeof(ReceiptRepository))
    {
    }

    public IReadOnlyList<Receipt> GetAll()
    {
        var receipts = Context.Receipts
            .AsNoTracking()
            .Include(receipt => receipt.Items)
            .OrderBy(receipt => receipt.Id)
            .ToList();
        Logger.Information("Получен список поступлений, записей: {Count}", receipts.Count);

        return receipts;
    }

    public Receipt? GetById(int id)
    {
        var receipt = Context.Receipts
            .AsNoTracking()
            .Include(item => item.Items)
            .FirstOrDefault(item => item.Id == id);
        Logger.Information("Запрос поступления {Id}, найдено: {Found}", id, receipt is not null);

        return receipt;
    }

    public Receipt Create(Receipt receipt)
    {
        ArgumentNullException.ThrowIfNull(receipt);
        Logger.Information("Создание поступления от {ReceiptDate:dd.MM.yyyy}, позиций: {Count}", receipt.ReceiptDate, receipt.Items.Count);

        foreach (var item in receipt.Items)
        {
            Logger.Information("Позиция поступления: обувь {ShoeId}, размер {Size}, количество {Quantity}", item.ShoeId, item.Size, item.Quantity);
        }

        ExecuteWrite("создание поступления", () =>
        {
            using var transaction = Context.Database.BeginTransaction();

            Context.Receipts.Add(receipt);
            Context.SaveChanges();
            transaction.Commit();
        });

        Logger.Information("Поступление создано, номер {Id}", receipt.Id);

        return receipt;
    }
}
