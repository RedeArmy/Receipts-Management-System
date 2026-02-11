using ReceiptsManagementSystem.Domain.ValueObjects;

namespace ReceiptsManagementSystem.Domain.Aggregates;

public sealed class Receipt
{
    public Guid Id { get; private set; }
    public CustomerId CustomerId { get; private set; }
    public DateTime Date { get; private set; }
    public IReadOnlyList<Money> Items => _items.AsReadOnly();
    private List<Money> _items;
    public Money Total => CalculateTotal();

    // Constructor principal
    public Receipt(CustomerId customerId, List<Money> items)
    {
        Id = Guid.NewGuid();
        CustomerId = customerId ?? throw new ArgumentNullException(nameof(customerId));
        _items = items ?? throw new ArgumentNullException(nameof(items));
        if (!_items.Any())
            throw new ArgumentException("Receipt must have at least one item.", nameof(items));
        Date = DateTime.UtcNow;
    }

    // Lógica de negocio interna
    private Money CalculateTotal()
    {
        decimal sum = _items.Sum(item => item.Amount);
        string currency = _items.First().Currency; // asumimos misma moneda
        return new Money(sum, currency);
    }

    // Método para agregar item
    public void AddItem(Money item)
    {
        if (item is null) throw new ArgumentNullException(nameof(item));
        if (_items.Any() && item.Currency != _items.First().Currency)
            throw new InvalidOperationException("All items must have the same currency.");
        _items.Add(item);
    }

    // Método para eliminar item
    public void RemoveItem(Money item)
    {
        if (!_items.Contains(item))
            throw new InvalidOperationException("Item not found in receipt.");

        _items.Remove(item);
    }
}