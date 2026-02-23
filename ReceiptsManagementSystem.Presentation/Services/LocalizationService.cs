using System.ComponentModel;
using System.Globalization;

namespace ReceiptsManagementSystem.Presentation.Services;

public sealed class LocalizationService : ILocalizationService
{
    public string AppTitle => Resources.Resources.Resources.AppTitle;
    public string AppSubtitle => Resources.Resources.Resources.AppSubtitle;
    public string NavReceipts => Resources.Resources.Resources.Nav_Receipts;
    public string NavNewReceipt => Resources.Resources.Resources.Nav_NewReceipt;
    public string NavExit => Resources.Resources.Resources.Nav_Exit;
    public string Version => Resources.Resources.Resources.Version;
    public string LanguageSpanish => Resources.Resources.Resources.Language_Spanish;
    public string LanguageEnglish => Resources.Resources.Resources.Language_English;

    public string CurrentLanguage => CultureInfo.CurrentUICulture.Name;

    public void ChangeLanguage(string cultureCode)
    {
        CultureInfo culture = new(cultureCode);
        CultureInfo.CurrentCulture = culture;
        CultureInfo.CurrentUICulture = culture;

        Resources.Resources.Resources.Culture = culture;

        OnPropertyChanged(string.Empty);
        OnPropertyChanged(nameof(CurrentLanguage));
    }

    public string CurrentLanguageCode =>
        CultureInfo.CurrentUICulture.TwoLetterISOLanguageName;

    public event PropertyChangedEventHandler? PropertyChanged;

    private void OnPropertyChanged(string? name)
        => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
}
