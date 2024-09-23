using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using NHibernate.Type;
using PIMTool.Core.Entities;
using PIMTool.Core.Enums;

namespace PIMTool.Core.Mappings;

public class ProjectMapping : ClassMapping<Project>
{
    public ProjectMapping()
    {
        Table(nameof(Project));

        Id(
            p => p.Id,
            mapper =>
            {
                mapper.Generator(Generators.Identity);
                mapper.Length(19);
            }
        );

        Property(
            p => p.Name,
            mapper =>
            {
                mapper.NotNullable(true);
                mapper.Length(50);
            }
        );

        Property(
            p => p.ProjectNumber,
            mapper =>
            {
                mapper.NotNullable(true);
                mapper.Unique(true);
            }
        );

        Property(
            p => p.Customer,
            mapper =>
            {
                mapper.NotNullable(true);
                mapper.Length(50);
            }
        );

        Property(
            p => p.Status,
            mapper =>
            {
                mapper.Type<EnumStringType<ProjectStatus>>();
                mapper.NotNullable(true);
                mapper.Length(3);
            }
        );

        Property(
            p => p.StartDate,
            mapper => mapper.NotNullable(true)
        );

        Property(
            p => p.EndDate,
            mapper => mapper.NotNullable(false)
        );

        Property(
            p => p.GroupId,
            mapper =>
            {
                mapper.NotNullable(true);
            }
        );

        Version(
            p => p.Version,
            mapper =>
            {
                mapper.Generated(VersionGeneration.Never);
                mapper.Insert(true);
            }
        );

        Set(
            p => p.Employees,
            mapper =>
            {
                mapper.Table("ProjectEmployee");
                mapper.Key(k => k.Column("ProjectId"));
                mapper.Cascade(Cascade.Persist);
            },
            relation => relation.ManyToMany(mtm =>
            {
                mtm.Class(typeof(Employee));
                mtm.Column("EmployeeId");
            })
        );
    }
}
