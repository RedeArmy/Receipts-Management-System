using ReceiptsManagementSystem.Domain.ValueObjects;

namespace ReceiptsManagementSystem.Domain.Aggregates;

public sealed class Receipt
{
    public Guid Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public DateTime Date { get; private set; }
    public List<Money> Items { get; private set; }
    public Money Total => CalculateTotal();

    // Constructor principal
    public Receipt(CustomerId customerId, List<Money> items)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        Items = items ?? throw new ArgumentNullException(nameof(items));
        if (!Items.Any())
            throw new ArgumentException("Receipt must have at least one item.", nameof(items));
        Date = DateTime.UtcNow;
    }

    // Lógica de negocio interna
    private Money CalculateTotal()
    {
        decimal sum = Items.Sum(item => item.Amount);
        string currency = Items.First().Currency; // asumimos misma moneda
        return new Money(sum, currency);
    }

    // Método para agregar item
    public void AddItem(Money item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        Items.Add(item);
    }

    // Método para eliminar item
    public void RemoveItem(Money item)
    {
        if (!Items.Contains(item))
            throw new InvalidOperationException("Item not found in receipt.");

        Items.Remove(item);
    }
}