using System.Globalization;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class ProjectListBooleanToHeightConverter : IMultiValueConverter
{
    public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
    {
        if (values[0] is bool shouldHaveHeight && values[1] is double actualHeight)
        {
            return shouldHaveHeight ? actualHeight : 0;
        }

        return Binding.DoNothing;
    }

    public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
