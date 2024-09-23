using PIMTool.Client.Models;
using PIMTool.Client.Models.Api;

namespace PIMTool.Client.Repositories.Interfaces;

public interface IProjectRepository
{
    Task<ApiResponse<object>> CreateProject(CreateProjectRequest project);
    Task<bool> DeleteMultipleProjects(IList<int> selectedProjectIds);
    Task<bool> DeleteProjectById(int id);
    Task<ApiResponse<object?>> GetProjectById(int id);
    Task<ApiResponse<PaginationResponse<Project>>> GetProjects(int pageSize, int pageIndex);
    Task<ApiResponse<PaginationResponse<Project>>> SearchProjects(string keyword, int? status, int pageSize, int pageIndex);
    Task<ApiResponse<object>> UpdateProject(UpdateProjectRequest project);
}
