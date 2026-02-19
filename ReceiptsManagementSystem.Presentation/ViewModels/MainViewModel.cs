using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReceiptsManagementSystem.Presentation.ViewModels.Base;
using ReceiptsManagementSystem.Presentation.ViewModels.Receipts;

namespace ReceiptsManagementSystem.Presentation.ViewModels;

public sealed partial class MainViewModel : BaseViewModel
{
    private readonly ReceiptListViewModel _receiptListViewModel;
    private readonly CreateReceiptViewModel _createReceiptViewModel;

    [ObservableProperty]
    private BaseViewModel _currentViewModel = null!;

    [ObservableProperty]
    private string _activeMenu = "Receipts";

    public MainViewModel(
        ReceiptListViewModel receiptListViewModel,
        CreateReceiptViewModel createReceiptViewModel)
    {
        _receiptListViewModel   = receiptListViewModel;
        _createReceiptViewModel = createReceiptViewModel;

        // Vista inicial al abrir la app
        NavigateToReceiptList();
    }

    [RelayCommand]
    private void NavigateToReceiptList()
    {
        CurrentViewModel = _receiptListViewModel;
        ActiveMenu       = "Receipts";
        Title            = "Recibos";
    }

    [RelayCommand]
    private void NavigateToCreateReceipt()
    {
        CurrentViewModel = _createReceiptViewModel;
        ActiveMenu       = "CreateReceipt";
        Title            = "Nuevo Recibo";
    }
}
