using MediatR;
using ReceiptsManagementSystem.Domain.Interfaces;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Queries.GetAllReceipts;

public sealed class GetAllReceiptsHandler
    : IRequestHandler<GetAllReceiptsQuery, List<ReceiptListItemDto>>
{
    private readonly IReceiptRepository _repository;

    public GetAllReceiptsHandler(IReceiptRepository repository)
    {
        _repository = repository;
    }

    public async Task<List<ReceiptListItemDto>> Handle(
        GetAllReceiptsQuery request,
        CancellationToken cancellationToken)
    {
        var receipts = await _repository.GetAllAsync(cancellationToken);

        return receipts.Select(r => new ReceiptListItemDto
        {
            Id            = r.Id,
            ReceiptNumber = r.ReceiptNumber,
            CustomerName  = r.CustomerName,
            Amount        = r.Amount.Amount,
            Currency      = r.Amount.Currency,
            Description   = r.Description,
            PaymentMethod = r.PaymentMethod,
            Status        = r.Status,
            CreatedAt     = r.CreatedAt
        }).ToList();
    }
}
