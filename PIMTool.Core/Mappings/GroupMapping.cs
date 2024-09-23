using NHibernate.Mapping.ByCode;
using NHibernate.Mapping.ByCode.Conformist;
using PIMTool.Core.Entities;

namespace PIMTool.Core.Mappings;

public class GroupMapping : ClassMapping<Group>
{
    public GroupMapping()
    {
        Table("ProjectGroup");

        Id(
            g => g.Id,
            mapper =>
            {
                mapper.Generator(Generators.HighLow);
            }
        );

        Property(
            g => g.GroupLeaderId,
            mapper =>
            {
                mapper.NotNullable(false);
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

        OneToOne(
            g => g.GroupLeader,
            mapper =>
            {
                mapper.Cascade(Cascade.None);
            }
        );

        Set(
            g => g.Projects,
            mapper =>
            {
                mapper.Cascade(Cascade.Persist);
                mapper.Inverse(true);
            },
            relation => relation.OneToMany(m => m.Class(typeof(Project)))
        );
    }
}
