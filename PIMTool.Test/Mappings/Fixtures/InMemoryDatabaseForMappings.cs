using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Dialect;
using NHibernate.Driver;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using PIMTool.Core.Entities;
using System.Reflection;
using Environment = NHibernate.Cfg.Environment;

namespace PIMTool.Tests.Mappings.Fixtures;

public class InMemoryDatabaseForMappings : IDisposable
{
    protected Configuration _config;
    protected ISessionFactory _sessionFactory;

    public ISession Session { get; set; }

    public InMemoryDatabaseForMappings()
    {
        _config = new Configuration()
            .SetProperty(Environment.ReleaseConnections, "on_close")
            .SetProperty(Environment.Dialect, typeof(SQLiteDialect).AssemblyQualifiedName)
            .SetProperty(Environment.ConnectionDriver, typeof(SQLite20Driver).AssemblyQualifiedName)
            .SetProperty(Environment.ConnectionString, "Data Source=:memory:");
        //.SetProperty(Environment.Dialect, typeof(MsSql2012Dialect).AssemblyQualifiedName)
        //.SetProperty(Environment.ConnectionDriver, typeof(SqlClientDriver).AssemblyQualifiedName)
        //.SetProperty(Environment.ConnectionString, "Server=localhost,1434;Database=PIMTool;Uid=sa;Pwd=StrongPassword123@;TrustServerCertificate=True;");

        _config.AddMapping(GetMapping<Project>());

        _sessionFactory = _config.BuildSessionFactory();

        Session = _sessionFactory.OpenSession();
        new SchemaExport(_config).Execute(true, true, false, Session.Connection, Console.Out);
    }

    private static HbmMapping GetMapping<T>()
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(Assembly.GetAssembly(typeof(T))?.GetExportedTypes());
        var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        return mapping;
    }

    public void Dispose()
    {
        Session.Dispose();
        _sessionFactory.Dispose();
    }
}
