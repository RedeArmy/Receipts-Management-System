using Microsoft.Extensions.DependencyInjection;
using ReceiptsManagementSystem.Domain.Interfaces;
using ReceiptsManagementSystem.Infrastructure.Database;
using ReceiptsManagementSystem.Infrastructure.Repositories;

namespace ReceiptsManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        string connectionString)
    {
        services.AddSingleton(new MigrationRunner(connectionString));

        services.AddScoped<IReceiptRepository>(sp =>
            new ReceiptRepository(connectionString));

        return services;
    }
}

public interface IConnectionStringProvider
{
    string ConnectionString { get; }
}

public class ConnectionStringProvider(string connectionString) : IConnectionStringProvider
{
    public string ConnectionString { get; } = connectionString;
}
