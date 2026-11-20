using Microsoft.Extensions.Configuration;

namespace ShoeStore.Infrastructure;

public static class AppConfiguration
{
    public static IConfiguration Instance { get; } = new ConfigurationBuilder()
        .SetBasePath(AppContext.BaseDirectory)
        .AddJsonFile("appsettings.json", optional: false)
        .Build();
}
