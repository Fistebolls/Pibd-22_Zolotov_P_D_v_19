using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ShoeStore.Models;

public class Manufacturer
{
    protected Manufacturer()
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

    [Required]
    [MaxLength(60)]
    [Column("country")]
    public string Country { get; private set; } = string.Empty;

    public static Manufacturer Create(string name, string country)
    {
        return Create(0, name, country);
    }

    public static Manufacturer Create(int id, string name, string country)
    {
        return new Manufacturer
        {
            Id = Guard.NotNegative(id, nameof(id)),
            Name = Guard.NotEmpty(name, nameof(name)),
            Country = Guard.NotEmpty(country, nameof(country))
        };
    }
}
