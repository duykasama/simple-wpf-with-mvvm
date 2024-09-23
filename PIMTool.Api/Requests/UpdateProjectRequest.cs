using PIMTool.Api.Attributes;
using PIMTool.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Api.Requests;

public class UpdateProjectRequest
{
    [Range(0, int.MaxValue, ErrorMessage = "Group id is invalid")]
    public int GroupId { get; set; }

    [Required]
    [MaxLength(50)]
    public string Name { get; set; } = null!;

    [Required]
    [MaxLength(50)]
    public string Customer { get; set; } = null!;

    [Range(1, 4)]
    public ProjectStatus Status { get; set; }

    public IEnumerable<string> Visas { get; set; } = [];

    [RequiredDate]
    [DateIsBefore(nameof(EndDate), "End Date must be after Start Date")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    [Range(0, int.MaxValue)]
    public int Version { get; set; }
}
