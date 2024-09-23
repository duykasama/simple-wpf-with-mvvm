using FluentResults;
using PIMTool.Core.Constants;
using PIMTool.Core.Entities;
using PIMTool.Core.Enums;
using PIMTool.Core.Extensions;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Repositories.Interfaces;
using PIMTool.Core.Services.Interfaces;
using Serilog;

namespace PIMTool.Core.Services.Implementations;

public class ProjectService : IProjectService
{
    #region Private members

    private readonly IGroupRepository _groupRepository;
    private readonly IEmployeeRepository _employeeRepository;
    private readonly IProjectRepository _projectRepository;
    private readonly IUnitOfWork _unitOfWork;

    #endregion

    public ProjectService(IGroupRepository groupRepository, IEmployeeRepository employeeRepository, IProjectRepository projectRepository, IUnitOfWork unitOfWork)
    {
        _groupRepository = groupRepository;
        _employeeRepository = employeeRepository;
        _projectRepository = projectRepository;
        _unitOfWork = unitOfWork;
    }

    #region Business methods

    public Result<Project> CreateProject(Project project, IEnumerable<string> visas)
    {
        Func<Result<Project>> createAction = () =>
        {
            var result = new Result<Project>();
            var employees = new List<Employee>();

            ValidateRequiredFields(project, ref result);

            if (result.IsFailed)
            {
                var reasonWithMetadata = result.Reasons.FirstOrDefault();
                reasonWithMetadata ??= new Error(LocalizedMessages.ValidationError);
                reasonWithMetadata.Metadata.TryAdd(nameof(ErrorType), ErrorType.ValidationError);
                return result;
            }

            var allEmployees = _employeeRepository.GetAll();
            foreach (var visa in visas)
            {
                var employee = allEmployees.FirstOrDefault(e => string.Equals(e.Visa, visa, StringComparison.OrdinalIgnoreCase));

                if (employee == null)
                {
                    result.WithError(LocalizedMessages.EmployeeVisaNotFoundDynamic.WithParameters(visa));
                }
                else
                {
                    employees.Add(employee);
                }
            }

            if (result.IsFailed)
            {
                var reasonWithMetadata = result.Reasons.FirstOrDefault();
                reasonWithMetadata ??= new Error(LocalizedMessages.DataNotFound);
                reasonWithMetadata.Metadata.TryAdd(nameof(ErrorType), ErrorType.ResourceMissingError);
                return result;
            }

            var existingProject = _projectRepository.GetProjectByNumber(project.ProjectNumber);
            if (existingProject != null)
            {
                var conflictProjectNumberError = new Error(LocalizedMessages.ProjectNumberAlreadyExistsDynamic.WithParameters(project.ProjectNumber));
                conflictProjectNumberError.Metadata.TryAdd(nameof(ErrorType), ErrorType.BusinessError);
                result.WithError(conflictProjectNumberError);
                return result;
            }

            var group = _groupRepository.Get(project.GroupId);
            if (group == null)
            {
                var reasonWithMetadata = result.Reasons.FirstOrDefault();
                if (reasonWithMetadata == null)
                {
                    reasonWithMetadata = new Error(LocalizedMessages.GroupNotFound);
                    result.WithReason(reasonWithMetadata);
                }
                reasonWithMetadata.Metadata.TryAdd(nameof(ErrorType), ErrorType.ResourceMissingError);
            }

            if (result.IsFailed)
            {
                return result;
            }

            project.Employees = employees;
            _projectRepository.Add(project);
            _unitOfWork.Complete();

            return Result.Ok(project).WithSuccess(LocalizedMessages.ProjectCreated);
        };

        return ExecuteInsideErrorHandler(createAction);
    }

    public Result DeleteProject(int projectId)
    {
        Func<Result> deleteAction = () =>
        {
            var result = new Result();
            Project? project = _projectRepository.Get(projectId);
            if (project == null)
            {
                var projectNotFoundError = new Error(LocalizedMessages.ProjectNotFoundDynamic);
                projectNotFoundError.Metadata.TryAdd(nameof(ErrorType), ErrorType.ResourceMissingError);
                result.WithError(projectNotFoundError);
                return result;
            }

            if (project.Status != ProjectStatus.NEW)
            {
                var projectStatusNotSatisfiedError = new Error(LocalizedMessages.NotNewProject);
                projectStatusNotSatisfiedError.Metadata.TryAdd(nameof(ErrorType), ErrorType.BusinessError);
                result.WithError(projectStatusNotSatisfiedError);
                return result;
            }

            _projectRepository.Delete(project);
            _unitOfWork.Complete();

            return result.WithSuccess(LocalizedMessages.ProjectDeleted);
        };

        return ExecuteInsideErrorHandler(deleteAction);
    }

