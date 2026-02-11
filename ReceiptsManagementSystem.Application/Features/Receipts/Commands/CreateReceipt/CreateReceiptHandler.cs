using MediatR;
using ReceiptsManagementSystem.Domain.Aggregates;
using ReceiptsManagementSystem.Domain.Interfaces;
using ReceiptsManagementSystem.Domain.ValueObjects;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Commands.CreateReceipt;

public sealed class CreateReceiptHandler : IRequestHandler<CreateReceiptCommand, Guid>
{
    private readonly IReceiptRepository _receiptRepository;

    public CreateReceiptHandler(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository ?? throw new ArgumentNullException(nameof(receiptRepository));
    }

    public async Task<Guid> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {
        
        if (request.ReceiptDto is null) throw new ArgumentNullException(nameof(request.ReceiptDto));
        if (request.ReceiptDto.Items is null || !request.ReceiptDto.Items.Any()) throw new ArgumentException("Receipt must contain at least one item.", nameof(request.ReceiptDto.Items));
        
        // Convertir DTO a Value Objects
        var customerId = new CustomerId(request.ReceiptDto.CustomerId);
        var items = request.ReceiptDto.Items
            .Select(i => new Money(i.Amount, i.Currency))
            .ToList();

        // Crear Aggregate Root
        var receipt = new Receipt(customerId, items);

        // Persistir usando repositorio
        await _receiptRepository.AddAsync(receipt, cancellationToken);

        // Devolver Id
        return receipt.Id;
    }
}