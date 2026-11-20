namespace ShoeStore.Models;

internal static class Guard
{
    public static string NotEmpty(string? value, string paramName)
    {
        ArgumentNullException.ThrowIfNull(value, paramName);

        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Значение не может быть пустым.", paramName);
        }

        return value.Trim();
    }

    public static int NotNegative(int value, string paramName)
    {
        if (value < 0)
        {
            throw new ArgumentOutOfRangeException(paramName, value, "Значение не может быть отрицательным.");
        }

        return value;
    }

    public static int Positive(int value, string paramName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, value, "Значение должно быть больше нуля.");
        }

        return value;
    }

    public static decimal Positive(decimal value, string paramName)
    {
        if (value <= 0)
        {
            throw new ArgumentOutOfRangeException(paramName, value, "Значение должно быть больше нуля.");
        }

        return value;
    }

    public static decimal InRange(decimal value, decimal min, decimal max, string paramName)
    {
        if (value < min || value > max)
        {
            throw new ArgumentOutOfRangeException(paramName, value, $"Значение должно быть в диапазоне от {min} до {max}.");
        }

        return value;
    }
}
