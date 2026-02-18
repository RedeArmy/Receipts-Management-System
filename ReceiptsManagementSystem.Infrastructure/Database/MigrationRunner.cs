using System.Data;
using Dapper;
using Microsoft.Data.Sqlite;

namespace ReceiptsManagementSystem.Infrastructure.Database;

public class MigrationRunner
{
    private readonly string _connectionString;
    private readonly string _migrationsFolder;

    public MigrationRunner(string connectionString, string migrationsFolder)
    {
        _connectionString = connectionString;
        _migrationsFolder = migrationsFolder;
    }

    public void Migrate()
    {
        using var connection = new SqliteConnection(_connectionString);
        connection.Open();

        // Crear tabla de control de migraciones si no existe
        connection.Execute(@"
            CREATE TABLE IF NOT EXISTS __MigrationsHistory (
                MigrationId TEXT PRIMARY KEY,
                AppliedOn TEXT NOT NULL
            );
        ");

        // Obtener scripts aplicados
        var applied = connection.Query<string>("SELECT MigrationId FROM __MigrationsHistory").ToHashSet();

        // Ejecutar scripts pendientes
        var files = Directory.GetFiles(_migrationsFolder, "*.sql")
            .OrderBy(f => f)
            .ToList();

        foreach (var file in files)
        {
            var migrationId = Path.GetFileName(file);
            if (applied.Contains(migrationId))
            {
                continue;
            }

            var sql = File.ReadAllText(file);
            using var transaction = connection.BeginTransaction();
            connection.Execute(sql, transaction: transaction);
            connection.Execute(
                "INSERT INTO __MigrationsHistory(MigrationId, AppliedOn) VALUES(@MigrationId, @AppliedOn)",
                new { MigrationId = migrationId, AppliedOn = DateTime.UtcNow },
                transaction: transaction
            );
            transaction.Commit();

            Console.WriteLine($"Migration applied: {migrationId}");
        }
    }
}