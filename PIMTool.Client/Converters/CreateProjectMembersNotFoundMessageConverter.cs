using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public partial class CreateProjectMembersNotFoundMessageConverter : IValueConverter
{
    [GeneratedRegex("^[a-zA-Z, ]+$")]
    private static partial Regex MembersRegex();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ValidationResult error && parameter is string)
        {
            return Application.Current.Resources[error.ErrorMessage];
        }

        if (value is string message)
        {
            if (parameter is string key)
            {
                return Application.Current.Resources[key];
            }

            return message;
        }

        return string.Empty;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
