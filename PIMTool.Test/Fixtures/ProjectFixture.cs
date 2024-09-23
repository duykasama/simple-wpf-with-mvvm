using PIMTool.Core.Entities;
using PIMTool.Core.Enums;

namespace PIMTool.Tests.Fixtures;

internal static class ProjectFixture
{
    public static Project NewProject()
    {
        var project = new Project
        {
            Name = "Fake project",
            Customer = "Fake customer",
            ProjectNumber = 9999,
            Status = ProjectStatus.NEW,
            StartDate = DateTime.Now.AddYears(-1),
        };

        return project;
    }

    public static Project UpdatedProject()
    {
        var project = new Project
        {
            Name = "Updated project",
            Customer = "Updated customer",
            ProjectNumber = 6666,
            Status = ProjectStatus.PLA,
            GroupId = 1,
            StartDate = DateTime.Now.AddYears(-1),
        };

        return project;
    }

    public static IEnumerable<Project> ProjectList()
    {
        var projects = new List<Project>()
        {
            new ()
            {
                Name = "Project 1",
                Customer = "Customer 1",
                ProjectNumber = 1111,
                Status = ProjectStatus.PLA,
                StartDate = DateTime.Now.AddYears(-1),
            },
            new ()
            {
                Name = "Project 2",
                Customer = "Customer 2",
                ProjectNumber = 2222,
                Status = ProjectStatus.FIN,
                StartDate = DateTime.Now.AddYears(-1),
            },
            new ()
            {
                Name = "Project 3",
                Customer = "Customer 3",
                ProjectNumber = 3333,
                Status = ProjectStatus.NEW,
                StartDate = DateTime.Now.AddYears(-1),
            },
            new ()
            {
                Name = "Project 4",
                Customer = "Customer 4",
                ProjectNumber = 0,
                Status = ProjectStatus.NEW,
                StartDate = DateTime.Now.AddYears(-1),
            },
        };

        return projects;
    }
}
