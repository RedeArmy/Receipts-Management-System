using System.ComponentModel;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace ReceiptsManagementSystem.Presentation.Services;

public sealed class LocalizationService : INotifyPropertyChanged
{
    private static LocalizationService? _instance;
    private CultureInfo _currentCulture;

    public static LocalizationService Instance => _instance ??= new();

    public event PropertyChangedEventHandler? PropertyChanged;

    private LocalizationService()
    {
        _currentCulture = new("es-GT");
        Resources.Resources.Resources.Culture = _currentCulture;
    }

    public CultureInfo CurrentCulture
    {
        get => _currentCulture;
        private set
        {
            if (_currentCulture.Equals(value))
            {
                return;
            }

            _currentCulture = value;
            Resources.Resources.Resources.Culture = value;

            OnPropertyChanged(null);
        }
    }

    public void ChangeLanguage(string cultureCode)
    {
        CurrentCulture = new(cultureCode);
    }

    public string AppTitle => Resources.Resources.Resources.AppTitle;
    public string AppSubtitle => Resources.Resources.Resources.AppSubtitle;
    public string NavReceipts => Resources.Resources.Resources.Nav_Receipts;
    public string NavNewReceipt => Resources.Resources.Resources.Nav_NewReceipt;
    public string NavExit => Resources.Resources.Resources.Nav_Exit;
    public string Version => Resources.Resources.Resources.Version;
    public string LanguageSpanish => Resources.Resources.Resources.Language_Spanish;
    public string LanguageEnglish => Resources.Resources.Resources.Language_English;

    private void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new(propertyName));
    }
}
