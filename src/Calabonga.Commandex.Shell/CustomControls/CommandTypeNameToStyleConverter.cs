using Calabonga.Commandex.Shell.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace Calabonga.Commandex.Shell.CustomControls;

public class CommandTypeNameToStyleConverter : IValueConverter
{
    public Style CommandGroupStyle { get; set; } = null!;

    public Style CommandItemStyle { get; set; } = null!;

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value produced by the binding source.</param>
    /// <param name="targetType">The type of the binding target property.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            if (string.IsNullOrEmpty(stringValue))
            {
                return CommandItemStyle ?? throw new ArgumentNullException(nameof(CommandItemStyle));
            }

            if (stringValue == nameof(CommandGroup))
            {
                return CommandGroupStyle ?? throw new ArgumentNullException(nameof(CommandGroupStyle));
            }
        }

        return CommandItemStyle ?? throw new ArgumentNullException(nameof(CommandItemStyle));
    }

    /// <summary>Converts a value.</summary>
    /// <param name="value">The value that is produced by the binding target.</param>
    /// <param name="targetType">The type to convert to.</param>
    /// <param name="parameter">The converter parameter to use.</param>
    /// <param name="culture">The culture to use in the converter.</param>
    /// <returns>A converted value. If the method returns <see langword="null" />, the valid null value is used.</returns>
    public object? ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture) => throw new NotImplementedException();
}
