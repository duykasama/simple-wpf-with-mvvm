using PIMTool.Client.ViewModels;

namespace PIMTool.Client.Services.Interfaces;

public interface INavigationService
{
    public ViewModelBase CurrentView { get; }
    void NavigateTo<T>() where T : ViewModelBase;
    void NavigateTo(Type pageType);
}
