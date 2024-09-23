using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class SearchBarTextToTextBlockVisibility : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string valueAsString)
        {
            bool empty = string.IsNullOrEmpty(valueAsString);
            return empty ? Visibility.Visible : Visibility.Collapsed;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
