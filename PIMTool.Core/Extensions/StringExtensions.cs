namespace PIMTool.Core.Extensions;

public static class StringExtensions
{
    public static bool IsNullOrEmpty(this string? value)
    {
        return string.IsNullOrWhiteSpace(value);
    }

    public static bool IsNotNullOrEmpty(this string? value)
    {
        return !string.IsNullOrEmpty(value);
    }

    public static string WithParameters(this string contentTemplate, params object[] parameters)
    {
        return $"{contentTemplate}:{string.Join(',', parameters)}";
    }
}
