using Autofac;
using Serilog;
using ShoeStore.Forms;
using ShoeStore.Infrastructure;

namespace ShoeStore;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(AppConfiguration.Instance)
            .CreateLogger();

        try
        {
            ApplicationConfiguration.Initialize();
            Application.SetUnhandledExceptionMode(UnhandledExceptionMode.CatchException);
            Application.ThreadException += OnThreadException;

            Log.Information("Приложение запущено");

            using var container = ContainerConfigurator.Configure();
            Application.Run(container.Resolve<MainForm>());
        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "Приложение аварийно завершено");
            throw;
        }
        finally
        {
            Log.Information("Приложение закрыто");
            Log.CloseAndFlush();
        }
    }

    private static void OnThreadException(object sender, ThreadExceptionEventArgs e)
    {
        Log.Error(e.Exception, "Необработанная ошибка");
        MessageBox.Show(
            DatabaseErrorFormatter.GetUserMessage(e.Exception),
            "Ошибка",
            MessageBoxButtons.OK,
            MessageBoxIcon.Error);
    }
}
