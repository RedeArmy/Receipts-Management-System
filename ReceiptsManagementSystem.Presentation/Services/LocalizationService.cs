using System.ComponentModel;
using System.Globalization;
using System.Resources;

namespace ReceiptsManagementSystem.Presentation.Services;

public sealed class LocalizationService : ILocalizationService
{
    private static LocalizationService? _instance;
    private CultureInfo _currentCulture;
    private readonly ResourceManager _resourceManager;
    private readonly CultureInfo _originalSystemCulture;

    public static LocalizationService Instance => _instance ??= new LocalizationService();

    public event PropertyChangedEventHandler? PropertyChanged;

    private LocalizationService()
    {
        _resourceManager = Resources.Resources.Resources.ResourceManager;
        _originalSystemCulture = CultureInfo.CurrentUICulture;
        _currentCulture = CultureInfo.CurrentUICulture;
    }

    public string AppTitle => Resources.Resources.Resources.AppTitle;
    public string AppSubtitle => Resources.Resources.Resources.AppSubtitle;
    public string NavReceipts => Resources.Resources.Resources.Nav_Receipts;
    public string NavNewReceipt => Resources.Resources.Resources.Nav_NewReceipt;
    public string NavSettings => Resources.Resources.Resources.Nav_Settings;
    public string NavExit => Resources.Resources.Resources.Nav_Exit;
    public string Version => Resources.Resources.Resources.Version;
    public string LanguageSpanish => Resources.Resources.Resources.Language_Spanish;
    public string LanguageEnglish => Resources.Resources.Resources.Language_English;

    public string ReceiptCustomerName => Resources.Resources.Resources.Receipt_CustomerName;
    public string ReceiptAmount => Resources.Resources.Resources.Receipt_Amount;
    public string ReceiptCurrency => Resources.Resources.Resources.Receipt_Currency;
    public string ReceiptDescription => Resources.Resources.Resources.Receipt_Description;
    public string ReceiptPaymentMethod => Resources.Resources.Resources.Receipt_PaymentMethod;
    public string ReceiptCheckNumber => Resources.Resources.Resources.Receipt_CheckNumber;
    public string ReceiptAccountNumber => Resources.Resources.Resources.Receipt_AccountNumber;
    public string ReceiptBank => Resources.Resources.Resources.Receipt_Bank;
    public string ReceiptCustomerSignature => Resources.Resources.Resources.Receipt_CustomerSignature;
    public string ReceiptReceiverName => Resources.Resources.Resources.Receipt_ReceiverName;
    public string ButtonSave => Resources.Resources.Resources.Button_Save;
    public string ButtonClear => Resources.Resources.Resources.Button_Clear;
    public string PaymentMethodCash => Resources.Resources.Resources.PaymentMethod_Cash;
    public string PaymentMethodCheck => Resources.Resources.Resources.PaymentMethod_Check;
    public string PaymentMethodTransfer => Resources.Resources.Resources.PaymentMethod_Transfer;
    public string ClientInfo => Resources.Resources.Resources.Client_Info;
    public string ReceiptDetails => Resources.Resources.Resources.Receipt_Details;
    public string PaymentMethod => Resources.Resources.Resources.Payment_Method;
    public string Signatures => Resources.Resources.Resources.Signatures;
    public string ReceiptDate => Resources.Resources.Resources.Receipt_Date;
    public string ReceiptDateDescription => Resources.Resources.Resources.Receipt_Date_Description;

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

            CultureInfo.CurrentCulture = value;
            CultureInfo.CurrentUICulture = value;
            Resources.Resources.Resources.Culture = value;

            OnPropertyChanged(string.Empty);

            OnPropertyChanged(nameof(CurrentCulture));
            OnPropertyChanged(nameof(CurrentLanguage));
            OnPropertyChanged(nameof(CurrentLanguageCode));
            OnPropertyChanged(nameof(AppTitle));
            OnPropertyChanged(nameof(AppSubtitle));
            OnPropertyChanged(nameof(NavReceipts));
            OnPropertyChanged(nameof(NavNewReceipt));
            OnPropertyChanged(nameof(NavSettings));
            OnPropertyChanged(nameof(NavExit));
            OnPropertyChanged(nameof(Version));
            OnPropertyChanged(nameof(LanguageSpanish));
            OnPropertyChanged(nameof(LanguageEnglish));
            OnPropertyChanged(nameof(ReceiptCustomerName));
            OnPropertyChanged(nameof(ReceiptAmount));
            OnPropertyChanged(nameof(ReceiptCurrency));
            OnPropertyChanged(nameof(ReceiptDescription));
            OnPropertyChanged(nameof(ReceiptPaymentMethod));
            OnPropertyChanged(nameof(ReceiptCheckNumber));
            OnPropertyChanged(nameof(ReceiptAccountNumber));
            OnPropertyChanged(nameof(ReceiptBank));
            OnPropertyChanged(nameof(ReceiptCustomerSignature));
            OnPropertyChanged(nameof(ReceiptReceiverName));
            OnPropertyChanged(nameof(ButtonSave));
            OnPropertyChanged(nameof(ButtonClear));
            OnPropertyChanged(nameof(PaymentMethodCash));
            OnPropertyChanged(nameof(PaymentMethodCheck));
            OnPropertyChanged(nameof(PaymentMethodTransfer));
            OnPropertyChanged(nameof(ClientInfo));
            OnPropertyChanged(nameof(ReceiptDetails));
            OnPropertyChanged(nameof(PaymentMethod));
            OnPropertyChanged(nameof(Signatures));
            OnPropertyChanged(nameof(ReceiptDate));
            OnPropertyChanged(nameof(ReceiptDateDescription));
        }
    }

    public string CurrentLanguageCode => _currentCulture.TwoLetterISOLanguageName;

    public string CurrentLanguage => _currentCulture.Name;

    public void InitializeLanguage(string preference)
    {
        if (preference == "auto")
        {
            CurrentCulture = DetectSystemLanguage();
        }
        else
        {
            try
            {
                CurrentCulture = new CultureInfo(preference);
            }
            catch (CultureNotFoundException)
            {
                CurrentCulture = DetectSystemLanguage();
            }
        }
    }

    public void ChangeLanguage(string cultureCode)
    {
        try
        {
            CurrentCulture = new CultureInfo(cultureCode);
        }
        catch (CultureNotFoundException)
        {
            // Ignore invalid culture codes silently
        }
    }

    public string GetString(string key, params object[] args)
    {
        var value = _resourceManager.GetString(key, _currentCulture);

        if (string.IsNullOrEmpty(value))
        {
            return $"[Missing: {key}]";
        }

        return args.Length > 0 ? string.Format(value, args) : value;
    }

    private CultureInfo DetectSystemLanguage()
    {
        var systemCulture = _originalSystemCulture;

        if (systemCulture.TwoLetterISOLanguageName == "es")
        {
            return new CultureInfo("es-GT");
        }

        return systemCulture.TwoLetterISOLanguageName == "en" ? new CultureInfo("en-US") : new CultureInfo("es-GT");
    }

    private void OnPropertyChanged(string? propertyName)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }

}
