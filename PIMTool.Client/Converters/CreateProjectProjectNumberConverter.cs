using System.Globalization;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class CreateProjectProjectNumberConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        return value;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        var valueAsString = value as string;
        if (string.IsNullOrEmpty(valueAsString))
        {
            return null!;
        }

        return value;
    }
}
