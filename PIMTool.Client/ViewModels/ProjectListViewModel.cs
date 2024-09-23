using CommunityToolkit.Mvvm.Messaging;
using PIMTool.Client.Constants;
using PIMTool.Client.Messages;
using PIMTool.Client.Models;
using PIMTool.Client.Services.Interfaces;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Input;

namespace PIMTool.Client.ViewModels;

public class ProjectListViewModel : ViewModelBase
{
    #region Search projects

    private string _searchKeyword = string.Empty;
    public string SearchKeyword
    {
        get => _searchKeyword;
        set
        {
            SetProperty(ref _searchKeyword, value);
            _searchCriteria.Keyword = value;
        }
    }

    private ProjectStatus? _selectedStatus = null;
    public ProjectStatus? SelectedStatus
    {
        get => _selectedStatus;
        set
        {
            SetProperty(ref _selectedStatus, value);
            _searchCriteria.Status = value;
        }
    }

    public IEnumerable<ProjectStatus> ProjectStatuses { get; private set; }

    public ICommand SearchProjectsCommand { get; set; }

    public ICommand ClearSearchCommand { get; set; }

    #endregion

    #region Paginate projects

    public ICommand PreviousPageCommand { get; }
    public ICommand NextPageCommand { get; }
    public ICommand NavigateCommand { get; }

    private const int PageSize = 10;
    private int _totalPages;
    public int TotalPages
    {
        get => _totalPages;
        set => SetProperty(ref _totalPages, value);
    }

    private bool _canGoPrev = false;
    public bool CanGoPrev
    {
        get => _canGoPrev;
        set => SetProperty(ref _canGoPrev, value);
    }

    private bool _canGoNext = false;
    public bool CanGoNext
    {
        get => _canGoNext;
        set => SetProperty(ref _canGoNext, value);
    }

    private int _currentPage;
    public int CurrentPage
    {
        get => _currentPage;
        set
        {
            SetProperty(ref _currentPage, value);
            UpdatePageNumbers();
        }
    }

    public ObservableCollection<PageNumber> PageNumbers { get; } = [];

    #endregion

    private IList<int> _selectedProjectIds = [];
    public IList<int> SelectedProjectIds
    {
        get => _selectedProjectIds;
        set => SetProperty(ref _selectedProjectIds, value);
    }

    public ICommand DeleteProjectCommand { get; set; }
    public ICommand DeleteMultipleProjectsCommand { get; set; }
    public ICommand NavigateToEditProjectCommand { get; set; }

    private readonly IProjectService _projectService;
    private readonly INavigationService _navigationService;
    private readonly SearchCriteria _searchCriteria;

    public ObservableCollection<SelectableProject> ProjectsList { get; set; } = [];

#pragma warning disable CS4014
    public ProjectListViewModel(IProjectService projectService, INavigationService navigationService, SearchCriteria searchCriteria)
    {
        RegisterMessages();
        _projectService = projectService;
        _navigationService = navigationService;
        _searchCriteria = searchCriteria;
        SearchKeyword = _searchCriteria.Keyword;
        SelectedStatus = _searchCriteria.Status;
        TotalPages = CurrentPage = 1;
        UpdatePageNumbers();

        SearchProjectsCommand = new CustomCommand((param) => SearchProjects(param));
        ClearSearchCommand = new CustomCommand((param) => ClearSearch(param));
        DeleteProjectCommand = new CustomCommand(DeleteProject);
        DeleteMultipleProjectsCommand = new CustomCommand(DeleteMultipleProjects);
        PreviousPageCommand = new CustomCommand(PreviousPage);
        NextPageCommand = new CustomCommand(NextPage);
        NavigateCommand = new CustomCommand(NavigateToPage);
        NavigateToEditProjectCommand = new CustomCommand(NaviateToEditProject);
        ProjectStatuses = GetAllProjectStatuses();

        PopulateProjectList();
    }
#pragma warning restore CS4014 

