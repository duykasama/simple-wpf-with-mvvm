using PIMTool.Client.Models;
using System.Globalization;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class CreateProjectGroupConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is Group group)
        {
            return group.GroupLeaderName ?? string.Empty;
        };

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
