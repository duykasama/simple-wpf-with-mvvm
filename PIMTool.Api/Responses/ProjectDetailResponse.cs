using PIMTool.Core.Enums;
using System.Text.Json.Serialization;

namespace PIMTool.Api.Responses;

public class ProjectDetailResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProjectNumber { get; set; }

    public string Customer { get; set; } = null!;

    public ProjectStatus Status { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int GroupId { get; set; }

    [JsonPropertyName("members")]
    public IEnumerable<EmployeeResponse> Employees { get; set; } = [];

    public int Version { get; set; }
}
