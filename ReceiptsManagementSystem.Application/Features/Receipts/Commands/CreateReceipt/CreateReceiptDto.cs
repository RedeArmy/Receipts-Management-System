using ReceiptsManagementSystem.Domain.Enums;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Commands.CreateReceipt;

public sealed class CreateReceiptDto
{
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GTQ";

    public string Description { get; set; } = string.Empty;

    public PaymentMethod PaymentMethod { get; set; }

    public string? CheckOrTransferNumber { get; set; }
    public string? AccountNumber { get; set; }
    public string? Bank { get; set; }

    // Saldo pendiente
    //public MoneyDto? PendingBalance { get; set; }

    public string CustomerSignatureName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;

    public ReceiptStatus Status { get; set; } = ReceiptStatus.Active;
    public string? CancellationReason { get; set; }
    public DateTime ReceiptDate { get; set; } = DateTime.Today;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
