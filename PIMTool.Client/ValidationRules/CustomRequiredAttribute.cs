using System.ComponentModel.DataAnnotations;
using System.Windows;

namespace PIMTool.Client.ValidationRules;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public sealed class CustomRequiredAttribute : ValidationAttribute
{
    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value == null || value == default)
        {
            var message = Application.Current.Resources[ErrorMessage].ToString();
            return new ValidationResult(ErrorMessage);
        }

        return ValidationResult.Success;
    }
}
