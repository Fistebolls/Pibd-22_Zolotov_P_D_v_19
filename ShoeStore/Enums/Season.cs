using System.ComponentModel;

namespace ShoeStore.Enums;

[Flags]
public enum Season
{
    [Description("Не указан")]
    None = 0,

    [Description("Весна")]
    Spring = 1 << 0,

    [Description("Лето")]
    Summer = 1 << 1,

    [Description("Осень")]
    Autumn = 1 << 2,

    [Description("Зима")]
    Winter = 1 << 3
}