    public Result DeleteManyProjects(int[] ids)
    {
        Func<Result> deleteAction = () =>
        {
            var result = new Result();
            foreach (var id in ids)
            {
                Project? project = _projectRepository.Get(id);
                if (project == null)
                {
                    var projectNotFoundError = new Error(LocalizedMessages.ProjectNotFoundDynamic.WithParameters(id));
                    projectNotFoundError.Metadata.TryAdd(nameof(ErrorType), ErrorType.ResourceMissingError);
                    result.WithError(projectNotFoundError);
                }
                else if (project.Status != ProjectStatus.NEW)
                {
                    var projectStatusNotSatisfiedError = new Error(LocalizedMessages.NotNewProject);
                    projectStatusNotSatisfiedError.Metadata.TryAdd(nameof(ErrorType), ErrorType.BusinessError);
                    result.WithError(projectStatusNotSatisfiedError);
                }
                else
                {
                    _projectRepository.Delete(project);
                }
            }

            if (result.IsFailed)
            {
                return result;
            }

            _unitOfWork.Complete();

            return result.WithSuccess(LocalizedMessages.ProjectDeleted);
        };

        return ExecuteInsideErrorHandler(deleteAction);
    }

    public Result<(IEnumerable<Project> projects, int pageIndex, int total)> GetPaginatedProjects(int pageIndex, int pageSize)
    {
        Func<Result<(IEnumerable<Project> projects, int pageIndex, int total)>> getAction = () =>
        {
            (IEnumerable<Project>, int, int) result;

            pageIndex = Math.Max(pageIndex, 1);
            pageSize = Math.Max(pageSize, 1);
            var skip = (pageIndex - 1) * pageSize;

            IEnumerable<Project> projects = _projectRepository.GetInterval(skip, pageSize);
            int projectsCount = _projectRepository.CountAll();

            result.Item1 = projects;
            result.Item2 = pageIndex;
            result.Item3 = projectsCount;

            return Result.Ok(result);
        };

        return ExecuteInsideErrorHandler(getAction);
    }

    public Result<Project> GetProjectDetail(int projectId)
    {
        Func<Result<Project>> getAction = () =>
        {
            Project? project = _projectRepository.GetProjectById(projectId);
            if (project == null)
            {
                return Result.Fail(LocalizedMessages.ProjectNotFoundDynamic.WithParameters(projectId));
            }

            return Result.Ok(project);
        };

        return ExecuteInsideErrorHandler(getAction);
    }

    public Result UpdateProject(int projectId, Project project, IEnumerable<string> visas)
    {
        Func<Result> updateAction = () =>
        {
            var result = new Result();
            var employees = new List<Employee>();

            ValidateRequiredFields(project, ref result);

            if (result.IsFailed)
            {
                var reasonWithMetadata = result.Reasons.FirstOrDefault();
                reasonWithMetadata ??= new Error(LocalizedMessages.ValidationError);
                reasonWithMetadata.Metadata.TryAdd(nameof(ErrorType), ErrorType.ValidationError);
                return result;
            }

            var allEmployees = _employeeRepository.GetAll();
            foreach (var visa in visas)
            {
                var employee = allEmployees.FirstOrDefault(e => string.Equals(e.Visa, visa, StringComparison.OrdinalIgnoreCase));

                if (employee == null)
                {
                    result.WithError(LocalizedMessages.EmployeeVisaNotFoundDynamic.WithParameters(visa));
                }
                else
                {
                    employees.Add(employee);
                }
            }

            if (result.IsFailed)
            {
                var reasonWithMetadata = result.Reasons.FirstOrDefault();
                reasonWithMetadata ??= new Error(LocalizedMessages.EmployeeNotFound);
                reasonWithMetadata.Metadata.TryAdd(nameof(ErrorType), ErrorType.ResourceMissingError);
                return result;
            }

            Project? existingProject = _projectRepository.Get(projectId);
            if (existingProject == null)
            {
                var reasonWithMetadata = result.Reasons.FirstOrDefault();
                if (reasonWithMetadata == null)
                {
                    reasonWithMetadata = new Error(LocalizedMessages.ProjectNotFoundDynamic.WithParameters(projectId));
                    result.WithReason(reasonWithMetadata);
                }
                reasonWithMetadata.Metadata.TryAdd(nameof(ErrorType), ErrorType.ResourceMissingError);
                return result;
            }

            if (project.Version != existingProject.Version)
            {
                var reasonWithMetadata = result.Reasons.FirstOrDefault();
                if (reasonWithMetadata == null)
                {
                    reasonWithMetadata = new Error(LocalizedMessages.ProjectHasBeenModified);
                    result.WithReason(reasonWithMetadata);
                }
                reasonWithMetadata.Metadata.TryAdd(nameof(ErrorType), ErrorType.BusinessError);
                return result;
            }

            var group = _groupRepository.Get(project.GroupId);
            if (group == null)
            {
                var reasonWithMetadata = result.Reasons.FirstOrDefault();
                if (reasonWithMetadata == null)
                {
                    reasonWithMetadata = new Error(LocalizedMessages.GroupNotFound);
                    result.WithReason(reasonWithMetadata);
                }
                reasonWithMetadata.Metadata.TryAdd(nameof(ErrorType), ErrorType.ResourceMissingError);
                return result;
            }

            existingProject.Name = project.Name;
            existingProject.Customer = project.Customer;
            existingProject.Status = project.Status;
            existingProject.StartDate = project.StartDate;
            existingProject.EndDate = project.EndDate;
            existingProject.GroupId = project.GroupId;
            existingProject.Employees = employees;

            _projectRepository.Update(existingProject);
            _unitOfWork.Complete();

            result.WithSuccess(LocalizedMessages.ProjectUpdated);

            return result;
        };

        return ExecuteInsideErrorHandler(updateAction);
    }

