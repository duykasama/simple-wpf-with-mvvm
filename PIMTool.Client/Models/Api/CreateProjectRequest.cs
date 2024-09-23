namespace PIMTool.Client.Models.Api;

public class CreateProjectRequest
{
    public int GroupId { get; set; }

    public int ProjectNumber { get; set; }

    public string Name { get; set; } = null!;

    public string Customer { get; set; } = null!;

    public int Status { get; set; }

    public IEnumerable<string> Visas { get; set; } = [];

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
