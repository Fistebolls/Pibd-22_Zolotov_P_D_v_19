using Autofac;
using ShoeStore.Forms;
using ShoeStore.Infrastructure;

namespace ShoeStore;

internal static class Program
{
    [STAThread]
    private static void Main()
    {
        ApplicationConfiguration.Initialize();

        using var container = ContainerConfigurator.Configure();
        Application.Run(container.Resolve<MainForm>());
    }
}
