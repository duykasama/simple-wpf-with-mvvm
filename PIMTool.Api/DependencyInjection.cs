using AutoMapper;
using NHibernate.Dialect;
using NHibernate.Driver;
using PIMTool.Api.AutoMapper.MappingProfiles;
using PIMTool.Api.Extensions;
using PIMTool.Api.Logging;
using PIMTool.Core.Helpers;
using PIMTool.Core.Pattern.Implementations;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Repositories.Implementations;
using PIMTool.Core.Repositories.Interfaces;
using PIMTool.Core.Services.Implementations;
using PIMTool.Core.Services.Interfaces;
using Serilog;

namespace PIMTool.Api;

internal static class DependencyInjection
{
    public static IServiceCollection RegisterServices(this IServiceCollection services)
    {
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IProjectRepository, ProjectRepository>();
        services.AddScoped<IGroupRepository, GroupRepository>();
        services.AddScoped<IEmployeeRepository, EmployeeRepository>();
        services.AddScoped<IEmployeeService, EmployeeService>();
        services.AddScoped<IGroupService, GroupService>();
        services.AddScoped<IProjectService, ProjectService>();

        return services;
    }

    public static IServiceCollection RegisterNHibernate(this IServiceCollection services)
    {
        services
        .AddSingleton<IConnectionStringProvider, ConnectionStringProvider>()
        .AddSingleton(
            (serviceProvider) =>
            {
                var connectionStringProvider = serviceProvider.GetService<IConnectionStringProvider>();
                ArgumentNullException.ThrowIfNull(connectionStringProvider, nameof(connectionStringProvider));
                var sessionFactory = SessionFactoryHelper.InitializeSessionFactory(
                    databaseDialect: typeof(MsSql2012Dialect),
                    databaseDriver: typeof(SqlClientDriver),
                    connectionStringProvider
                );

                return sessionFactory;
            }
        );

        return services;
    }

    public static IServiceCollection RegisterAutoMapper(this IServiceCollection services)
    {
        services.AddSingleton(
            (_) =>
            {
                var mapperConfiguration = new MapperConfiguration(cfg =>
                {
                    cfg.AddAllProfiles<ProjectMappingProfile>();
                });

                return mapperConfiguration.CreateMapper();
            }
        );

        return services;
    }

    public static void AddSerilog(this WebApplicationBuilder builder)
    {
        builder.Logging.ClearProviders();
        Log.Logger = new LoggerConfiguration()
            .Enrich.With(new ThreadIdEnricher())
            .Enrich.WithProperty("Environment", builder.Environment.EnvironmentName)
            .WriteTo.Console(outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] (Thread: {ThreadId}) {Message}{NewLine}{Exception}")
            .WriteTo.File("log-.log", rollingInterval: RollingInterval.Day)
            .ReadFrom.Configuration(builder.Configuration)
            .CreateLogger();

        builder.Host.UseSerilog();
    }
}
