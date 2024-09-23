using PIMTool.Client.Models;

namespace PIMTool.Client.Repositories.Interfaces;

public interface IGroupRepository
{
    Task<IEnumerable<Group>> GetAllGroups();
}
