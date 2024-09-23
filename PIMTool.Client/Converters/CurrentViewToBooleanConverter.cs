using System.Globalization;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class CurrentViewToBooleanConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value == null)
        {
            return false;
        }
        var valueAsType = value.GetType();
        var parameterAsType = parameter as Type;

        return valueAsType == parameterAsType;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