    private void RegisterMessages()
    {
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, (_, _) => TriggerStatusBindingChange());
        WeakReferenceMessenger.Default.Register<DeleteConfirmedMessage>(this, (_, message) => ConfirmDeleteProject(message));
        WeakReferenceMessenger.Default.Register<DeleteMultipleConfirmedMessage>(this, (_, _) => ConfirmDeleteMultipleProjects());
        WeakReferenceMessenger.Default.Register<ShouldUnregisterMessage>(this, UnregisterMessages);
    }

    private void UnregisterMessages(object _, ShouldUnregisterMessage message)
    {
        WeakReferenceMessenger.Default.Unregister<LanguageChangedMessage>(this);
        WeakReferenceMessenger.Default.Unregister<DeleteConfirmedMessage>(this);
        WeakReferenceMessenger.Default.Unregister<DeleteMultipleConfirmedMessage>(this);
    }

    private void HandleCollectionItemChanged(object? sender, PropertyChangedEventArgs e)
    {
        var arg = (IsSelectedChangedEventArgs)e;
        if (arg.IsSelected && !SelectedProjectIds.Contains(arg.Id))
        {
            SelectedProjectIds.Add(arg.Id);
            SelectedProjectIds = new List<int>(SelectedProjectIds);
        }
        else if (!arg.IsSelected && SelectedProjectIds.Contains(arg.Id))
        {
            SelectedProjectIds.Remove(arg.Id);
            SelectedProjectIds = new List<int>(SelectedProjectIds);
        }
    }

    private void TriggerStatusBindingChange()
    {
        var oldValue = SelectedStatus;
        var differentStatus = ProjectStatuses.FirstOrDefault(value => value != oldValue);
        SelectedStatus = differentStatus;
        SelectedStatus = oldValue;
    }

    private async Task PopulateProjectList()
    {
        var (projects, pageIndex, lastPage, _) = await _projectService.SearchProjectsWithPagination(SearchKeyword, SelectedStatus, pageSize: PageSize);
        CurrentPage = pageIndex;
        TotalPages = lastPage;
        CanGoPrev = CurrentPage > 1;
        CanGoNext = CurrentPage < lastPage;
        UpdatePageNumbers();
        ProjectsList.Clear();
        foreach (var project in projects)
        {
            project.PropertyChanged += HandleCollectionItemChanged;
            ProjectsList.Add(project);
        }
    }

    private async Task SearchProjects(object parameter)
    {
        var (projects, pageIndex, lastPage, _) = await _projectService.SearchProjectsWithPagination(SearchKeyword, SelectedStatus, pageIndex: CurrentPage, pageSize: PageSize);
        CurrentPage = pageIndex;
        TotalPages = lastPage;
        CanGoPrev = CurrentPage > 1;
        CanGoNext = CurrentPage < lastPage;
        UpdatePageNumbers();
        ProjectsList.Clear();
        foreach (var project in projects)
        {
            if (SelectedProjectIds.Contains(project.Id))
            {
                project.IsSelected = true;
            }
            project.PropertyChanged += HandleCollectionItemChanged;
            ProjectsList.Add(project);
        }
        UpdateSelectedProjectIds();
    }

    private void UpdateSelectedProjectIds()
    {
        SelectedProjectIds = SelectedProjectIds.Where(id => ProjectsList.Any(p => p.Id == id)).ToList();
    }

    private async Task ClearSearch(object parameter)
    {
        SearchKeyword = string.Empty;
        SelectedStatus = null;
        CurrentPage = 1;

        var (projects, _, lastPage, _) = await _projectService.GetPaginatedProjects(pageSize: PageSize);
        TotalPages = lastPage;
        CanGoPrev = CurrentPage > 1;
        CanGoNext = CurrentPage < lastPage;
        UpdatePageNumbers();
        ProjectsList.Clear();
        foreach (var project in projects)
        {
            if (SelectedProjectIds.Contains(project.Id))
            {
                project.IsSelected = true;
            }
            project.PropertyChanged += HandleCollectionItemChanged;
            ProjectsList.Add(project);
        }
        UpdateSelectedProjectIds();
    }

    private void DeleteMultipleProjects(object _)
    {
        if (ProjectsList.Any(p => p.Status != ProjectStatus.NEW && SelectedProjectIds.Contains(p.Id)))
        {
            var message = Application.Current.Resources[MultilingualKey.DeleteNewProjectError].ToString() ?? string.Empty;
            WeakReferenceMessenger.Default.Send(new ProjectStatusNotSatisfiedToDeleteMessage(message));
            return;
        }

        WeakReferenceMessenger.Default.Send(new ShowDeleteConfirmationMessage(null));
    }

    private async void ConfirmDeleteMultipleProjects()
    {
        var deleteSuccess = await _projectService.DeleteMultipleProjects(SelectedProjectIds);
        if (!deleteSuccess)
        {
            _navigationService.NavigateTo<ErrorPageViewModel>();
            return;
        }
        foreach (var id in SelectedProjectIds)
        {
            var project = ProjectsList.Single(p => p.Id == id);
            ProjectsList.Remove(project);
        }
        SelectedProjectIds = [];
        await SearchProjects(null!);
    }

    private void DeleteProject(object parameter)
    {
        if (parameter is not int id)
        {
            return;
        }

        var projectToDelete = ProjectsList.Where(p => p.Id == id).FirstOrDefault();
        if (projectToDelete == null)
        {
            return;
        }

        if (projectToDelete.Status != ProjectStatus.NEW)
        {
            var message = Application.Current.Resources[MultilingualKey.DeleteNewProjectError].ToString() ?? string.Empty;
            WeakReferenceMessenger.Default.Send(new ProjectStatusNotSatisfiedToDeleteMessage(message));
            return;
        }

        WeakReferenceMessenger.Default.Send(new ShowDeleteConfirmationMessage(id));
    }

    private async void ConfirmDeleteProject(DeleteConfirmedMessage message)
    {
        int id = message.Value;
        var deleteSuccess = await _projectService.DeleteProject(id);
        if (!deleteSuccess)
        {
            _navigationService.NavigateTo<ErrorPageViewModel>();
            return;
        }
        var project = ProjectsList.Single(p => p.Id == id);
        ProjectsList.Remove(project);
        SelectedProjectIds.Remove(id);
        OnPropertyChanged(nameof(SelectedProjectIds));
        await SearchProjects(null!);
    }

    private static IEnumerable<ProjectStatus> GetAllProjectStatuses()
    {
        var statuses = Enum.GetValues(typeof(ProjectStatus)).Cast<ProjectStatus>().ToList();
        return statuses;
    }

    private void UpdatePageNumbers()
    {
        PageNumbers.Clear();
        int visiblePages = 5;
        int start, end;

        if (TotalPages <= visiblePages)
        {
            start = 1;
            end = TotalPages;
        }
        else
        {
            int halfVisible = visiblePages / 2;

            start = Math.Max(1, CurrentPage - halfVisible);
            end = Math.Min(TotalPages, start + visiblePages - 1);

            if (end - start + 1 < visiblePages)
            {
                start = Math.Max(1, end - visiblePages + 1);
            }
        }

        for (int i = start; i <= end; i++)
        {
            PageNumbers.Add(new PageNumber
            {
                Number = i,
                IsSelected = i == CurrentPage,
            });
        }
    }

#pragma warning disable CS4014
    private void PreviousPage(object parameter)
    {
        if (!CanGoPrev)
        {
            return;
        }
        CurrentPage--;
        SearchProjects(null!);
    }

    private void NextPage(object parameter)
    {
        if (!CanGoNext)
        {
            return;
        }
        CurrentPage++;
        SearchProjects(null!);
    }

    private void NavigateToPage(object parameter)
    {
        if (parameter is int pageIndex && pageIndex > 0 && pageIndex <= TotalPages && pageIndex != CurrentPage)
        {
            CurrentPage = pageIndex;
            SearchProjects(null!);
        }
    }
#pragma warning restore CS4014

    private void NaviateToEditProject(object parameter)
    {
        _navigationService.NavigateTo<CreateProjectViewModel>();
        WeakReferenceMessenger.Default.Send(new ShouldEditProjectMessage((int)parameter));
    }
}