    public Result<(IEnumerable<Project> projects, int pageIndex, int total)> SearchProjects(string keyword, ProjectStatus status, int pageIndex, int pageSize)
    {
        Func<Result<(IEnumerable<Project> projects, int pageIndex, int total)>> getAction = () =>
        {
            (IEnumerable<Project>, int, int) result;

            IEnumerable<Project> projects = _projectRepository.GetAll();
            var filteredProjects = projects.Where(p =>
                    p.ProjectNumber.ToString().Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Name.Contains(keyword, StringComparison.OrdinalIgnoreCase) ||
                    p.Customer.Contains(keyword, StringComparison.OrdinalIgnoreCase));
            if (status != default)
            {
                filteredProjects = filteredProjects.Where(p => p.Status == status);
            }

            pageIndex = Math.Max(pageIndex, 1);
            pageSize = Math.Max(pageSize, 1);
            var paginatedProjects = filteredProjects
                .OrderBy(p => p.ProjectNumber)
                .Skip((pageIndex - 1) * pageSize)
                .Take(pageSize);

            result.Item1 = paginatedProjects;
            result.Item2 = pageIndex;
            result.Item3 = filteredProjects.Count();

            return Result.Ok(result);
        };

        return ExecuteInsideErrorHandler(getAction);
    }

    #endregion

    #region Error handlers

    private static Result<T> ExecuteInsideErrorHandler<T>(Func<Result<T>> action)
    {
        try
        {
            var result = action.Invoke();
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return Result.Fail(ex.Message);
        }
    }

    private static Result ExecuteInsideErrorHandler(Func<Result> action)
    {
        try
        {
            var result = action.Invoke();
            return result;
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return Result.Fail(ex.Message);
        }
    }

    #endregion

    #region Validations

    private static void ValidateRequiredFields(Project project, ref Result<Project> result)
    {
        if (string.IsNullOrEmpty(project.Name))
        {
            result.WithError(LocalizedMessages.MissingProjectName);
        }

        if (string.IsNullOrEmpty(project.Customer))
        {
            result.WithError(LocalizedMessages.MissingCustomer);
        }

        if (project.ProjectNumber < 0 || project.ProjectNumber > 9999)
        {
            result.WithError(LocalizedMessages.InvalidProjectNumber);
        }

        if (project.StartDate > project.EndDate)
        {
            result.WithError(LocalizedMessages.InvalidStartDate);
        }
    }

    private static void ValidateRequiredFields(Project project, ref Result result)
    {
        if (string.IsNullOrEmpty(project.Name))
        {
            result.WithError(LocalizedMessages.MissingProjectName);
        }

        if (string.IsNullOrEmpty(project.Customer))
        {
            result.WithError(LocalizedMessages.MissingCustomer);
        }

        if (project.ProjectNumber < 0 || project.ProjectNumber > 9999)
        {
            result.WithError(LocalizedMessages.InvalidProjectNumber);
        }

        if (project.StartDate > project.EndDate)
        {
            result.WithError(LocalizedMessages.InvalidStartDate);
        }
    }

    #endregion
}
