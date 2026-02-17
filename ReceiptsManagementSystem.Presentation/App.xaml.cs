using System.Windows;
<<<<<<< Updated upstream
=======
using Microsoft.Extensions.Configuration;
>>>>>>> Stashed changes
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReceiptsManagementSystem.Application;
using ReceiptsManagementSystem.Infrastructure;
using ReceiptsManagementSystem.Infrastructure.Database;
<<<<<<< Updated upstream

namespace ReceiptsManagementSystem.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
=======
using ReceiptsManagementSystem.Presentation.Configuration;

namespace ReceiptsManagementSystem.Presentation;

public partial class App : Application
>>>>>>> Stashed changes
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
<<<<<<< Updated upstream
            .ConfigureServices((context, services) =>
            {
                string connectionString = "Data Source=receipts.db";
=======
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile("appsettings.json",
                    optional: false,
                    reloadOnChange: false);

                config.AddJsonFile(
                    $"appsettings.{context.HostingEnvironment.EnvironmentName}.json",
                    optional: true,
                    reloadOnChange: false);

                config.AddEnvironmentVariables();
            })
            .ConfigureServices((context, services) =>
            {
                var dbSettings = context.Configuration
                                 .GetSection(DatabaseSettings.SectionName)
                                 .Get<DatabaseSettings>()
                             ?? throw new InvalidOperationException(
                                 "Falta la sección 'Database' en appsettings.json");
>>>>>>> Stashed changes

                // Registrar Application
                services.AddApplication();

                // Registrar Infrastructure (repositorios + MigrationRunner)
<<<<<<< Updated upstream
                services.AddInfrastructure(connectionString);
=======
                services.AddInfrastructure(dbSettings.ResolvedConnectionString);
>>>>>>> Stashed changes

                // Registrar MainWindow para DI
                services.AddTransient<MainWindow>();
            })
            .Build();
<<<<<<< Updated upstream

        // Ejecutar migraciones antes de iniciar la UI
        var migrationRunner = _host.Services.GetRequiredService<MigrationRunner>();
        migrationRunner.Migrate();
=======
>>>>>>> Stashed changes
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Iniciar host
        await _host.StartAsync();
<<<<<<< Updated upstream
=======
        
        // Migraciones en OnStartup — aquí podemos manejar errores y mostrarlos
        try
        {
            var migrationRunner = _host.Services.GetRequiredService<MigrationRunner>();
            migrationRunner.Migrate();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error al inicializar la base de datos:\n\n{ex.Message}",
                "Error de inicio",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Shutdown(1);
            return;
        }
>>>>>>> Stashed changes

        // Abrir MainWindow desde DI para poder inyectar servicios y ViewModels
        var mainWindow = _host.Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}