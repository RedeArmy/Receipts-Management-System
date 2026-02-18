using ReceiptsManagementSystem.Domain.Enums;
using ReceiptsManagementSystem.Domain.ValueObjects;

namespace ReceiptsManagementSystem.Domain.Aggregates;

/// <summary>
/// DTO usado exclusivamente para reconstituir un Receipt desde persistencia.
/// No debe usarse para creación de nuevos recibos.
/// </summary>
public sealed record ReceiptReconstitutionDto
{
    public required Guid Id { get; init; }
    public required int ReceiptNumber { get; init; }
    public required CustomerId CustomerId { get; init; }
    public required string CustomerName { get; init; }
    public required Money Amount { get; init; }
    public required string Description { get; init; }
    public required PaymentMethod PaymentMethod { get; init; }
    public string? CheckOrTransferNumber { get; init; }
    public string? AccountNumber { get; init; }
    public string? Bank { get; init; }
    public required string CustomerSignatureName { get; init; }
    public required string ReceiverName { get; init; }
    public required ReceiptStatus Status { get; init; }
    public string? CancellationReason { get; init; }
    public required DateTime CreatedAt { get; init; }
}