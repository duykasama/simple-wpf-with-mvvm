using CommunityToolkit.Mvvm.Messaging;
using PIMTool.Client.Attributes;
using PIMTool.Client.Constants;
using PIMTool.Client.Messages;
using PIMTool.Client.Models;
using PIMTool.Client.Models.Api;
using PIMTool.Client.Repositories.Interfaces;
using PIMTool.Client.Services.Interfaces;
using PIMTool.Client.ValidationRules;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Windows;
using System.Windows.Input;

namespace PIMTool.Client.ViewModels;
public class CreateProjectViewModel : ViewModelBase
{
    # region Project data

    private int? _projectNumber;
    [CustomRequired(ErrorMessage = MultilingualKey.RequiredProjectNumber)]
    [Range(1, 9999, ErrorMessage = MultilingualKey.ProjectNumberOutOfRange)]
    [CustomValidation(typeof(CreateProjectViewModel), nameof(ValidateProjectNumberDuplication))]
    public int? ProjectNumber
    {
        get => _projectNumber;
        set
        {
            _isProjectNumberDuplicate = false;
            SetProperty(ref _projectNumber, value, true);
        }
    }

    private string _projectName = string.Empty;
    [Required(ErrorMessage = MultilingualKey.RequiredProjectName)]
    [MaxLength(50, ErrorMessage = MultilingualKey.ProjectNameTooLong)]
    public string ProjectName
    {
        get => _projectName;
        set => SetProperty(ref _projectName, value, true);
    }

    private string _customer = string.Empty;
    [Required(ErrorMessage = MultilingualKey.RequiredCustomer)]
    [MaxLength(ErrorMessage = MultilingualKey.CustomerNameTooLong)]
    public string Customer
    {
        get => _customer;
        set => SetProperty(ref _customer, value, true);
    }

    private Group? _group;
    [Required(ErrorMessage = MultilingualKey.RequiredGroup)]
    public Group? Group
    {
        get => _group;
        set => SetProperty(ref _group, value, true);
    }

    private string _members = string.Empty;
    [RegularExpression("^[A-Za-z, ]+$", ErrorMessage = MultilingualKey.WrongMembersFormat)]
    public string Members
    {
        get => _members;
        set => SetProperty(ref _members, value, true);
    }

    private ProjectStatus _status = ProjectStatus.NEW;
    [Range(1, int.MaxValue, ErrorMessage = MultilingualKey.RequiredStatus)]
    public ProjectStatus Status
    {
        get => _status;
        set => SetProperty(ref _status, value, true);
    }

    private DateTime? _startDate;
    [Required(ErrorMessage = MultilingualKey.RequiredStartDate)]
    public DateTime? StartDate
    {
        get => _startDate;
        set
        {
            SetProperty(ref _startDate, value, true);
            ValidateProperty(EndDate, nameof(EndDate));
        }
    }

    private DateTime? _endDate;
    [DateIsAfter(nameof(StartDate), MultilingualKey.InvalidEndDate)]
    public DateTime? EndDate
    {
        get => _endDate;
        set => SetProperty(ref _endDate, value, true);
    }

    private int _version;

    #endregion

    #region Commands

    public ICommand CreateOrUpdateProjectCommand { get; set; }

    public ICommand CancelCommand { get; set; }

    public ICommand HideGeneralErrorTextCommand { get; set; }

    #endregion

    #region Validation members

    private string _membersError = string.Empty;

    public string MembersError
    {
        get => _membersError;
        set => SetProperty(ref _membersError, value);
    }

    private bool _showGeneralErrorText = false;

    public bool ShowGeneralErrorText
    {
        get => _showGeneralErrorText && HasErrors;
        set => SetProperty(ref _showGeneralErrorText, value, true);
    }

    private bool _isProjectNumberDuplicate = false;

    public static ValidationResult ValidateProjectNumberDuplication(string _, ValidationContext context)
    {
        var instance = (CreateProjectViewModel)context.ObjectInstance;
        bool isDuplicate = instance._isProjectNumberDuplicate;

        if (isDuplicate)
        {
            var message = Application.Current.Resources[MultilingualKey.DuplicateProjectNumber].ToString();
            return new(message);
        }

        return ValidationResult.Success!;
    }

    #endregion

    #region Services

    private readonly IGroupRepository _groupRepository;

    private readonly IProjectService _projectService;

    private readonly INavigationService _navigationService;

    #endregion

    private bool _isEditing = false;
    public bool IsEditing
    {
        get => _isEditing;
        set => SetProperty(ref _isEditing, value);
    }

    private int _projectId;

    public ObservableCollection<Group> Groups { get; set; } = [];

    public ObservableCollection<ProjectStatus> Statuses { get; set; } = [];

#pragma warning disable CS4014
    public CreateProjectViewModel(IGroupRepository groupRepository, IProjectService projectService, INavigationService navigationService)
    {
        WeakReferenceMessenger.Default.Register<ShouldEditProjectMessage>(this, (recipient, message) => HandleMessage(recipient, message));
        WeakReferenceMessenger.Default.Register<LanguageChangedMessage>(this, HandleLanguageChange);
        _groupRepository = groupRepository;
        _projectService = projectService;
        _navigationService = navigationService;
        CreateOrUpdateProjectCommand = new CustomCommand((parameter) => CreateOrUpdateProject(parameter));
        CancelCommand = new CustomCommand(Cancel);
        HideGeneralErrorTextCommand = new CustomCommand(HideGeneralErrorText);

        PopulateComboBoxes();
    }
#pragma warning restore CS4014

