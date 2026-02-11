using MediatR;
using ReceiptsManagementSystem.Domain.Aggregates;
using ReceiptsManagementSystem.Domain.Interfaces;
using ReceiptsManagementSystem.Domain.ValueObjects;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Commands.CreateReceipt;

public class CreateReceiptHandler
{
    private readonly IReceiptRepository _receiptRepository;

    public CreateReceiptHandler(IReceiptRepository receiptRepository)
    {
        _receiptRepository = receiptRepository ?? throw new ArgumentNullException(nameof(receiptRepository));
    }

    public async Task<Guid> Handle(CreateReceiptCommand request, CancellationToken cancellationToken)
    {
        // Convertir DTO a Value Objects
        var customerId = new CustomerId(request.ReceiptDto.CustomerId);
        var items = request.ReceiptDto.ItemAmounts
            .Select(amount => new Money(amount, request.ReceiptDto.Currency))
            .ToList();

        // Crear Aggregate Root
        var receipt = new Receipt(customerId, items);

        // Persistir usando repositorio
        await _receiptRepository.AddAsync(receipt);

        // Devolver Id
        return receipt.Id;
    }
}