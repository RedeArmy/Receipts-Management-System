namespace ReceiptsManagementSystem.Application.Features.Receipts.Commands.CreateReceipt;

public sealed class CreateReceiptDto
{
    public Guid CustomerId { get; set; }
    public List<MoneyDto> Items { get; set; } = new();
}

public sealed class MoneyDto
{
    public decimal Amount { get; set; }
    public string Currency { get; set; } = string.Empty;
}