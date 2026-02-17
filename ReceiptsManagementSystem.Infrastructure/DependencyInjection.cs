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
        // Scoped: una instancia por operación, correcto para repositorios
        services.AddScoped<IReceiptRepository>(
            _ => new ReceiptRepository(connectionString));

        // MigrationRunner sí puede ser Singleton
        services.AddSingleton(_ =>
        {
            var migrationsFolder = Path.Combine(
                AppContext.BaseDirectory, "Database", "Migrations");
            return new MigrationRunner(connectionString, migrationsFolder);
        });

        return services;
    }
}