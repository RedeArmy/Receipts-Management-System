using System.Configuration;
using System.Data;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReceiptsManagementSystem.Application;
using ReceiptsManagementSystem.Infrastructure;
using ReceiptsManagementSystem.Infrastructure.Database;

namespace ReceiptsManagementSystem.Presentation;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : System.Windows.Application
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureServices((context, services) =>
            {
                string connectionString = "Data Source=receipts.db";

                // Registrar Application
                services.AddApplication();

                // Registrar Infrastructure (repositorios + MigrationRunner)
                services.AddInfrastructure(connectionString);

                // Registrar MainWindow para DI
                services.AddTransient<MainWindow>();
            })
            .Build();

        // Ejecutar migraciones antes de iniciar la UI
        var migrationRunner = _host.Services.GetRequiredService<MigrationRunner>();
        migrationRunner.Migrate();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Iniciar host
        await _host.StartAsync();

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