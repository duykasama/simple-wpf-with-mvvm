using PIMTool.Client.Models;
using PIMTool.Client.Services.Interfaces;

namespace PIMTool.Client.ViewModels;

public class AppSidebarViewModel : ViewModelBase
{
    private INavigationService _navigationService;

    public INavigationService NavigationService
    {
        get => _navigationService;
        set
        {
            if (_navigationService != value)
            {
                _navigationService = value;
            }
        }
    }

    public CustomCommand NavigateCommand { get; set; }

    public AppSidebarViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;
        NavigateCommand = new CustomCommand(HandleNavigate);
    }

    private void HandleNavigate(object parameter)
    {
        var pageType = parameter as Type;
        ArgumentNullException.ThrowIfNull(pageType, nameof(pageType));

        _navigationService.NavigateTo(pageType);
    }

}
