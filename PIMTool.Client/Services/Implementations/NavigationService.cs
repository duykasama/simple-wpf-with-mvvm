using CommunityToolkit.Mvvm.ComponentModel;
using PIMTool.Client.Services.Interfaces;
using PIMTool.Client.ViewModels;

namespace PIMTool.Client.Services.Implementations;

public sealed class NavigationService : ObservableObject, INavigationService
{
    private readonly Func<Type, ViewModelBase> _viewModelFactory;

    private ViewModelBase _currentView;
    public ViewModelBase CurrentView
    {
        get => _currentView;
        private set => SetProperty(ref _currentView, value);
    }

    public NavigationService(Func<Type, ViewModelBase> viewModelFactory)
    {
        _viewModelFactory = viewModelFactory;
    }

    public void NavigateTo<T>() where T : ViewModelBase
    {
        var newView = _viewModelFactory.Invoke(typeof(T));
        CurrentView = newView;
    }

    public void NavigateTo(Type pageType)
    {
        var newView = _viewModelFactory.Invoke(pageType);
        CurrentView = newView;
    }
}
