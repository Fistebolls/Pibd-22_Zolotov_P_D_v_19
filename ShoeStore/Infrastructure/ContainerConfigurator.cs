using Autofac;
using ShoeStore.Forms;
using ShoeStore.Repositories;
using ShoeStore.Repositories.Stubs;

namespace ShoeStore.Infrastructure;

public static class ContainerConfigurator
{
    public static IContainer Configure()
    {
        var builder = new ContainerBuilder();

        RegisterRepositories(builder);
        RegisterForms(builder);

        return builder.Build();
    }

    private static void RegisterRepositories(ContainerBuilder builder)
    {
        builder.RegisterType<ManufacturerRepositoryStub>().As<IManufacturerRepository>().SingleInstance();
        builder.RegisterType<ShoeRepositoryStub>().As<IShoeRepository>().SingleInstance();
        builder.RegisterType<ReceiptRepositoryStub>().As<IReceiptRepository>().SingleInstance();
        builder.RegisterType<SaleRepositoryStub>().As<ISaleRepository>().SingleInstance();
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
