using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PIMTool.Core.Entities;

namespace PIMTool.Core.Mappings;

public class EmployeeMapping : ClassMapping<Employee>
{
    public EmployeeMapping()
    {
        Table(nameof(Employee));

        Id(
            e => e.Id,
            mapper =>
            {
                mapper.Generator(Generators.HighLow);
            }
        );

        Property(
            e => e.Visa,
            mapper =>
            {
                mapper.Length(50);
                mapper.NotNullable(true);
            }
        );

        Property(
            e => e.FirstName,
            mapper =>
            {
                mapper.Length(50);
                mapper.NotNullable(true);
            }
        );

        Property(
            e => e.LastName,
            mapper =>
            {
                mapper.Length(50);
                mapper.NotNullable(true);
            }
        );

        Property(
            e => e.BirthDate,
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
            p => p.Projects,
            mapper =>
            {
                mapper.Table("ProjectEmployee");
                mapper.Key(k => k.Column("EmployeeId"));
                mapper.Cascade(Cascade.Persist);
            },
            relation => relation.ManyToMany(mtm =>
            {
                mtm.Class(typeof(Project));
                mtm.Column("ProjectId");
            })
        );
    }
}
