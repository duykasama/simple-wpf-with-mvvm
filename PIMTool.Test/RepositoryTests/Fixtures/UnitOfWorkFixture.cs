using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using PIMTool.Core.Entities;
using PIMTool.Core.Pattern.Interfaces;
using System.Reflection;
using Environment = NHibernate.Cfg.Environment;

namespace PIMTool.Tests.RepositoryTests.Fixtures;

internal class UnitOfWorkFixture
{
    private readonly ISessionFactory _sessionFactory;

    internal UnitOfWorkFixture()
    {
        _sessionFactory = CreateSessionFactory();
    }

    private static HbmMapping GetMapping<T>()
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(Assembly.GetAssembly(typeof(T))?.GetExportedTypes());
        var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        return mapping;
    }

    private static ISessionFactory CreateSessionFactory()
    {
        const string ConnectionString = "Server=localhost,1434;Database=PIMTool_Test;Uid=sa;Pwd=StrongPassword123@;TrustServerCertificate=True;";
        var databaseDialect = typeof(MsSql2012Dialect);
        var databaseDriver = typeof(SqlClientDriver);
        var configuration = new Configuration();
        configuration
            .SetProperty(Environment.Dialect, databaseDialect.AssemblyQualifiedName)
            .SetProperty(Environment.ConnectionDriver, databaseDriver.AssemblyQualifiedName)
            .SetProperty(Environment.ConnectionString, ConnectionString);

        configuration.AddMapping(GetMapping<BaseEntity>());
        configuration.DataBaseIntegration(db => db.LogSqlInConsole = true);

        new SchemaExport(configuration).Execute(true, true, false);
        return configuration.BuildSessionFactory();
    }

    public IUnitOfWork CreateUnitOfWork() => new Core.Pattern.Implementations.UnitOfWork(_sessionFactory);
}
