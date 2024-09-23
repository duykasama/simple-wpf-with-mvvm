using FluentResults;
using PIMTool.Core.Entities;

namespace PIMTool.Core.Services.Interfaces;

public interface IGroupService
{
    Result<IEnumerable<Group>> GetAllGroups();
    Result<Group> GetById(int id);
}
