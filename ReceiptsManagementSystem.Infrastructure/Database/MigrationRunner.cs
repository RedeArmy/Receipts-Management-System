using System.Reflection;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ReceiptsManagementSystem.Infrastructure.Database;

public class MigrationRunner(string connectionString)
{
    public void Migrate()
    {
        using var connection = new SqliteConnection(connectionString);
        connection.Open();

        CreateMigrationHistoryTable(connection);
        RunPendingMigrations(connection);
    }

    private static void CreateMigrationHistoryTable(SqliteConnection connection)
    {
        const string sql = @"
            CREATE TABLE IF NOT EXISTS __MigrationsHistory (
                MigrationId TEXT PRIMARY KEY,
                AppliedOn TEXT NOT NULL
            )";

        connection.Execute(sql);
    }

    private void RunPendingMigrations(SqliteConnection connection)
    {
        var assembly = Assembly.GetExecutingAssembly();

        var migrationScripts = assembly
            .GetManifestResourceNames()
            .Where(name => name.Contains(".Migrations.") && name.EndsWith(".sql"))
            .OrderBy(name => name)
            .ToList();

        if (migrationScripts.Count == 0)
        {
            throw new InvalidOperationException(
                "No se encontraron migraciones SQL embebidas.");
        }

        var appliedMigrations = connection
            .Query<string>("SELECT MigrationId FROM __MigrationsHistory")
            .ToHashSet();

        foreach (var scriptName in migrationScripts)
        {
            var migrationId = Path.GetFileNameWithoutExtension(scriptName);

            if (appliedMigrations.Contains(migrationId))
            {
                Console.WriteLine($"Migration already applied: {migrationId}");
                continue;
            }

            var script = ReadEmbeddedResource(assembly, scriptName);

            using var transaction = connection.BeginTransaction();

            try
            {
                connection.Execute(script, transaction: transaction);

                connection.Execute(
                    "INSERT INTO __MigrationsHistory (MigrationId, AppliedOn) VALUES (@MigrationId, @AppliedOn)",
                    new { MigrationId = migrationId, AppliedOn = DateTime.UtcNow.ToString("O") },
                    transaction: transaction);

                transaction.Commit();

                Console.WriteLine($"Migration applied: {migrationId}");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                throw new InvalidOperationException(
                    $"Error applying migration {migrationId}: {ex.Message}", ex);
            }
        }
    }

    private static string ReadEmbeddedResource(Assembly assembly, string resourceName)
    {
        using var stream = assembly.GetManifestResourceStream(resourceName)
            ?? throw new InvalidOperationException($"Resource {resourceName} not found");

        using var reader = new StreamReader(stream);
        return reader.ReadToEnd();
    }
}
