using FluentResults;
using PIMTool.Core.Constants;
using PIMTool.Core.Entities;
using PIMTool.Core.Repositories.Interfaces;
using PIMTool.Core.Services.Interfaces;

namespace PIMTool.Core.Services.Implementations;

public class GroupService : IGroupService
{
    private readonly IGroupRepository _groupRepository;

    public GroupService(IGroupRepository groupRepository)
    {
        _groupRepository = groupRepository;
    }

    public Result<IEnumerable<Group>> GetAllGroups()
    {
        var groups = _groupRepository.GetAll();

        return Result.Ok(groups);
    }

    public Result<Group> GetById(int id)
    {
        var group = _groupRepository.Get(id);
        if (group == null)
        {
            return Result.Fail(LocalizedMessages.GroupNotFound);
        }

        return Result.Ok(group);
    }
}
