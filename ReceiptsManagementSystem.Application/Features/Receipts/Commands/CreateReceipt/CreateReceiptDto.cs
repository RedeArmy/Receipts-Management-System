using ReceiptsManagementSystem.Domain.Enums;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Commands.CreateReceipt;

public sealed class CreateReceiptDto
{
    // Cliente
    public Guid CustomerId { get; set; }
    public string CustomerName { get; set; } = string.Empty;

    // Monto
    public MoneyDto Amount { get; set; } = new();

    // Concepto
    public string Description { get; set; } = string.Empty;

    // Método de pago
    public PaymentMethod PaymentMethod { get; set; }

    // Campos adicionales según el método de pago
    public string? CheckOrTransferNumber { get; set; } 
    public string? AccountNumber { get; set; } 
    public string? Bank { get; set; }

    // Saldo pendiente
    //public MoneyDto? PendingBalance { get; set; }

    // Firmas
    public string CustomerSignatureName { get; set; } = string.Empty;
    public string ReceiverName { get; set; } = string.Empty;

    public ReceiptStatus Status { get; set; } = ReceiptStatus.Active;
    public string? CancellationReason { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}

public sealed class MoneyDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = "GTQ";
}