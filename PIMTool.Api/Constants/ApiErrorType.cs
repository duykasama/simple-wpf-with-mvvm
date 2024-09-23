namespace PIMTool.Api.Constants;

public static class ApiErrorType
{
    public const string BadRequest = "https://tools.ietf.org/html/rfc9110#section-15.5.1";
    public const string Unauthorized = "https://tools.ietf.org/html/rfc9110#section-15.5.2";
    public const string NotFound = "https://tools.ietf.org/html/rfc9110#section-15.5.5";
    public const string Conflict = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.5.10";
    public const string InternalServerError = "https://tools.ietf.org/html/rfc9110#section-15.6.1";
    public const string NotImplemented = "https://datatracker.ietf.org/doc/html/rfc9110#section-15.6.2";
}
