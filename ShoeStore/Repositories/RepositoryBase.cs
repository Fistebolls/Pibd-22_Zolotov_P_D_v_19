using Serilog;
using ShoeStore.Data;

namespace ShoeStore.Repositories;

public abstract class RepositoryBase
{
    protected RepositoryBase(DataContext context, ILogger logger, Type sourceType)
    {
        ArgumentNullException.ThrowIfNull(context);
        ArgumentNullException.ThrowIfNull(logger);

        Context = context;
        Logger = logger.ForContext(sourceType);
    }

    protected DataContext Context { get; }

    protected ILogger Logger { get; }

    protected void ExecuteWrite(string operation, Action action)
    {
        try
        {
            action();
        }
        catch (Exception exception)
        {
            Logger.Error(exception, "Ошибка при выполнении операции: {Operation}", operation);
            throw;
        }
        finally
        {
            Context.ChangeTracker.Clear();
        }
    }
}
