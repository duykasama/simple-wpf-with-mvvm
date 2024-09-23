using FluentResults;
using PIMTool.Core.Entities;
using PIMTool.Core.Enums;

namespace PIMTool.Core.Services.Interfaces;

public interface IProjectService
{
    Result<(IEnumerable<Project> projects, int pageIndex, int total)> GetPaginatedProjects(int pageIndex, int pageSize);
    Result<(IEnumerable<Project> projects, int pageIndex, int total)> SearchProjects(string keyword, ProjectStatus status, int pageIndex, int pageSize);
    Result<Project> GetProjectDetail(int projectId);
    Result<Project> CreateProject(Project project, IEnumerable<string> visas);
    Result UpdateProject(int projectId, Project project, IEnumerable<string> visas);
    Result DeleteProject(int projectId);
    Result DeleteManyProjects(int[] ids);
}
