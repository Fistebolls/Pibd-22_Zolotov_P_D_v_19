using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Repositories;

public class SaleRepository : RepositoryBase, ISaleRepository
{
    public SaleRepository(DataContext context, ILogger logger)
        : base(context, logger, typeof(SaleRepository))
    {
    }

    public IReadOnlyList<Sale> GetAll()
    {
        var sales = Context.Sales
            .AsNoTracking()
            .Include(sale => sale.Items)
            .OrderBy(sale => sale.Id)
            .ToList();
        Logger.Information("Получен список продаж, записей: {Count}", sales.Count);

        return sales;
    }

    public Sale? GetById(int id)
    {
        var sale = Context.Sales
            .AsNoTracking()
            .Include(item => item.Items)
            .FirstOrDefault(item => item.Id == id);
        Logger.Information("Запрос продажи {Id}, найдена: {Found}", id, sale is not null);

        return sale;
    }

    public Sale Create(Sale sale)
    {
        ArgumentNullException.ThrowIfNull(sale);
        Logger.Information("Создание продажи от {SaleDate:dd.MM.yyyy}, позиций: {Count}", sale.SaleDate, sale.Items.Count);

        foreach (var item in sale.Items)
        {
            Logger.Information(
                "Позиция продажи: обувь {ShoeId}, размер {Size}, количество {Quantity}, цена {SalePrice}",
                item.ShoeId,
                item.Size,
                item.Quantity,
                item.SalePrice);
        }

        ExecuteWrite("создание продажи", () =>
        {
            using var transaction = Context.Database.BeginTransaction();

            Context.Sales.Add(sale);
            Context.SaveChanges();
            transaction.Commit();
        });

        Logger.Information("Продажа создана, номер {Id}", sale.Id);

        return sale;
    }
}
