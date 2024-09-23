using PIMTool.Client.Models;
using PIMTool.Client.Models.Api;

namespace PIMTool.Client.Services.Interfaces;

public interface IProjectService
{
    Task<(bool, object?)> CreateProject(CreateProjectRequest project);
    Task<(bool, object?)> UpdateProject(UpdateProjectRequest project);
    Task<bool> DeleteProject(int id);
    Task<(Project?, object?)> GetProjectDetails(int id);
    Task<(IEnumerable<SelectableProject>, int, int, bool)> GetPaginatedProjects(int pageSize = 10, int pageIndex = 1);
    Task<(IEnumerable<SelectableProject>, int, int, bool)> SearchProjectsWithPagination(string keyword, ProjectStatus? status, int pageSize = 10, int pageIndex = 1);
    Task<bool> DeleteMultipleProjects(IList<int> selectedProjectIds);
}
