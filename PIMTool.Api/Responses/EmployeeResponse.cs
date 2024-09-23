namespace PIMTool.Api.Responses;

public class EmployeeResponse
{
    public string Visa { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public DateOnly BirthDate { get; set; }
}
