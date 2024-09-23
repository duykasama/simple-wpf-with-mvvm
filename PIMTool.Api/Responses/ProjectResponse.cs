using PIMTool.Core.Enums;

namespace PIMTool.Api.Responses;

public class ProjectResponse
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int ProjectNumber { get; set; }

    public string Customer { get; set; } = null!;

    public ProjectStatus Status { get; set; }

    public DateOnly StartDate { get; set; }

    public DateOnly? EndDate { get; set; }

    public int GroupId { get; set; }
}
