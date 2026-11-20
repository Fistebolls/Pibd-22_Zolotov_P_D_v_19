using Autofac;
using Serilog;
using ShoeStore.Data;
using ShoeStore.Forms;
using ShoeStore.Repositories;

namespace ShoeStore.Infrastructure;

public static class ContainerConfigurator
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        RegisterInfrastructure(builder);
        RegisterRepositories(builder);
        RegisterForms(builder);

        return builder.Build();
    }

    private static void RegisterInfrastructure(ContainerBuilder builder)
    {
        builder.Register(_ => Log.Logger).As<ILogger>().SingleInstance();
        builder.RegisterType<DataContext>().AsSelf().InstancePerDependency();
    }

    private static void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<ManufacturerRepository>().As<IManufacturerRepository>().InstancePerDependency();
        builder.RegisterType<ShoeRepository>().As<IShoeRepository>().InstancePerDependency();
        builder.RegisterType<ReceiptRepository>().As<IReceiptRepository>().InstancePerDependency();
        builder.RegisterType<SaleRepository>().As<ISaleRepository>().InstancePerDependency();
    }

    private static void RegisterForms(ContainerBuilder builder)
    {
        builder.RegisterType<MainForm>().AsSelf().ExternallyOwned();
        builder.RegisterType<ManufacturerListForm>().AsSelf().ExternallyOwned();
        builder.RegisterType<ManufacturerEditForm>().AsSelf().ExternallyOwned();
        builder.RegisterType<ShoeListForm>().AsSelf().ExternallyOwned();
        builder.RegisterType<ShoeEditForm>().AsSelf().ExternallyOwned();
        builder.RegisterType<ReceiptListForm>().AsSelf().ExternallyOwned();
        builder.RegisterType<ReceiptCreateForm>().AsSelf().ExternallyOwned();
        builder.RegisterType<SaleListForm>().AsSelf().ExternallyOwned();
        builder.RegisterType<SaleCreateForm>().AsSelf().ExternallyOwned();
    }
}
