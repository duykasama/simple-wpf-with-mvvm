using PIMTool.Client.Models;
using PIMTool.Client.Models.Api;
using PIMTool.Client.Repositories.Interfaces;
using PIMTool.Client.Services.Interfaces;
using Serilog;

namespace PIMTool.Client.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly IProjectRepository _projectRepository;

    public ProjectService(IProjectRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public async Task<(bool, object?)> CreateProject(CreateProjectRequest project)
    {
        try
        {
            var result = await _projectRepository.CreateProject(project);
            if (result.IsSuccess)
            {
                return (true, null);
            }

            if (result.Data is not ApiError error)
            {
                return (false, null);
            }

            var errorDetail = new ErrorDetail();
            if (error.Errors.Any())
            {
                var errors = error.Errors.Select(e => e.ToString()!);
                var firstErrorMessage = errors.First();
                if (firstErrorMessage.Contains("VISA", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorDetail.ErrorPath = nameof(CreateProjectRequest.Visas);
                    var visas = errors
                        .Select(msg => msg.Split(' ').ToList())
                        .Where(words => words.Count > 4)
                        .Select(words => $"{words[3]}")
                        .ToList();
                    var visaList = string.Join(", ", visas);
                    var combinedMessage = visaList;

                    errorDetail.Message = combinedMessage;
                }
                else if (firstErrorMessage.Contains("NUMBER", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorDetail.ErrorPath = nameof(CreateProjectRequest.ProjectNumber);
                }
            }

            return (false, errorDetail);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return (false, new ErrorDetail());
        }
    }

    public async Task<bool> DeleteMultipleProjects(IList<int> selectedProjectIds)
    {
        try
        {
            var deleteSuccess = await _projectRepository.DeleteMultipleProjects(selectedProjectIds);
            return deleteSuccess;
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return false;
        }
    }

    public async Task<bool> DeleteProject(int id)
    {
        try
        {
            var deleteSuccess = await _projectRepository.DeleteProjectById(id);

            return deleteSuccess;
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return false;
        }
    }

    public async Task<(IEnumerable<SelectableProject>, int, int, bool)> GetPaginatedProjects(int pageSize, int pageIndex)
    {
        try
        {
            var result = await _projectRepository.GetProjects(pageSize, pageIndex);
            var items = result?.Data?.Items ?? [];
            var projects = items.Select(ConvertToSelectableProject);
            var currentPage = result?.Data?.PageIndex ?? 1;
            var lastPage = result?.Data?.LastPage ?? 1;
            var isLastPage = result?.Data?.IsLastPage ?? true;
            return (projects, currentPage, lastPage, isLastPage);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return ([], 1, 1, true);
        }
    }

    public async Task<(Project?, object?)> GetProjectDetails(int id)
    {
        try
        {
            var result = await _projectRepository.GetProjectById(id);
            if (result.Data is ApiResponse<Project> response)
            {
                return (response.Data, null);
            }

            var errorDetail = new ErrorDetail();
            if (result.Data is ApiError error)
            {
                errorDetail.Message = error.Detail;
            }

            errorDetail.Message = "An error occurred";
            return (null, errorDetail);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return (null, null);
        }
    }

    public async Task<(IEnumerable<SelectableProject>, int, int, bool)> SearchProjectsWithPagination(string keyword, ProjectStatus? status, int pageSize = 10, int pageIndex = 1)
    {
        try
        {
            var result = await _projectRepository.SearchProjects(keyword, (int?)status, pageSize, pageIndex);
            var items = result?.Data?.Items ?? [];
            var projects = items.Select(ConvertToSelectableProject);
            var currentPage = result?.Data?.PageIndex ?? 1;
            var lastPage = result?.Data?.LastPage ?? 1;
            var isLastPage = result?.Data?.IsLastPage ?? true;
            return (projects, currentPage, lastPage, isLastPage);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return ([], 1, 1, true);
        }
    }

    public async Task<(bool, object?)> UpdateProject(UpdateProjectRequest project)
    {
        try
        {
            var result = await _projectRepository.UpdateProject(project);
            if (result.IsSuccess)
            {
                return (true, null);
            }

            if (result.Data is not ApiError error)
            {
                return (false, null);
            }

            var errorDetail = new ErrorDetail();
            if (error.Errors.Any())
            {
                var errors = error.Errors.Select(e => e.ToString()!);
                var firstErrorMessage = errors.First();
                if (firstErrorMessage.Contains("VISA", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorDetail.ErrorPath = nameof(CreateProjectRequest.Visas);
                    var visas = errors
                        .Select(msg => msg.Split(' ').ToList())
                        .Where(words => words.Count > 4)
                        .Select(words => $"{words[3]}")
                        .ToList();
                    var visaList = string.Join(", ", visas);
                    var combinedMessage = visaList;
                    //var combinedMessage = $"Employees with visas {visaList} do not exist.";

                    errorDetail.Message = combinedMessage;
                }
                else if (firstErrorMessage.Contains("NUMBER", StringComparison.CurrentCultureIgnoreCase))
                {
                    errorDetail.ErrorPath = nameof(CreateProjectRequest.ProjectNumber);
                }
            }

            return (false, errorDetail);
        }
        catch (Exception ex)
        {
            Log.Error(ex, ex.Message);
            return (false, new ErrorDetail());
        }
    }

    private SelectableProject ConvertToSelectableProject(Project project)
    {
        return new SelectableProject
        {
            Id = project.Id,
            Name = project.Name,
            Customer = project.Customer,
            GroupId = project.GroupId,
            Members = project.Members,
            Status = project.Status,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            ProjectNumber = project.ProjectNumber,
            IsSelected = false
        };
    }
}
