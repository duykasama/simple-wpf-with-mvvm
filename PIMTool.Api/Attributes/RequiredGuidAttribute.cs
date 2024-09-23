using System.ComponentModel.DataAnnotations;

namespace PIMTool.Api.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequiredGuidAttribute : ValidationAttribute
{
    public RequiredGuidAttribute(string errorMessage)
    {
        ErrorMessage = errorMessage;
    }

    public RequiredGuidAttribute()
    {
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    {
        if (value is null || (Guid)value == default)
        {
            return new ValidationResult(ErrorMessage ?? "Guid value is required");
        }
        return ValidationResult.Success;
    }
}
