using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PIMTool.Api.Requests;
using PIMTool.Api.Responses;
using PIMTool.Core.Constants;
using PIMTool.Core.Entities;
using PIMTool.Core.Extensions;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Services.Interfaces;

namespace PIMTool.Api.Controllers;

public sealed class ProjectsController : BaseApiController
{
    private readonly IProjectService _projectService;
    private readonly IMapper _mapper;

    public ProjectsController(IProjectService projectService, IMapper mapper, IStringLocalizerFactory localizerFactory, IUnitOfWork unitOfWork) : base(localizerFactory, unitOfWork)
    {
        _projectService = projectService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetPaginatedProjects([FromQuery] PaginationRequest request)
    {
        var result = _projectService.GetPaginatedProjects(request.PageIndex, request.PageSize);
        if (result.IsSuccess)
        {
            var (projects, pageIndex, total) = result.Value;
            var response = _mapper.Map<ICollection<ProjectResponse>>(projects);

            var lastPage = (int)Math.Ceiling((decimal)total / request.PageSize);
            lastPage = Math.Max(1, lastPage);
            var paginatedResponse = new PaginationResponse<ProjectResponse>()
            {
                Items = response,
                PageSize = request.PageSize,
                PageIndex = pageIndex,
                IsLastPage = lastPage == pageIndex,
                LastPage = lastPage,
                Total = total
            };
            var finalResponse = new ApiResponse<PaginationResponse<ProjectResponse>>(paginatedResponse)
            {
                Title = LocalizeMessage(LocalizedMessages.Success),
            };

            return Ok(finalResponse);
        }

        return BuildErrorResponse(result.Reasons, title: LocalizedMessages.Error, detail: LocalizedMessages.ServerError);
    }

    [HttpGet("search")]
    public IActionResult SearchProjects([FromQuery] SearchWithPaginationRequest request)
    {
        var result = _projectService.SearchProjects(request.Keyword, request.Status, request.PageIndex, request.PageSize);
        if (result.IsSuccess)
        {
            var (projects, pageIndex, total) = result.Value;
            var response = _mapper.Map<ICollection<ProjectResponse>>(projects);

            var lastPage = (int)Math.Ceiling((decimal)total / request.PageSize);
            lastPage = Math.Max(1, lastPage);
            var paginatedResponse = new PaginationResponse<ProjectResponse>()
            {
                Items = response,
                PageSize = request.PageSize,
                PageIndex = pageIndex,
                IsLastPage = lastPage == pageIndex,
                LastPage = lastPage,
                Total = total
            };
            var finalResponse = new ApiResponse<PaginationResponse<ProjectResponse>>(paginatedResponse)
            {
                Title = LocalizeMessage(LocalizedMessages.Success),
            };

            return Ok(finalResponse);
        }

        return BuildErrorResponse(result.Reasons, title: LocalizedMessages.Error, detail: LocalizedMessages.ServerError);
    }

    [HttpGet]
    [Route("{id:int}")]
    public IActionResult GetProject([FromRoute] int id)
    {
        var result = _projectService.GetProjectDetail(id);
        if (result.IsSuccess)
        {
            var project = result.Value;
            var transformedProject = _mapper.Map<ProjectDetailResponse>(project);
            var response = new ApiResponse<ProjectDetailResponse>(transformedProject)
            {
                Title = LocalizeMessage(LocalizedMessages.Success),
            };

            return Ok(response);
        }

        return BuildErrorResponse(result.Reasons, title: LocalizedMessages.Error, detail: LocalizedMessages.ProjectNotFound);
    }

    [HttpPost]
    public IActionResult CreateProject([FromBody] CreateProjectRequest request)
    {
        var project = _mapper.Map<Project>(request);
        var result = _projectService.CreateProject(project, request.Visas);
        if (result.IsSuccess)
        {
            var createdProject = result.Value;
            var tranformedProject = _mapper.Map<ProjectResponse>(createdProject);

            var response = new ApiResponse<ProjectResponse>(tranformedProject)
            {
                Title = LocalizeMessage(LocalizedMessages.ProjectCreated),
            };

            return CreatedAtAction(nameof(CreateProject), response);
        }

        return BuildErrorResponse(result.Reasons, title: LocalizedMessages.Error, detail: LocalizedMessages.CreateProjectFailure);
    }

    [HttpPut("{id:int}")]
    public IActionResult UpdateProject([FromRoute] int id, [FromBody] UpdateProjectRequest request)
    {
        var project = _mapper.Map<Project>(request);
        var result = _projectService.UpdateProject(id, project, request.Visas);
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BuildErrorResponse(result.Reasons, title: LocalizedMessages.Error, detail: LocalizedMessages.UpdateProjectFailure);
    }

    [HttpDelete("{id:int}")]
    public IActionResult DeleteProject([FromRoute] int id)
    {
        var result = _projectService.DeleteProject(id);
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BuildErrorResponse(result.Reasons, title: LocalizedMessages.Error, detail: LocalizedMessages.DeleteProjectFailureDynamic.WithParameters(id));
    }

    [HttpDelete("many")]
    public IActionResult DeleteManyProjects([FromBody] int[] ids)
    {
        var result = _projectService.DeleteManyProjects(ids);
        if (result.IsSuccess)
        {
            return NoContent();
        }

        return BuildErrorResponse(result.Reasons, title: LocalizedMessages.Error, detail: LocalizedMessages.DeleteMultiProjectFailure);
    }
}
