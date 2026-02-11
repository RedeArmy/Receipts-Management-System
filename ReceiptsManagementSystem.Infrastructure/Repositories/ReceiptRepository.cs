using Dapper;
using Microsoft.Data.Sqlite;
using ReceiptsManagementSystem.Domain.Aggregates;
using ReceiptsManagementSystem.Domain.Interfaces;

namespace ReceiptsManagementSystem.Infrastructure.Repositories;

public sealed class ReceiptRepository : IReceiptRepository
{
    private readonly string _connectionString;

    public ReceiptRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task AddAsync(Receipt receipt)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync(
            @"INSERT INTO Receipts (Id, CustomerId, Date, Total)
              VALUES (@Id, @CustomerId, @Date, @Total)",
            new
            {
                receipt.Id,
                CustomerId = receipt.CustomerId.Value,
                receipt.Date,
                Total = receipt.Total.Amount
            });
    }

    public async Task<Receipt> GetByIdAsync(Guid id)
    {
        using var connection = new SqliteConnection(_connectionString);
        var receipt = await connection.QuerySingleOrDefaultAsync<Receipt>(
            "SELECT * FROM Receipts WHERE Id = @Id", new { Id = id });
        return receipt!;
    }

    public async Task UpdateAsync(Receipt receipt)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync(
            @"UPDATE Receipts
              SET CustomerId = @CustomerId, Date = @Date, Total = @Total
              WHERE Id = @Id",
            new
            {
                receipt.Id,
                CustomerId = receipt.CustomerId.Value,
                receipt.Date,
                Total = receipt.Total.Amount
            });
    }

    public async Task DeleteAsync(Guid id)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.ExecuteAsync("DELETE FROM Receipts WHERE Id = @Id", new { Id = id });
    }

    public async Task<List<Receipt>> GetAllAsync()
    {
        using var connection = new SqliteConnection(_connectionString);
        var list = await connection.QueryAsync<Receipt>("SELECT * FROM Receipts");
        return list.ToList();
    }
}