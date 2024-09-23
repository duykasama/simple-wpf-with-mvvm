namespace PIMTool.Core.Entities;

public class Employee : BaseEntity
{
    public virtual string Visa { get; set; } = null!;

    public virtual string FirstName { get; set; } = null!;

    public virtual string LastName { get; set; } = null!;

    public virtual DateTime BirthDate { get; set; }

    public virtual Group? ProjectGroup { get; set; }

    public virtual ICollection<Project> Projects { get; set; } = Array.Empty<Project>();
}
