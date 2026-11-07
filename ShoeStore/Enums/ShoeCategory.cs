using System.ComponentModel;

namespace ShoeStore.Enums;

public enum ShoeCategory
{
    [Description("Мужская")]
    Men = 1,

    [Description("Женская")]
    Women = 2,

    [Description("Детская")]
    Children = 3,

    [Description("Унисекс")]
    Unisex = 4
}
