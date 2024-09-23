namespace PIMTool.Core.Entities;

public class Group : BaseEntity
{
    public virtual int GroupLeaderId { get; set; }

    public virtual Employee? GroupLeader { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = Array.Empty<Project>();
}
