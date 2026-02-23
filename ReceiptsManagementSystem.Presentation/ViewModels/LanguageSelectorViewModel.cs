using System.Collections.Generic;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReceiptsManagementSystem.Presentation.Services;

namespace ReceiptsManagementSystem.Presentation.ViewModels;

public sealed partial class LanguageSelectorViewModel : ObservableObject
{
    private readonly LocalizationService _localizationService;

    [ObservableProperty]
    private string _currentLanguage;

    public List<LanguageOption> AvailableLanguages { get; }

    public LanguageSelectorViewModel()
    {
        _localizationService = LocalizationService.Instance;
        _currentLanguage = _localizationService.CurrentCulture.Name;

        AvailableLanguages = new List<LanguageOption>
        {
            new("es-GT", "🇬🇹 Español"),
            new("en-US", "🇺🇸 English")
        };
    }

    [RelayCommand]
    private void ChangeLanguage(string cultureCode)
    {
        _localizationService.ChangeLanguage(cultureCode);
        CurrentLanguage = cultureCode;
    }
}

public sealed record LanguageOption(string Code, string Display);
