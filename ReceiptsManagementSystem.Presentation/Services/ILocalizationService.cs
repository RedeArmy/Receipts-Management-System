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
    string ReceiptCustomerName { get; }
    string ReceiptAmount { get; }
    string ReceiptCurrency { get; }
    string ReceiptDescription { get; }
    string ReceiptPaymentMethod { get; }
    string ReceiptCheckNumber { get; }
    string ReceiptAccountNumber { get; }
    string ReceiptBank { get; }
    string ReceiptCustomerSignature { get; }
    string ReceiptReceiverName { get; }
    string ButtonSave { get; }
    string ButtonClear { get; }
    string PaymentMethodCash { get; }
    string PaymentMethodCheck { get; }
    string PaymentMethodTransfer { get; }
    string ClientInfo { get; }
    string ReceiptDetails { get; }
    string PaymentMethod { get; }
    string Signatures { get; }
    string ReceiptDate { get; }
    string ReceiptDateDescription { get; }

    CultureInfo CurrentCulture { get; }
    string CurrentLanguage { get; }
    string CurrentLanguageCode { get; }

    void InitializeLanguage(string preference);
    void ChangeLanguage(string cultureCode);
    string GetString(string key, params object[] args);
}
