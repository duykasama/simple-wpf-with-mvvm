using System.Text.Json.Serialization;

namespace PIMTool.Client.Models.Api;

public class UpdateProjectRequest
{
    [JsonIgnore]
    public int Id { get; set; }

    public int GroupId { get; set; }

    public string Name { get; set; } = null!;

    public string Customer { get; set; } = null!;

    public int Status { get; set; }

    public IEnumerable<string> Visas { get; set; } = [];

    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public int Version { get; set; }
}
