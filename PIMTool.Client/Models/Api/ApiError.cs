namespace PIMTool.Client.Models.Api;

public class ApiError
{
    public string Type { get; set; }

    public string Title { get; set; }

    public int Status { get; set; }

    public string Detail { get; set; }

    public IEnumerable<object> Errors { get; set; }

    public string TraceId { get; set; }
}
