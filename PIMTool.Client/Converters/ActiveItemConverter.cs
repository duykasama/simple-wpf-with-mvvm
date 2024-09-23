using System.Globalization;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class ActiveItemConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var isActive = (bool)value;

        return isActive ? 1 : 0.5;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
