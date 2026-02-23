using System;
using System.Globalization;
using System.Windows.Data;

namespace ReceiptsManagementSystem.Presentation.Converters;

public class LanguageSelectedConverter : IValueConverter
{
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string selected && parameter is string lang)
        {
            return selected == lang;
        }

        return false;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();
}
