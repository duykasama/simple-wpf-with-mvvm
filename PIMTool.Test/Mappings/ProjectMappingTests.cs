using FluentAssertions;
using NHibernate;
using PIMTool.Core.Entities;
using PIMTool.Tests.Mappings.Fixtures;

namespace PIMTool.Tests.Mappings;

[Trait("Category", "MappingTest")]
public class ProjectMappingTests : IDisposable
{
    private readonly ISession _session;
    private readonly InMemoryDatabaseForMappings _databaseForMappings;

    public ProjectMappingTests()
    {
        _databaseForMappings = new InMemoryDatabaseForMappings();
        _session = _databaseForMappings.Session;
    }

    [Fact]
    public void MapPrimitivePropertiesTest()
    {
        object id = 0;
        using (var transaction = _session.BeginTransaction())
        {
            var employee = new Employee()
            {
                Visa = "NTH",
                FirstName = "Duy",
                LastName = "Nguyen",
                BirthDate = new DateTime(2000, 1, 1),
            };

            id = _session.Save(employee);
            transaction.Commit();
        }

        _session.Clear();

        using (var transaction = _session.BeginTransaction())
        {
            var employee = _session.Get<Employee>(id);

            #region Assertions

            employee.Visa
                .Should().Be("NTH");

            employee.FirstName
                .Should().Be("Duy");

            employee.LastName
                .Should().Be("Nguyen");

            employee.BirthDate.Year
                .Should().Be(2000);

            employee.BirthDate.Month
                .Should().Be(1);

            employee.BirthDate.Day
                .Should().Be(1);

            #endregion

            transaction.Commit();
        }
    }

    public void Dispose()
    {
        _session.Dispose();
        _databaseForMappings.Dispose();
    }
}
