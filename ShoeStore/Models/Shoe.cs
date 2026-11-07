using ShoeStore.Enums;

namespace ShoeStore.Models;

public class Shoe
{
    private Shoe(int id, string name, int manufacturerId, decimal price, ShoeCategory category, Season seasons)
    {
        Id = id;
        Name = name;
        ManufacturerId = manufacturerId;
        Price = price;
        Category = category;
        Seasons = seasons;
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    public int ManufacturerId { get; private set; }

    public decimal Price { get; private set; }

    public ShoeCategory Category { get; private set; }

    public Season Seasons { get; private set; }

    public static Shoe Create(string name, int manufacturerId, decimal price, ShoeCategory category, Season seasons)
    {
        return Create(0, name, manufacturerId, price, category, seasons);
    }

    public static Shoe Create(int id, string name, int manufacturerId, decimal price, ShoeCategory category, Season seasons)
    {
        if (!Enum.IsDefined(category))
        {
            throw new ArgumentOutOfRangeException(nameof(category), category, "Недопустимая категория обуви.");
        }

        if (seasons == Season.None)
        {
            throw new ArgumentException("Необходимо указать хотя бы один сезон.", nameof(seasons));
        }

        return new Shoe(
            Guard.NotNegative(id, nameof(id)),
            Guard.NotEmpty(name, nameof(name)),
            Guard.Positive(manufacturerId, nameof(manufacturerId)),
            Guard.Positive(price, nameof(price)),
            category,
            seasons);
    }
}
