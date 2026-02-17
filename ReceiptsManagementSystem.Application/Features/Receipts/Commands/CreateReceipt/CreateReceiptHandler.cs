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
        var dto = request.ReceiptDto;
        
        var nextNumber = await _receiptRepository.GetNextReceiptNumberAsync(cancellationToken);
        
        var customerId = new CustomerId(dto.CustomerId);
        var amount     = new Money(dto.Amount.Amount, dto.Amount.Currency);
       
        var receipt = new Receipt(
            receiptNumber:         nextNumber,
            customerId:            customerId,
            customerName:          dto.CustomerName,
            amount:                amount,
            description:           dto.Description,
            paymentMethod:         dto.PaymentMethod,
            checkOrTransferNumber: dto.CheckOrTransferNumber,
            accountNumber:         dto.AccountNumber,
            bank:                  dto.Bank,
            customerSignatureName: dto.CustomerSignatureName,
            receiverName:          dto.ReceiverName);

        // 4. Persistir
        await _receiptRepository.AddAsync(receipt, cancellationToken);

        return receipt.Id;
    }
}