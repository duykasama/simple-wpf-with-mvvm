using PIMTool.Core.Enums;

namespace PIMTool.Core.Entities;

public class Project : BaseEntity
{
    public virtual string Name { get; set; } = null!;

    public virtual int ProjectNumber { get; set; }

    public virtual string Customer { get; set; } = null!;

    public virtual ProjectStatus Status { get; set; }

    public virtual DateTime StartDate { get; set; }

    public virtual DateTime? EndDate { get; set; }

    public virtual int GroupId { get; set; }

    public virtual ICollection<Employee> Employees { get; set; } = Array.Empty<Employee>();
}
