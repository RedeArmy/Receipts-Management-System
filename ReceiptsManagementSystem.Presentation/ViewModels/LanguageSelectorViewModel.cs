using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReceiptsManagementSystem.Presentation.Services;

namespace ReceiptsManagementSystem.Presentation.ViewModels;

public sealed partial class LanguageSelectorViewModel : ObservableObject
{
    private readonly ILocalizationService _localizationService;

    [ObservableProperty]
    private string _currentLanguage;


    public LanguageSelectorViewModel(ILocalizationService localizationService)
    {
        _localizationService = localizationService;
        _currentLanguage = _localizationService.CurrentLanguageCode;

        _localizationService.PropertyChanged += (_, _) =>
        {
            CurrentLanguage = _localizationService.CurrentLanguageCode;
        };
    }

    [RelayCommand]
    private void ChangeLanguage(string cultureCode)
    {
        _localizationService.ChangeLanguage(cultureCode);
        CurrentLanguage = cultureCode;
    }
}

public sealed record LanguageOption;
