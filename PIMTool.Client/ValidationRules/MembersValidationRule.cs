using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace PIMTool.Client.ValidationRules;

public partial class MembersValidationRule : ValidationRule
{
    public override ValidationResult Validate(object value, CultureInfo cultureInfo)
    {
        var valueAsString = value as string ?? string.Empty;
        if (string.IsNullOrWhiteSpace(valueAsString))
        {
            return ValidationResult.ValidResult;
        }
        else if (!MembersRegex().IsMatch(valueAsString))
        {
            return new ValidationResult(false, "Members field's format is not correct");
        };

        return ValidationResult.ValidResult;
    }

    [GeneratedRegex("^[A-Za-z]+(,[A-Za-z]+)*$")]
    private static partial Regex MembersRegex();
}
