using ReceiptsManagementSystem.Domain.ValueObjects;
using ReceiptsManagementSystem.Domain.Enums;

namespace ReceiptsManagementSystem.Domain.Aggregates;

public sealed class Receipt
{
    public Guid Id { get; private set; }
    public int ReceiptNumber { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public string CustomerName { get; private set; } = string.Empty;
    public Money Amount { get; private set; }
    public string Description { get; private set; } = string.Empty;
    public PaymentMethod PaymentMethod { get; private set; }
    public string? CheckOrTransferNumber { get; private set; }
    public string? AccountNumber { get; private set; }
    public string? Bank { get; private set; }
    //public Money? PendingBalance { get; private set; }
    public string CustomerSignatureName { get; private set; } = string.Empty;
    public string ReceiverName { get; private set; } = string.Empty;
    public ReceiptStatus Status { get; private set; } = ReceiptStatus.Active;
    public string? CancellationReason { get; private set; }
    public DateTime CreatedAt { get; private set; }

    // Constructor principal
    public Receipt(
        int receiptNumber,
        CustomerId customerId,
        string customerName,
        Money amount,
        string description,
        PaymentMethod paymentMethod,
        string? checkOrTransferNumber,
        string? accountNumber,
        string? bank,
        //Money? pendingBalance,
        string customerSignatureName,
        string receiverName)
    {
        Id = Guid.NewGuid();
        ReceiptNumber = receiptNumber;
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        CustomerName = string.IsNullOrWhiteSpace(customerName)
            ? throw new ArgumentNullException(nameof(customerName))
            : customerName;
        Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        Description = string.IsNullOrWhiteSpace(description)
            ? throw new ArgumentNullException(nameof(description))
            : description;
        PaymentMethod = paymentMethod;
        CheckOrTransferNumber = checkOrTransferNumber;
        AccountNumber = accountNumber;
        Bank = bank;
        //PendingBalance = pendingBalance;
        CustomerSignatureName = string.IsNullOrWhiteSpace(customerSignatureName)
            ? throw new ArgumentNullException(nameof(customerSignatureName))
            : customerSignatureName;
        ReceiverName = string.IsNullOrWhiteSpace(receiverName)
            ? throw new ArgumentNullException(nameof(receiverName))
            : receiverName;
        CreatedAt = DateTime.UtcNow;

        ValidatePaymentFields();
    }

    private Receipt()
    {
        CustomerId = null!;
        Amount = null!;
    }

    public static Receipt Reconstitute(ReceiptReconstitutionDto dto)
    
    {
        return new Receipt
        {
            Id = dto.Id,
            ReceiptNumber = dto.ReceiptNumber,
            CustomerId = dto.CustomerId,
            CustomerName = dto.CustomerName,
            Amount = dto.Amount,
            Description = dto.Description,
            PaymentMethod = dto.PaymentMethod,
            CheckOrTransferNumber = dto.CheckOrTransferNumber,
            AccountNumber = dto.AccountNumber,
            Bank = dto.Bank,
            CustomerSignatureName = dto.CustomerSignatureName,
            ReceiverName = dto.ReceiverName,
            Status = dto.Status,
            CancellationReason = dto.CancellationReason,
            CreatedAt = dto.CreatedAt
        };
    }

    private void ValidatePaymentFields()
    {
        switch (PaymentMethod)
        {
            case PaymentMethod.Check or PaymentMethod.Transfer
                when string.IsNullOrWhiteSpace(CheckOrTransferNumber):
                throw new ArgumentException(
                    "Check or transfer number is required for check/transfer payment.");
            case PaymentMethod.Transfer when string.IsNullOrWhiteSpace(AccountNumber) || string.IsNullOrWhiteSpace(Bank):
                throw new ArgumentException("Account number and Bank are required for transfer payment.");
        }
    }

    // Método para cancelar recibo
    public void Cancel(string reason)
    {
        if (Status == ReceiptStatus.Cancelled)
        {
            throw new InvalidOperationException("Receipt is already cancelled.");
        }

        if (string.IsNullOrWhiteSpace(reason))
        {
            throw new ArgumentException("Cancellation reason must be provided.", nameof(reason));
        }

        Status = ReceiptStatus.Cancelled;
        CancellationReason = reason;
    }

    // Método para actualizar monto y descripción
    public void UpdateReceipt(Money amount, string description)
    {
        if (Status == ReceiptStatus.Cancelled)
            throw new InvalidOperationException("Cannot update a cancelled receipt.");

        Amount = amount ?? throw new ArgumentNullException(nameof(amount));
        Description = string.IsNullOrWhiteSpace(description)
            ? throw new ArgumentNullException(nameof(description))
            : description;
    }
}