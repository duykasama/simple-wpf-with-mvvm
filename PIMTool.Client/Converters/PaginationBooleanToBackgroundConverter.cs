using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PIMTool.Client.Converters;

public class PaginationBooleanToBackgroundConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        bool isSelected = (bool)value;
        return isSelected ? Brushes.LightGray : Brushes.Transparent;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
