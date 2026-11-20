using Microsoft.EntityFrameworkCore;
using Serilog;
using ShoeStore.Data;
using ShoeStore.Models;

namespace ShoeStore.Repositories;

public class ManufacturerRepository : RepositoryBase, IManufacturerRepository
{
    public ManufacturerRepository(DataContext context, ILogger logger)
        : base(context, logger, typeof(ManufacturerRepository))
    {
    }

    public IReadOnlyList<Manufacturer> GetAll()
    {
        var manufacturers = Context.Manufacturers.AsNoTracking().OrderBy(manufacturer => manufacturer.Id).ToList();
        Logger.Information("Получен список фабрик, записей: {Count}", manufacturers.Count);

        return manufacturers;
    }

    public Manufacturer? GetById(int id)
    {
        var manufacturer = Context.Manufacturers.AsNoTracking().FirstOrDefault(item => item.Id == id);
        Logger.Information("Запрос фабрики {Id}, найдена: {Found}", id, manufacturer is not null);

        return manufacturer;
    }

    public Manufacturer Create(Manufacturer manufacturer)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);
        Logger.Information("Создание фабрики: название {Name}, страна {Country}", manufacturer.Name, manufacturer.Country);

        ExecuteWrite("создание фабрики", () =>
        {
            Context.Manufacturers.Add(manufacturer);
            Context.SaveChanges();
        });

        Logger.Information("Фабрика создана, номер {Id}", manufacturer.Id);

        return manufacturer;
    }

    public void Update(Manufacturer manufacturer)
    {
        ArgumentNullException.ThrowIfNull(manufacturer);
        Logger.Information(
            "Изменение фабрики {Id}: название {Name}, страна {Country}",
            manufacturer.Id,
            manufacturer.Name,
            manufacturer.Country);

        ExecuteWrite("изменение фабрики", () =>
        {
            Context.Manufacturers.Update(manufacturer);
            Context.SaveChanges();
        });

        Logger.Information("Фабрика {Id} изменена", manufacturer.Id);
    }

    public void Delete(int id)
    {
        Logger.Information("Удаление фабрики {Id}", id);

        ExecuteWrite("удаление фабрики", () =>
        {
            var manufacturer = Context.Manufacturers.Find(id);

            if (manufacturer is null)
            {
                Logger.Warning("Фабрика {Id} не найдена, удалять нечего", id);
                return;
            }

            Context.Manufacturers.Remove(manufacturer);
            Context.SaveChanges();
            Logger.Information("Фабрика {Id} удалена", id);
        });
    }
}
