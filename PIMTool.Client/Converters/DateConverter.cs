using System.Globalization;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class DateConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is DateTime date)
        {
            return date.ToString("dd.MM.yyyy");
        }
        else if (value is null || (value is string stringValue && string.IsNullOrEmpty(stringValue)))
        {
            return string.Empty;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
