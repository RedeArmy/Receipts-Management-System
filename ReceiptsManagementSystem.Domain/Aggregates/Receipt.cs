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

    public static Receipt Reconstitute(
        Guid id,
        int receiptNumber,
        CustomerId customerId,
        string customerName,
        Money amount,
        string description,
        PaymentMethod paymentMethod,
        string? checkOrTransferNumber,
        string? accountNumber,
        string? bank,
        string customerSignatureName,
        string receiverName,
        ReceiptStatus status,
        string? cancellationReason,
        DateTime createdAt)
    {
        return new Receipt
        {
            Id = id,
            ReceiptNumber = receiptNumber,
            CustomerId = customerId,
            CustomerName = customerName,
            Amount = amount,
            Description = description,
            PaymentMethod = paymentMethod,
            CheckOrTransferNumber = checkOrTransferNumber,
            AccountNumber = accountNumber,
            Bank = bank,
            CustomerSignatureName = customerSignatureName,
            ReceiverName = receiverName,
            Status = status,
            CancellationReason = cancellationReason,
            CreatedAt = createdAt
        };
    }

    private void ValidatePaymentFields()
    {
        if (PaymentMethod is PaymentMethod.Check or PaymentMethod.Transfer)
        {
            if (string.IsNullOrWhiteSpace(CheckOrTransferNumber))
                throw new ArgumentException("Check or transfer number is required for check/transfer payment.");
        }

        if (PaymentMethod == PaymentMethod.Transfer)
        {
            if (string.IsNullOrWhiteSpace(AccountNumber) || string.IsNullOrWhiteSpace(Bank))
                throw new ArgumentException("Account number and Bank are required for transfer payment.");
        }
    }

    // Método para cancelar recibo
    public void Cancel(string reason)
    {
        if (Status == ReceiptStatus.Cancelled)
            throw new InvalidOperationException("Receipt is already cancelled.");

        if (string.IsNullOrWhiteSpace(reason))
            throw new ArgumentException("Cancellation reason must be provided.", nameof(reason));

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