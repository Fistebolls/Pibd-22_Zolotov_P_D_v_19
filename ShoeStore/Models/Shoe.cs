using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ShoeStore.Enums;

namespace ShoeStore.Models;

public class Shoe
{
    protected Shoe()
    {
    }

    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Column("id")]
    public int Id { get; private set; }

    [Required]
    [MaxLength(100)]
    [Column("name")]
    public string Name { get; private set; } = string.Empty;

    [Column("manufacturer_id")]
    [ForeignKey(nameof(Manufacturer))]
    public int ManufacturerId { get; private set; }

    [Column("price", TypeName = "numeric(10,2)")]
    public decimal Price { get; private set; }

    [Column("category")]
    public ShoeCategory Category { get; private set; }

    [Column("seasons")]
    public Season Seasons { get; private set; }

    public Manufacturer? Manufacturer { get; private set; }

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

        return new Shoe
        {
            Id = Guard.NotNegative(id, nameof(id)),
            Name = Guard.NotEmpty(name, nameof(name)),
            ManufacturerId = Guard.Positive(manufacturerId, nameof(manufacturerId)),
            Price = Guard.Positive(price, nameof(price)),
            Category = category,
            Seasons = seasons
        };
    }
}
