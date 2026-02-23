using System;
using System.Globalization;
using System.Windows.Data;

namespace ReceiptsManagementSystem.Presentation.Converters;

public class LanguageSelectedConverter : IValueConverter
{

#pragma warning disable S2326 // Disable SonarCloud warning S2326: "Make method static"
    public object Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
        => IsSelected(value, parameter);

#pragma warning disable S2326 // Disable SonarCloud warning S2326: "Make method static"
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
