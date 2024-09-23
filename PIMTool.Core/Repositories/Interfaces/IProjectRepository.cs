using PIMTool.Core.Entities;
using PIMTool.Core.Repositories.Base;

namespace PIMTool.Core.Repositories.Interfaces;

public interface IProjectRepository : IBaseRepository<Project>
{
    Project? GetProjectByNumber(int number);
    Project? GetProjectById(int id);
    IEnumerable<Project> GetInterval(int skip, int take);
    int CountAll();
}
