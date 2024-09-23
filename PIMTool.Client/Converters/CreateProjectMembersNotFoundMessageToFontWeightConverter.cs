using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Data;

namespace PIMTool.Client.Converters;

public partial class CreateProjectMembersNotFoundMessageToFontWeightConverter : IValueConverter
{
    [GeneratedRegex("^[a-zA-Z, ]+$")]
    private static partial Regex MembersRegex();

    public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
    {
        if (value is ValidationResult validationResult)
        {
            var messagePatternMatched = MembersRegex().IsMatch(validationResult.ErrorMessage ?? string.Empty);

            return messagePatternMatched ? FontWeights.Bold : FontWeights.SemiBold;
        }
        else if (value is string)
        {
            return FontWeights.Bold;
        }

        return Binding.DoNothing;
    }

    public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
    {
        throw new NotImplementedException();
    }
}
