using NHibernate;
using NHibernate.Linq;
using PIMTool.Core.Entities;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Repositories.Base;
using PIMTool.Core.Repositories.Interfaces;

namespace PIMTool.Core.Repositories.Implementations;

public class ProjectRepository : BaseRepository<Project>, IProjectRepository
{
    private readonly ISession _session;

    public ProjectRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
        _session = unitOfWork.CurrentSession;
    }

    public int CountAll()
    {
        var count = _session.Query<Project>().Count();

        return count;
    }

    public IEnumerable<Project> GetInterval(int skip, int take)
    {
        var projects = _session.Query<Project>()
            .OrderBy(x => x.ProjectNumber)
            .Skip(skip)
            .Take(take)
            .ToList();

        return projects;
    }

    public Project? GetProjectById(int id)
    {
        var project = _session.Query<Project>()
            .Where(p => p.Id == id)
            .Fetch(p => p.Employees)
            .ToList()
            .FirstOrDefault();

        return project;
    }

    public Project? GetProjectByNumber(int number)
    {
        var project = _session.Query<Project>()
            .Where(p => p.ProjectNumber == number)
            .FirstOrDefault();

        return project;
    }
}
