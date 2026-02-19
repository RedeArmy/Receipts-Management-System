using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using MediatR;
using ReceiptsManagementSystem.Application.Features.Receipts.Queries.GetAllReceipts;
using ReceiptsManagementSystem.Presentation.ViewModels.Base;

namespace ReceiptsManagementSystem.Presentation.ViewModels.Receipts;

public sealed partial class ReceiptListViewModel : BaseViewModel
{
    private readonly IMediator _mediator;

    [ObservableProperty]
    private ObservableCollection<ReceiptListItemDto> _receipts = [];

    [ObservableProperty]
    private string _searchText = string.Empty;

    public ReceiptListViewModel(IMediator mediator)
    {
        _mediator = mediator;
        Title     = "Recibos";
    }

    /// <summary>
    /// Se llama automáticamente cuando la vista se carga.
    /// </summary>
    [RelayCommand]
    private async Task LoadReceiptsAsync()
    {
        await ExecuteBusyAsync(async () =>
        {
            var result = await _mediator.Send(
                new GetAllReceiptsQuery(),
                CancellationToken.None);

            Receipts = new ObservableCollection<ReceiptListItemDto>(result);
        });
    }

    [RelayCommand]
    private async Task RefreshAsync() => await LoadReceiptsAsync();
}
