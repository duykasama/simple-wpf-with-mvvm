using PIMTool.Core.Entities;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Repositories.Base;
using PIMTool.Core.Repositories.Interfaces;

namespace PIMTool.Core.Repositories.Implementations;

public class GroupRepository : BaseRepository<Group>, IGroupRepository
{
    public GroupRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }
}
