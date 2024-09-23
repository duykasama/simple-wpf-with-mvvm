using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PIMTool.Client.Converters;

public class PaginationBooleanToForegoundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isSelected = (bool)value;
        var color = isSelected ? Colors.Gray : Colors.Blue;

        return new SolidColorBrush(color);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
