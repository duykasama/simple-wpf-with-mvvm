using PIMTool.Client.Models;
using PIMTool.Client.Services.Interfaces;
using System.Windows.Input;

namespace PIMTool.Client.ViewModels;

public class ErrorPageViewModel : ViewModelBase
{
    private readonly INavigationService _navigationService;

    public ICommand RedirectToSearchCommand { get; set; }

    public ErrorPageViewModel(INavigationService navigationService)
    {
        _navigationService = navigationService;

        RedirectToSearchCommand = new CustomCommand(RedirectToSearch);
    }

    private void RedirectToSearch(object parameter)
    {
        _navigationService.NavigateTo<ProjectListViewModel>();
    }
}
