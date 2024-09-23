using System.Globalization;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class BooleanToFontSizeConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isActive = (bool)value;

        return isActive ? 18 : 14;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
