using PIMTool.Client.Constants;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public class LocalizedMessageConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ValidationResult error)
        {
            return Application.Current.Resources[error.ErrorMessage];
        }
        else if (value is string stringValue)
        {
            var head = Application.Current.Resources[MultilingualKey.MembersNotFound0];

            return $"{head} {stringValue}";
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
