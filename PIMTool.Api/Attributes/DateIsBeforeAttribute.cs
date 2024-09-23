using PIMTool.Core.Extensions;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Api.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class DateIsBeforeAttribute : ValidationAttribute
{
    private string DateToCompare { get; set; }

    public DateIsBeforeAttribute(string dateToCompare, string errorMessage)
    {
        DateToCompare = dateToCompare;
        ErrorMessage = errorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var validationFailureMessage = ErrorMessage ?? "Date validation failed";
        var dateString = validationContext.ObjectType.GetProperty(DateToCompare)?.GetValue(validationContext.ObjectInstance, null);
        if (dateString == null || dateString.ToString().IsNullOrEmpty()) // If the date to compare is not provided, this validation always succeeds
        {
            return ValidationResult.Success;
        }
        var dateToCompare = (DateTime)dateString;
        if (value == null || dateToCompare == default)
        {
            return ValidationResult.Success;
        }
        var contextDate = DateTime.Parse(value.ToString()!);

        return (contextDate < dateToCompare) ? ValidationResult.Success : new ValidationResult(validationFailureMessage);
    }
}
