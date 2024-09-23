using System.ComponentModel.DataAnnotations;

namespace PIMTool.Client.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class DateIsAfterAttribute : ValidationAttribute
{
    private string DateToCompare { get; set; }

    public DateIsAfterAttribute(string dateToCompare, string errorMessage)
    {
        DateToCompare = dateToCompare;
        ErrorMessage = errorMessage;
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        var validationFailureMessage = ErrorMessage ?? "Date validation failed";
        var dateString = validationContext.ObjectType.GetProperty(DateToCompare)?.GetValue(validationContext.ObjectInstance, null);
        if (dateString == null || string.IsNullOrEmpty(dateString.ToString())) // If the date to compare is not provided, this validation always succeeds
        {
            return ValidationResult.Success;
        }
        var dateToCompare = (DateTime)dateString;
        if (value == null || dateToCompare == default)
        {
            return ValidationResult.Success;
        }
        var contextDate = DateTime.Parse(value.ToString()!);

        return (contextDate > dateToCompare) ? ValidationResult.Success : new ValidationResult(validationFailureMessage);
    }
}
