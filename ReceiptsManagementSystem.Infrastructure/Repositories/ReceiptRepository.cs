using Dapper;
using Microsoft.Data.Sqlite;
using ReceiptsManagementSystem.Domain.Aggregates;
using ReceiptsManagementSystem.Domain.Enums;
using ReceiptsManagementSystem.Domain.Interfaces;
using ReceiptsManagementSystem.Domain.ValueObjects;

namespace ReceiptsManagementSystem.Infrastructure.Repositories;

public sealed class ReceiptRepository : IReceiptRepository
{
    private readonly string _connectionString;

    public ReceiptRepository(string connectionString)
    {
        _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
    }
    
    public async Task<int> GetNextReceiptNumberAsync(CancellationToken cancellationToken)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = "SELECT COALESCE(MAX(ReceiptNumber), 0) + 1 FROM Receipts";
        return await connection.ExecuteScalarAsync<int>(
            new CommandDefinition(sql, cancellationToken: cancellationToken));
    }

    public async Task AddAsync(Receipt receipt, CancellationToken cancellationToken)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = """
                               INSERT INTO Receipts (
                                   Id, ReceiptNumber, CustomerId, CustomerName,
                                   Amount, Currency, Description, PaymentMethod,
                                   CheckOrTransferNumber, AccountNumber, Bank,
                                   CustomerSignatureName, ReceiverName,
                                   Status, CancellationReason, CreatedAt
                               )
                               VALUES (
                                   @Id, @ReceiptNumber, @CustomerId, @CustomerName,
                                   @Amount, @Currency, @Description, @PaymentMethod,
                                   @CheckOrTransferNumber, @AccountNumber, @Bank,
                                   @CustomerSignatureName, @ReceiverName,
                                   @Status, @CancellationReason, @CreatedAt
                               )
                           """;

        await connection.ExecuteAsync(
            new CommandDefinition(sql, MapToParameters(receipt),
                cancellationToken: cancellationToken));
    }

    public async Task<Receipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = "SELECT * FROM Receipts WHERE Id = @Id";

        var data = await connection.QuerySingleOrDefaultAsync(
            new CommandDefinition(sql, new { Id = id.ToString() },
                cancellationToken: cancellationToken));

        return data is null ? null : MapToReceipt(data);
    }

    public async Task<List<Receipt>> GetAllAsync(CancellationToken cancellationToken)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = "SELECT * FROM Receipts ORDER BY ReceiptNumber DESC";

        var dataList = await connection.QueryAsync(
            new CommandDefinition(sql, cancellationToken: cancellationToken));

        var result = new List<Receipt>();
        foreach (var d in dataList)
        {
            result.Add(MapToReceipt(d));
        }
        return result;
    }

    public async Task UpdateAsync(Receipt receipt, CancellationToken cancellationToken)
    {
        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = """
                               UPDATE Receipts SET
                                   CustomerName          = @CustomerName,
                                   Amount                = @Amount,
                                   Currency              = @Currency,
                                   Description           = @Description,
                                   PaymentMethod         = @PaymentMethod,
                                   CheckOrTransferNumber = @CheckOrTransferNumber,
                                   AccountNumber         = @AccountNumber,
                                   Bank                  = @Bank,
                                   CustomerSignatureName = @CustomerSignatureName,
                                   ReceiverName          = @ReceiverName
                               WHERE Id = @Id
                           """;

        await connection.ExecuteAsync(
            new CommandDefinition(sql, MapToParameters(receipt),
                cancellationToken: cancellationToken));
    }

    public async Task CancelAsync(Guid id, string reason, CancellationToken cancellationToken)
    {
        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Cancellation reason is required.", nameof(reason));
        }

        using var connection = new SqliteConnection(_connectionString);
        await connection.OpenAsync(cancellationToken);

        const string sql = """
                               UPDATE Receipts
                               SET Status = @Status, CancellationReason = @CancellationReason
                               WHERE Id = @Id
                           """;

        await connection.ExecuteAsync(
            new CommandDefinition(sql, new
            {
                Id = id.ToString(),
                Status = ReceiptStatus.Cancelled.ToString(),
                CancellationReason = reason
            }, cancellationToken: cancellationToken));
    }

    private static object MapToParameters(Receipt r) => new
    {
        Id = r.Id.ToString(),
        r.ReceiptNumber,
        CustomerId = r.CustomerId.Value.ToString(),
        r.CustomerName,
        Amount = r.Amount.Amount,
        Currency = r.Amount.Currency,
        r.Description,
        PaymentMethod = r.PaymentMethod.ToString(),
        r.CheckOrTransferNumber,
        r.AccountNumber,
        r.Bank,
        r.CustomerSignatureName,
        r.ReceiverName,
        Status = r.Status.ToString(),
        r.CancellationReason,
        CreatedAt = r.CreatedAt.ToString("O")
    };

    private static Receipt MapToReceipt(dynamic data)
    {
        return Receipt.Reconstitute(
            id: Guid.Parse((string)data.Id),
            receiptNumber: (int)data.ReceiptNumber,
            customerId: new CustomerId(Guid.Parse((string)data.CustomerId)),
            customerName: (string)data.CustomerName,
            amount: new Money((decimal)data.Amount, (string)data.Currency),
            description: (string)data.Description,
            paymentMethod: Enum.Parse<PaymentMethod>((string)data.PaymentMethod),
            checkOrTransferNumber: (string?)data.CheckOrTransferNumber,
            accountNumber: (string?)data.AccountNumber,
            bank: (string?)data.Bank,
            customerSignatureName: (string)data.CustomerSignatureName,
            receiverName: (string)data.ReceiverName,
            status: Enum.Parse<ReceiptStatus>((string)data.Status),
            cancellationReason: (string?)data.CancellationReason,
            createdAt: DateTime.Parse((string)data.CreatedAt)
        );
    }
}