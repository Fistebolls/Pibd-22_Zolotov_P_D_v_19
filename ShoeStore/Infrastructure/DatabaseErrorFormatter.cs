using Npgsql;

namespace ShoeStore.Infrastructure;

public static class DatabaseErrorFormatter
{
    public static string GetUserMessage(Exception exception)
    {
        ArgumentNullException.ThrowIfNull(exception);

        var postgresException = exception as PostgresException ?? exception.InnerException as PostgresException;

        return postgresException?.SqlState switch
        {
            PostgresErrorCodes.UniqueViolation => "Запись с такими данными уже существует.",
            PostgresErrorCodes.ForeignKeyViolation => "Операция невозможна: запись связана с другими данными.",
            _ => $"Ошибка при работе с базой данных: {exception.GetBaseException().Message}"
        };
    }
}
