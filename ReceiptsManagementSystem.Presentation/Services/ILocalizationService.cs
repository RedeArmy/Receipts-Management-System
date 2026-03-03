using System.ComponentModel;
using System.Globalization;

namespace ReceiptsManagementSystem.Presentation.Services;

public interface ILocalizationService : INotifyPropertyChanged
{
    string AppTitle { get; }
    string AppSubtitle { get; }
    string NavReceipts { get; }
    string NavNewReceipt { get; }
    string NavSettings { get; }
    string NavExit { get; }
    string Version { get; }
    string LanguageSpanish { get; }
    string LanguageEnglish { get; }

    CultureInfo CurrentCulture { get; }
    string CurrentLanguage { get; }
    string CurrentLanguageCode { get; }

    void InitializeLanguage(string preference);
    void ChangeLanguage(string cultureCode);
    string GetString(string key, params object[] args);
}
