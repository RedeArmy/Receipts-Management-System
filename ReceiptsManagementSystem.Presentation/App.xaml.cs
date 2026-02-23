using System.Windows;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using ReceiptsManagementSystem.Application;
using ReceiptsManagementSystem.Infrastructure;
using ReceiptsManagementSystem.Infrastructure.Database;
using ReceiptsManagementSystem.Presentation.Configuration;
using ReceiptsManagementSystem.Presentation.Services;
using ReceiptsManagementSystem.Presentation.ViewModels;
using ReceiptsManagementSystem.Presentation.ViewModels.Receipts;

namespace ReceiptsManagementSystem.Presentation;

public partial class App
{
    private readonly IHost _host;

    public App()
    {
        _host = Host.CreateDefaultBuilder()
            .ConfigureAppConfiguration((context, config) =>
            {
                config.AddJsonFile(
                    "appsettings.json",
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

                // Application e Infrastructure
                services.AddApplication();
                services.AddInfrastructure(dbSettings.ResolvedConnectionString);

                services.AddSingleton<ILocalizationService, LocalizationService>();
                services.AddTransient<LanguageSelectorViewModel>();

                // ViewModels
                services.AddTransient<ReceiptListViewModel>();
                services.AddTransient<CreateReceiptViewModel>();
                services.AddSingleton<MainViewModel>();

                // MainWindow
                services.AddTransient<MainWindow>();
            })
            .Build();
    }

    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        await _host.StartAsync();

        try
        {
            var migrationRunner = _host.Services
                .GetRequiredService<MigrationRunner>();

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

        try
        {
            var mainWindow = _host.Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
        catch (Exception ex)
        {
            MessageBox.Show(
                $"Error:\n\n{ex.Message}\n\n" +
                $"Stack Trace:\n{ex.StackTrace}\n\n" +
                $"Inner Exception:\n{ex.InnerException?.Message}",
                "Critical Error",
                MessageBoxButton.OK,
                MessageBoxImage.Error);

            Shutdown(1);
        }
    }

    protected override async void OnExit(ExitEventArgs e)
    {
        await _host.StopAsync();
        _host.Dispose();
        base.OnExit(e);
    }
}
