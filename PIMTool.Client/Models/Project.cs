namespace PIMTool.Client.Models;

public class Project : BaseModel
{
    public string Name { get; set; } = null!;

    public int ProjectNumber { get; set; }

    public string Customer { get; set; } = null!;

    public ProjectStatus Status { get; set; }

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int GroupId { get; set; }

    public IEnumerable<Employee> Members { get; set; } = [];
}
