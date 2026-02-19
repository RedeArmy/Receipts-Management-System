using ReceiptsManagementSystem.Domain.Enums;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Queries.GetAllReceipts;

public sealed class ReceiptListItemDto
{
    public Guid Id                { get; init; }
    public int ReceiptNumber      { get; init; }
    public string CustomerName    { get; init; } = string.Empty;
    public decimal Amount         { get; init; }
    public string Currency        { get; init; } = "GTQ";
    public string Description     { get; init; } = string.Empty;
    public PaymentMethod PaymentMethod { get; init; }
    public ReceiptStatus Status   { get; init; }
    public DateTime CreatedAt     { get; init; }
}
