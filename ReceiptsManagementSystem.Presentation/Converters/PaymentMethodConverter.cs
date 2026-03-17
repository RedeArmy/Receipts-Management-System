using System.Globalization;
using System.Windows.Data;
using ReceiptsManagementSystem.Domain.Enums;
using ReceiptsManagementSystem.Presentation.Services;

namespace ReceiptsManagementSystem.Presentation.Converters;

public class PaymentMethodConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values.Length == 0 || values[0] is not PaymentMethod paymentMethod)
        {
            return string.Empty;
        }

        var localization = LocalizationService.Instance;

        return paymentMethod switch
        {
            PaymentMethod.Cash => localization.PaymentMethodCash,
            PaymentMethod.Check => localization.PaymentMethodCheck,
            PaymentMethod.Transfer => localization.PaymentMethodTransfer,
            _ => paymentMethod.ToString()
        };
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
