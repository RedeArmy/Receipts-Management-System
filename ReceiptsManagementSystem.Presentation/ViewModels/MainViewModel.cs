using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReceiptsManagementSystem.Presentation.Services;
using ReceiptsManagementSystem.Presentation.ViewModels.Base;
using ReceiptsManagementSystem.Presentation.ViewModels.Receipts;
using ReceiptsManagementSystem.Presentation.ViewModels.Settings;
using Strings = ReceiptsManagementSystem.Presentation.Resources.Resources.Resources;

namespace ReceiptsManagementSystem.Presentation.ViewModels;

public sealed partial class MainViewModel : BaseViewModel
{
    private readonly ReceiptListViewModel _receiptListViewModel;
    private readonly CreateReceiptViewModel _createReceiptViewModel;
    private readonly SettingsViewModel _settingsViewModel;
    private readonly ILocalizationService _localization;

    [ObservableProperty]
    private BaseViewModel _currentViewModel = null!;

    [ObservableProperty]
    private string _activeMenu = "Receipts";

    public ILocalizationService Localization => _localization;

    public MainViewModel(
        ReceiptListViewModel receiptListViewModel,
        CreateReceiptViewModel createReceiptViewModel,
        SettingsViewModel settingsViewModel,
        ILocalizationService localization)
    {
        _receiptListViewModel   = receiptListViewModel;
        _createReceiptViewModel = createReceiptViewModel;
        _settingsViewModel = settingsViewModel;
        _localization = localization;

        _localization.PropertyChanged += (_, _) => UpdateTitles();

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
    private void NavigateToSettings()
    {
        CurrentViewModel = _settingsViewModel;
        ActiveMenu = "Settings";
        Title = Strings.Page_Settings;
    }

    [RelayCommand]
    private void ChangeLanguage(string cultureCode)
    {
        _localization.ChangeLanguage(cultureCode);
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
            Title = _localization.GetString("Page_Receipts");
        }
        else if (CurrentViewModel == _createReceiptViewModel)
        {
            Title = _localization.GetString("Page_NewReceipt");
        }
        else if (CurrentViewModel == _settingsViewModel)
        {
            Title = _localization.GetString("Page_Settings");
        }
    }
}
