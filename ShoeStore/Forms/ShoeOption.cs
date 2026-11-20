using ShoeStore.Models;

namespace ShoeStore.Forms;

internal sealed record ShoeOption(int Id, string Title, decimal Price)
{
    public static ShoeOption From(Shoe shoe)
    {
        return new ShoeOption(shoe.Id, $"№{shoe.Id} {shoe.Name}", shoe.Price);
    }
}
