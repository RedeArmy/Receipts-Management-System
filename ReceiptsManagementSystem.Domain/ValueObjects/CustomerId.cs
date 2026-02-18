namespace ReceiptsManagementSystem.Domain.ValueObjects;

public sealed class CustomerId
{
    public Guid Value { get; }

    public CustomerId(Guid value)
    {
        if (value == Guid.Empty)
        {
            throw new ArgumentException("CustomerId cannot be empty", nameof(value));
        }
        
        Value = value;
    }

    public override bool Equals(object? obj) =>
        obj is CustomerId other && Value == other.Value;

    public override int GetHashCode() => Value.GetHashCode();
}