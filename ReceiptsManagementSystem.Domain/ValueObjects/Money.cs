namespace ReceiptsManagementSystem.Domain.ValueObjects;

public sealed class Money
{
    public decimal Amount { get; }
    public string Currency { get; }

    public Money(decimal amount, string currency = "GTQ")
    {
        if (amount < 0)
            throw new ArgumentException("Amount cannot be negative", nameof(amount));

        Amount = amount;
        Currency = currency;
    }

    // Ejemplo de operación dentro de Value Object
    public Money Add(Money other)
    {
        if (Currency != other.Currency)
            throw new InvalidOperationException("Cannot add amounts with different currencies");
        
        return new Money(Amount + other.Amount, Currency);
    }

    public override string ToString() => $"{Currency} {Amount:N2}";

    // Igualdad basada en valor
    public override bool Equals(object? obj) =>
        obj is Money other && Amount == other.Amount && Currency == other.Currency;

    public override int GetHashCode() => HashCode.Combine(Amount, Currency);
}