    private async Task HandleMessage(object _, ShouldEditProjectMessage message)
    {
        var projectId = _projectId = message.ProjectId;
        var (project, error) = await _projectService.GetProjectDetails(projectId);
        if (project == null)
        {
            WeakReferenceMessenger.Default.Send(new ProjectNotFoundMessage(projectId));
            return;
        }

        IsEditing = true;
        ProjectNumber = project.ProjectNumber;
        ProjectName = project.Name;
        Customer = project.Customer;
        Group = Groups.FirstOrDefault(g => g.Id == project.GroupId);
        Members = string.Join(",", project.Members.Select(m => m.Visa));
        Status = project.Status;
        StartDate = project.StartDate;
        EndDate = project.EndDate;
        _version = project.Version;
    }

    private void HandleLanguageChange(object _, LanguageChangedMessage message)
    {
        ReTriggerValidationIfNeeded(ProjectNumber, nameof(ProjectNumber));
        ReTriggerValidationIfNeeded(ProjectName, nameof(ProjectName));
        ReTriggerValidationIfNeeded(Customer, nameof(Customer));
        ReTriggerValidationIfNeeded(Group, nameof(Group));
        ReTriggerValidationIfNeeded(Members, nameof(Members));
        ReTriggerValidationIfNeeded(Status, nameof(Status));
        ReTriggerValidationIfNeeded(StartDate, nameof(StartDate));
        ReTriggerValidationIfNeeded(EndDate, nameof(EndDate));
        TriggerStatusBindingChange();
    }

    private void TriggerStatusBindingChange()
    {
        var oldValue = Status;
        var differentStatus = Statuses.FirstOrDefault(value => value != oldValue);
        Status = differentStatus;
        Status = oldValue;
    }

    private void ReTriggerValidationIfNeeded(object? value, string propertyName)
    {
        if (GetErrors(propertyName).Any())
        {
            ValidateProperty(value, propertyName);
        };
    }

    private async Task CreateOrUpdateProject(object _)
    {
        ValidateAllProperties();
        if (HasErrors)
        {
            ShowGeneralErrorText = true;
            return;
        }


        (bool success, object? error) = (false, null);
        if (IsEditing)
        {
            var project = new UpdateProjectRequest
            {
                Id = _projectId,
                Name = ProjectName,
                Customer = Customer,
                GroupId = Group?.Id ?? 0,
                Visas = TransformMembersField(Members),
                Status = (int)Status,
                StartDate = StartDate ?? DateTime.Now,
                EndDate = EndDate,
                Version = _version,
            };
            (success, error) = await _projectService.UpdateProject(project);
        }
        else
        {
            var project = new CreateProjectRequest
            {
                ProjectNumber = ProjectNumber ?? 0,
                Name = ProjectName,
                Customer = Customer,
                GroupId = Group?.Id ?? 0,
                Visas = TransformMembersField(Members),
                Status = (int)Status,
                StartDate = StartDate ?? DateTime.Now,
                EndDate = EndDate,
            };
            (success, error) = await _projectService.CreateProject(project);
        }

        if (success)
        {
            _navigationService.NavigateTo<ProjectListViewModel>();
        }
        else
        {
            if (error is ErrorDetail errorDetail)
            {
                switch (errorDetail.ErrorPath)
                {
                    case nameof(CreateProjectRequest.ProjectNumber):
                        _isProjectNumberDuplicate = true;
                        break;
                    case nameof(CreateProjectRequest.Visas):
                        MembersError = string.Empty; // change error message to ensure message is displayed
                        MembersError = errorDetail.Message;
                        break;
                    default:
                        _navigationService.NavigateTo<ErrorPageViewModel>();
                        return;
                }
            }

            ValidateAllProperties();
        }
    }

    private IEnumerable<string> TransformMembersField(string membersString)
    {
        var parts = membersString.Split(',');
        var result = parts.Select(x => x.Trim()).Where(x => !string.IsNullOrEmpty(x)).Distinct();
        return result;
    }

    private void Cancel(object parameter)
    {
        _navigationService.NavigateTo<ProjectListViewModel>();
    }

    private void HideGeneralErrorText(object parameter)
    {
        ShowGeneralErrorText = false;
    }

    private async Task PopulateComboBoxes()
    {
        IEnumerable<Group> retrievedGroups = await _groupRepository.GetAllGroups();
        ProjectStatus[] validStatuses = Enum.GetValues(typeof(ProjectStatus))
            .Cast<ProjectStatus>()
            .ToArray();

        Groups.Clear();
        foreach (var group in retrievedGroups)
        {
            Groups.Add(group);
        }

        Statuses.Clear();
        foreach (var status in validStatuses)
        {
            Statuses.Add(status);
        }
    }
}
