using System.Globalization;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class ListToItemsCountConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is IEnumerable<int> items)
        {
            return items.Count();
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
