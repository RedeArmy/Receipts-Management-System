using System.ComponentModel;

namespace ReceiptsManagementSystem.Presentation.Services;

public interface ILocalizationService : INotifyPropertyChanged
{
    string AppTitle { get; }
    string AppSubtitle { get; }
    string NavReceipts { get; }
    string NavNewReceipt { get; }
    string NavExit { get; }
    string Version { get; }
    string LanguageSpanish { get; }
    string LanguageEnglish { get; }

    string CurrentLanguage { get; }

    string CurrentLanguageCode { get; }

    void ChangeLanguage(string cultureCode);
}
