using System.Windows;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ReceiptsManagementSystem.Presentation.Services;
using ReceiptsManagementSystem.Presentation.ViewModels.Base;

namespace ReceiptsManagementSystem.Presentation.ViewModels.Settings;

public sealed partial class SettingsViewModel : BaseViewModel
{
    private readonly ILocalizationService _localizationService;
    private readonly UserPreferencesService _preferencesService;

    [ObservableProperty]
    private string _selectedLanguage;

    [ObservableProperty]
    private List<LanguageOption> _availableLanguages;

    public ILocalizationService Localization => _localizationService;

    public string SettingsLanguageTitle => _localizationService.GetString("Settings_Language");
    public string SettingsLanguageDescription => _localizationService.GetString("Settings_Language_Description");
    public string SettingsSaveButton => _localizationService.GetString("Settings_Save_Button");
    public string SettingsInfoTitle => _localizationService.GetString("Settings_Info_Title");
    public string SettingsInfoDescription => _localizationService.GetString("Settings_Info_Description");

    public SettingsViewModel(
        ILocalizationService localizationService,
        UserPreferencesService preferencesService)
    {
        _localizationService = localizationService;
        _preferencesService = preferencesService;

        Title = _localizationService.GetString("Page_Settings");

        _availableLanguages = CreateLanguageOptions();
        _selectedLanguage = _preferencesService.PreferredLanguage;

        _selectedLanguage = _preferencesService.PreferredLanguage;

        _localizationService.PropertyChanged += (_, _) =>
        {
            Title = _localizationService.GetString("Page_Settings");
            var currentSelection = SelectedLanguage;
            AvailableLanguages = CreateLanguageOptions();

            SelectedLanguage = currentSelection;

            OnPropertyChanged(nameof(Localization));
            OnPropertyChanged(nameof(SettingsLanguageTitle));
            OnPropertyChanged(nameof(SettingsLanguageDescription));
            OnPropertyChanged(nameof(SettingsSaveButton));
            OnPropertyChanged(nameof(SettingsInfoTitle));
            OnPropertyChanged(nameof(SettingsInfoDescription));
        };
    }

    [RelayCommand]
    private void SaveLanguagePreference()
    {
        _preferencesService.PreferredLanguage = SelectedLanguage;

        _localizationService.InitializeLanguage(SelectedLanguage);

        MessageBox.Show(
            _localizationService.GetString("Settings_Saved_Message"),
            "✓",
            MessageBoxButton.OK,
            MessageBoxImage.Information);
    }

    private List<LanguageOption> CreateLanguageOptions()
    {
        return
        [
            new LanguageOption("auto", GetLocalizedString("Settings_Language_Auto")),
            new LanguageOption("es-GT", GetLocalizedString("Spanish_Languages")),
            new LanguageOption("en-US", GetLocalizedString("English_Languages"))
        ];
    }

    private string GetLocalizedString(string key)
    {
        return _localizationService.GetString(key);
    }
}

public sealed record LanguageOption(string Code, string DisplayName);
