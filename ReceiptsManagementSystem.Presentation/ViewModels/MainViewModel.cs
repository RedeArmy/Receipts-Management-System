using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReceiptsManagementSystem.Presentation.Services;
using ReceiptsManagementSystem.Presentation.ViewModels.Base;
using ReceiptsManagementSystem.Presentation.ViewModels.Receipts;
using Strings = ReceiptsManagementSystem.Presentation.Resources.Resources.Resources;

namespace ReceiptsManagementSystem.Presentation.ViewModels;

public sealed partial class MainViewModel : BaseViewModel
{
    private readonly ReceiptListViewModel _receiptListViewModel;
    private readonly CreateReceiptViewModel _createReceiptViewModel;
    private readonly ILocalizationService _localization;

    public ILocalizationService Localization => _localization;

    [ObservableProperty]
    private BaseViewModel _currentViewModel = null!;

    [ObservableProperty]
    private string _activeMenu = "Receipts";

    [ObservableProperty]
    private string _selectedLanguage = "es";

    public MainViewModel(
        ReceiptListViewModel receiptListViewModel,
        CreateReceiptViewModel createReceiptViewModel,
        ILocalizationService localization)
    {
        _receiptListViewModel   = receiptListViewModel;
        _createReceiptViewModel = createReceiptViewModel;
        _localization = localization;

        SelectedLanguage = _localization.CurrentLanguage;

        _localization.PropertyChanged += (_, _) =>
        {
            SelectedLanguage = _localization.CurrentLanguage;
            UpdateTitles();
        };

        NavigateToReceiptList();
    }

    [RelayCommand]
    private void NavigateToReceiptList()
    {
        CurrentViewModel = _receiptListViewModel;
        ActiveMenu       = "Receipts";
        Title            = Strings.Page_Receipts;
    }

    [RelayCommand]
    private void NavigateToCreateReceipt()
    {
        CurrentViewModel = _createReceiptViewModel;
        ActiveMenu       = "CreateReceipt";
        Title            = Strings.Page_NewReceipt;
    }

    [RelayCommand]
    private void ChangeLanguage(string cultureCode)
    {
        _localization.ChangeLanguage(cultureCode);
        SelectedLanguage = cultureCode;
    }

    [RelayCommand]
    private void Exit()
    {
        var result = MessageBox.Show(
            Strings.Message_ExitConfirmation,
            Strings.Message_ExitTitle,
            MessageBoxButton.YesNo,
            MessageBoxImage.Question);

        if (result == MessageBoxResult.Yes)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }

    private void UpdateTitles()
    {
        if (CurrentViewModel == _receiptListViewModel)
        {
            Title = Strings.Page_Receipts;
        }
        else if (CurrentViewModel == _createReceiptViewModel)
        {
            Title = Strings.Page_NewReceipt;
        }
    }
}
