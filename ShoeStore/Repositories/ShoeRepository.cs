using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Repositories;

public class ShoeRepository : RepositoryBase, IShoeRepository
{
    public ShoeRepository(DataContext context, ILogger logger)
        : base(context, logger, typeof(ShoeRepository))
    {
    }

    public IReadOnlyList<Shoe> GetAll()
    {
        var shoes = Context.Shoes.AsNoTracking().OrderBy(shoe => shoe.Id).ToList();
        Logger.Information("Получен список обуви, записей: {Count}", shoes.Count);

        return shoes;
    }

    public IReadOnlyList<Shoe> GetByManufacturerId(int manufacturerId)
    {
        var shoes = Context.Shoes
            .AsNoTracking()
            .Where(shoe => shoe.ManufacturerId == manufacturerId)
            .OrderBy(shoe => shoe.Id)
            .ToList();
        Logger.Information("Модели обуви фабрики {ManufacturerId}, записей: {Count}", manufacturerId, shoes.Count);

        return shoes;
    }

    public Shoe? GetById(int id)
    {
        var shoe = Context.Shoes.AsNoTracking().FirstOrDefault(item => item.Id == id);
        Logger.Information("Запрос обуви {Id}, найдена: {Found}", id, shoe is not null);

        return shoe;
    }

    public Shoe Create(Shoe shoe)
    {
        ArgumentNullException.ThrowIfNull(shoe);
        Logger.Information(
            "Создание обуви: наименование {Name}, фабрика {ManufacturerId}, цена {Price}, категория {Category}, сезон {Seasons}",
            shoe.Name,
            shoe.ManufacturerId,
            shoe.Price,
            shoe.Category,
            shoe.Seasons);

        ExecuteWrite("создание обуви", () =>
        {
            Context.Shoes.Add(shoe);
            Context.SaveChanges();
        });

        Logger.Information("Обувь создана, номер {Id}", shoe.Id);

        return shoe;
    }

    public void Update(Shoe shoe)
    {
        ArgumentNullException.ThrowIfNull(shoe);
        Logger.Information(
            "Изменение обуви {Id}: наименование {Name}, фабрика {ManufacturerId}, цена {Price}, категория {Category}, сезон {Seasons}",
            shoe.Id,
            shoe.Name,
            shoe.ManufacturerId,
            shoe.Price,
            shoe.Category,
            shoe.Seasons);

        ExecuteWrite("изменение обуви", () =>
        {
            Context.Shoes.Update(shoe);
            Context.SaveChanges();
        });

        Logger.Information("Обувь {Id} изменена", shoe.Id);
    }

    public void Delete(int id)
    {
        Logger.Information("Удаление обуви {Id}", id);

        ExecuteWrite("удаление обуви", () =>
        {
            var shoe = Context.Shoes.Find(id);

            if (shoe is null)
            {
                Logger.Warning("Обувь {Id} не найдена, удалять нечего", id);
                return;
            }

            Context.Shoes.Remove(shoe);
            Context.SaveChanges();
            Logger.Information("Обувь {Id} удалена", id);
        });
    }
}
