using System.ComponentModel;
using System.Globalization;
using System.Reflection;
using System.Windows;
using System.Windows.Data;

namespace Warehouse.Converters;

public class EnumDescriptionConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter,
        CultureInfo culture)
    {
        if (value is not Enum enumValue)
            return string.Empty;
        return enumValue.GetType().GetField(enumValue.ToString())
            ?.GetCustomAttribute<DescriptionAttribute>()?.Description
            ?? enumValue.ToString();
    }

    public object ConvertBack(object value, Type targetType, object parameter,
        CultureInfo culture) => DependencyProperty.UnsetValue;
}
