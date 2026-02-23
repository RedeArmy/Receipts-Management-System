using System;
using System.Globalization;
using System.Windows.Data;

namespace ReceiptsManagementSystem.Presentation.Converters;

public class LanguageSelectedConverter : IValueConverter
{

    /// <summary>
    /// Converts the source value to the target value.
    /// Although this method could be static, it must remain an instance method
    /// to comply with <see cref="IValueConverter"/> for WPF/XAML usage.
    /// The warning about "make static" can be safely ignored.
    /// </summary>
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => IsSelected(value, parameter);

    /// <summary>
    /// Conversion back is not implemented. Could be static, but is kept as an instance
    /// method to satisfy <see cref="IValueConverter"/> requirements in WPF/XAML.
    /// The warning about "make static" is intentional and safe.
    /// </summary>
    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
        => throw new NotImplementedException();

    private static bool IsSelected(object? value, object? parameter)
    {
        if (value is string selected && parameter is string lang)
        {
            return selected == lang;
        }

        return false;
    }
}
