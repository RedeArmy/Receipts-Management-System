using ReceiptsManagementSystem.Domain.Aggregates;

namespace ReceiptsManagementSystem.Domain.Interfaces;

public interface IReceiptRepository
{
    Task<Receipt?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<int> GetNextReceiptNumberAsync(CancellationToken cancellationToken);
    Task AddAsync(Receipt receipt, CancellationToken cancellationToken);
    Task UpdateAsync(Receipt receipt, CancellationToken cancellationToken);
    Task CancelAsync(Guid id, string reason, CancellationToken cancellationToken);
    Task<List<Receipt>> GetAllAsync(CancellationToken cancellationToken);
}