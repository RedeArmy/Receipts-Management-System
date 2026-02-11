using ReceiptsManagementSystem.Domain.ValueObjects;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Commands.CreateReceipt;

public sealed class CreateReceiptDto
{
    public Guid CustomerId { get; set; }
    public List<decimal> ItemAmounts { get; set; } = new();
    public string Currency { get; set; } = "USD";
}