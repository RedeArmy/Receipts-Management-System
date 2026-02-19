using MediatR;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Queries.GetAllReceipts;

public sealed record GetAllReceiptsQuery : IRequest<List<ReceiptListItemDto>>;
