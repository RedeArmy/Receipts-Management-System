using MediatR;

namespace ReceiptsManagementSystem.Application.Features.Receipts.Commands.CreateReceipt;

public sealed record CreateReceiptCommand(CreateReceiptDto ReceiptDto) : IRequest<Guid>;