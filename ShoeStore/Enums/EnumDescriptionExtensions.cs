using System.ComponentModel;
using System.Reflection;

namespace ShoeStore.Enums;

public static class EnumDescriptionExtensions
{
    public static string GetDescription(this Enum value)
    {
        var enumType = value.GetType();
        var isFlags = enumType.IsDefined(typeof(FlagsAttribute), false);

        if (!isFlags || Convert.ToInt32(value) == 0)
        {
            return GetMemberDescription(enumType, value);
        }

        var descriptions = Enum.GetValues(enumType)
            .Cast<Enum>()
            .Where(flag => Convert.ToInt32(flag) != 0 && value.HasFlag(flag))
            .Select(flag => GetMemberDescription(enumType, flag));

        return string.Join(", ", descriptions);
    }

    private static string GetMemberDescription(Type enumType, Enum value)
    {
        var name = value.ToString();
        var attribute = enumType.GetField(name)?.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? name;
    }
}
