using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PIMTool.Api.Responses;
using PIMTool.Core.Constants;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Services.Interfaces;

namespace PIMTool.Api.Controllers;

public sealed class GroupsController : BaseApiController
{
    private readonly IGroupService _groupService;
    private readonly IMapper _mapper;

    public GroupsController(IGroupService groupService, IStringLocalizerFactory localizerFactory, IMapper mapper, IUnitOfWork unitOfWork) : base(localizerFactory, unitOfWork)
    {
        _groupService = groupService;
        _mapper = mapper;
    }

    [HttpGet]
    public IActionResult GetAllGroups()
    {
        var getGroupsResult = _groupService.GetAllGroups();
        if (getGroupsResult.IsFailed)
        {
            return Problem();
        }

        var groups = getGroupsResult.Value;
        var transformedGroups = _mapper.Map<IEnumerable<GroupResponse>>(groups);
        var response = new ApiResponse<IEnumerable<GroupResponse>>(transformedGroups)
        {
            Title = LocalizeMessage(LocalizedMessages.Success),
        };

        if (!groups.Any())
        {
            response.Title = LocalizeMessage(LocalizedMessages.Error);
            response.Messages.Add(LocalizeMessage(LocalizedMessages.NoGroups));
        }

        return Ok(response);
    }
}
