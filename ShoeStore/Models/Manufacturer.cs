namespace ShoeStore.Models;

public class Manufacturer
{
    private Manufacturer(int id, string name, string country)
    {
        Id = id;
        Name = name;
        Country = country;
    }

    public int Id { get; private set; }

    public string Name { get; private set; }

    public string Country { get; private set; }

    public static Manufacturer Create(string name, string country)
    {
        return Create(0, name, country);
    }

    public static Manufacturer Create(int id, string name, string country)
    {
        return new Manufacturer(
            Guard.NotNegative(id, nameof(id)),
            Guard.NotEmpty(name, nameof(name)),
            Guard.NotEmpty(country, nameof(country)));
    }
}
