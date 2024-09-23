using PIMTool.Api.Attributes;
using PIMTool.Core.Enums;
using System.ComponentModel.DataAnnotations;

namespace PIMTool.Api.Requests;

public class CreateProjectRequest
{
    [Range(0, int.MaxValue, ErrorMessage = "Group id is invalid")]
    public int GroupId { get; set; }

    [Range(typeof(int), "1", "9999")]
    public int ProjectNumber { get; set; }

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
    [DateIsBefore(nameof(EndDate), "Start Date must be before End Date")]
    public DateTime StartDate { get; set; }

    public DateTime? EndDate { get; set; }
}
