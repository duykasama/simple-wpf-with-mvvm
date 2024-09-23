using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace PIMTool.Client.Converters;

public class ActiveLanguageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is string stringValue)
        {
            var color = Colors.Black;
            var currentCultureName = Thread.CurrentThread.CurrentCulture.Name;

            if ((stringValue == "EN" && currentCultureName == "en-US") || (stringValue == "FR" && currentCultureName == "fr-FR"))
            {
                color = Colors.DodgerBlue;
            }

            return new SolidColorBrush(color);
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
