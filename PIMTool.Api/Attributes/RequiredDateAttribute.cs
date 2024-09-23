using System.ComponentModel.DataAnnotations;

namespace PIMTool.Api.Attributes;

[AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
public sealed class RequiredDateAttribute : ValidationAttribute
{
    public override bool IsValid(object? value)
    {
        return (DateTime)(value ?? default(DateTime)) != default;
    }
}
