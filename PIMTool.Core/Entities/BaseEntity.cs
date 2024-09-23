namespace PIMTool.Core.Entities;

public abstract class BaseEntity
{
    public virtual int Id { get; set; }

    public virtual int Version { get; set; }
}
