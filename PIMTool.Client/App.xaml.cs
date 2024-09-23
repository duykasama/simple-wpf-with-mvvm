using Microsoft.Extensions.DependencyInjection;
using PIMTool.Client.Constants;
using PIMTool.Client.Logging;
using PIMTool.Client.Models;
using PIMTool.Client.Repositories.Implementations;
using PIMTool.Client.Repositories.Interfaces;
using PIMTool.Client.Services.Implementations;
using PIMTool.Client.Services.Interfaces;
using PIMTool.Client.ViewModels;
using PIMTool.Client.Views;
using Serilog;
using System.Windows;

namespace PIMTool.Client;

/// <summary>
/// Interaction logic for App.xaml
/// </summary>
public partial class App : Application
{
    public IServiceProvider Services { get; }

    public App()
    {
        Services = ConfigureServices();
        Log.Logger = new LoggerConfiguration()
            .Enrich.With(new ThreadIdEnricher())
            .WriteTo.File("client-log-.log", rollingInterval: RollingInterval.Day, outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss.fff zzz} [{Level:u3}] (Thread: {ThreadId}) {Message}{NewLine}{Exception}")
            .CreateLogger();

        Log.Information("Application started.");
    }

    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);
        var mainWindow = Services.GetRequiredService<MainWindow>();
        mainWindow.Show();
    }

    private static ServiceProvider ConfigureServices()
    {
        var services = new ServiceCollection();

        services.AddSingleton<MainWindow>(provider => new MainWindow
        {
            DataContext = provider.GetRequiredService<MainWindowViewModel>()
        });

        services.AddSingleton<SearchCriteria>();

        #region ViewModels

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<AppHeaderViewModel>();
        services.AddSingleton<AppSidebarViewModel>();
        services.AddTransient<CreateProjectViewModel>();
        services.AddTransient<ProjectListViewModel>();
        services.AddTransient<ErrorPageViewModel>();

        #endregion

        #region Services

        services.AddSingleton<INavigationService, NavigationService>();
        services.AddSingleton<IProjectRepository, ProjectRepository>();
        services.AddSingleton<IProjectService, ProjectService>();
        services.AddSingleton<IGroupRepository, GroupRepository>();

        services.AddSingleton<Func<Type, ViewModelBase>>(provider =>
        {
            Func<Type, ViewModelBase> func = (viewModelType) => (ViewModelBase)provider.GetRequiredService(viewModelType);
            return func;
        });

        services.AddHttpClient(ApiClients.PIMTool.Name, opt =>
        {
            opt.BaseAddress = new Uri(ApiClients.PIMTool.Address);
        });

        #endregion

        return services.BuildServiceProvider();
    }

    protected override void OnExit(ExitEventArgs e)
    {
        Log.CloseAndFlush();
        base.OnExit(e);
    }
}
