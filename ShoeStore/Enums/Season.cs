using System.ComponentModel;

namespace ShoeStore.Enums;

[Flags]
public enum Season
{
    [Description("Не указан")]
    None = 0,

    [Description("Весна")]
    Spring = 1,

    [Description("Лето")]
    Summer = 2,

    [Description("Осень")]
    Autumn = 4,

    [Description("Зима")]
    Winter = 8
}
