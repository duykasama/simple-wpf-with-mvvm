namespace PIMTool.Tests.Services.Fixtures.Models;

public class ExpectedCreateProjectResult
{
    public bool IsSuccess { get; set; }
    public bool IsFailed { get; set; }
    public string Message { get; set; } = null!;
}
