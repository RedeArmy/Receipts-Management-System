using Microsoft.Extensions.DependencyInjection;
using ReceiptsManagementSystem.Domain.Interfaces;
using ReceiptsManagementSystem.Infrastructure.Database;

namespace ReceiptsManagementSystem.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, string connectionString)
    {
        // Registrar repositorios
        services.AddSingleton<IReceiptRepository>(sp => new Repositories.ReceiptRepository(connectionString));

        // Registrar MigrationRunner como singleton
        services.AddSingleton(sp =>
        {
            // Carpeta donde estarán los scripts de migración
            var migrationsFolder = Path.Combine(AppContext.BaseDirectory, "Database", "Migrations");
            return new MigrationRunner(connectionString, migrationsFolder);
        });

        return services;
    }
}