using NHibernate;
using NHibernate.Cfg;
using NHibernate.Cfg.MappingSchema;
using NHibernate.Mapping.ByCode;
using NHibernate.Tool.hbm2ddl;
using PIMTool.Core.Entities;
using PIMTool.Core.Pattern.Interfaces;
using Serilog;
using System.Reflection;
using Environment = NHibernate.Cfg.Environment;

namespace PIMTool.Core.Helpers;

public static class SessionFactoryHelper
{
    public static ISessionFactory InitializeSessionFactory(Type databaseDialect, Type databaseDriver, IConnectionStringProvider connectionStringProvider)
    {
        var configuration = new Configuration();
        configuration
            .SetProperty(Environment.Dialect, databaseDialect.AssemblyQualifiedName)
            .SetProperty(Environment.ConnectionDriver, databaseDriver.AssemblyQualifiedName)
            .SetProperty(Environment.ConnectionString, connectionStringProvider.GetConnectionString());

        configuration.AddMapping(GetMapping<BaseEntity>());
        configuration.DataBaseIntegration(db => db.LogSqlInConsole = true);

        var sessionFactory = configuration.BuildSessionFactory();
        CreateTablesIfNeeded(sessionFactory, configuration);

        return sessionFactory;
    }

    private static HbmMapping GetMapping<T>() where T : class
    {
        var mapper = new ModelMapper();
        mapper.AddMappings(Assembly.GetAssembly(typeof(T))?.GetExportedTypes());
        var mapping = mapper.CompileMappingForAllExplicitlyAddedEntities();
        return mapping;
    }

    private static void CreateTablesIfNeeded(ISessionFactory sessionFactory, Configuration configuration)
    {
        using var session = sessionFactory.OpenSession();
        try
        {
            var sqlQuery = session.CreateSQLQuery(@"
                    SELECT TABLE_NAME 
                    FROM INFORMATION_SCHEMA.TABLES 
                    WHERE TABLE_NAME = :tableName");

            sqlQuery.SetParameter("tableName", nameof(Project));

            var result = sqlQuery.UniqueResult();
            if (result != null)
            {
                return;
            }

            new SchemaExport(configuration).Execute(true, true, false);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
        }
    }
}
