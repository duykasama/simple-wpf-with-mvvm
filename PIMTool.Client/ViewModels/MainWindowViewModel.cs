using PIMTool.Client.Services.Interfaces;

namespace PIMTool.Client.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private INavigationService _navigationService;
    public INavigationService NavigationService
    {
        get => _navigationService;
        set => SetProperty(ref _navigationService, value);
    }

    public AppHeaderViewModel AppHeader { get; set; }
    public AppSidebarViewModel AppSidebar { get; set; }

    public MainWindowViewModel(INavigationService navigationService, AppHeaderViewModel appHeader, AppSidebarViewModel appSidebar)
    {
        _navigationService = navigationService;
        AppHeader = appHeader;
        AppSidebar = appSidebar;
        _navigationService.NavigateTo<ProjectListViewModel>();
    }
}
