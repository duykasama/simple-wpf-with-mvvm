using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PIMTool.Client.Converters;

public class BooleanToForegroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isActive = (bool)value;
        var color = isActive ? Colors.DodgerBlue : Colors.Black;

        return new SolidColorBrush(color);
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
