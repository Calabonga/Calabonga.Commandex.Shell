using System.Globalization;
using System.Windows.Data;

namespace Calabonga.Commandex.Shell.CustomControls;

public class ListViewNameConverter : IValueConverter
{
    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not null && parameter is not null)
        {
            if (value.ToString()!.Contains(parameter.ToString()!))
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => value is not null && bool.Parse(value.ToString()!);
}