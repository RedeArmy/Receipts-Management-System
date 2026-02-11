using ReceiptsManagementSystem.Domain.Aggregates;

namespace ReceiptsManagementSystem.Domain.Interfaces;

public interface IReceiptRepository
{
    Task<Receipt> GetByIdAsync(Guid id);
    Task AddAsync(Receipt receipt, CancellationToken cancellationToken);
    Task UpdateAsync(Receipt receipt);
    Task DeleteAsync(Guid id);
    Task<List<Receipt>> GetAllAsync();
}