using FluentAssertions;
using FluentResults;
using NSubstitute;
using PIMTool.Core.Constants;
using PIMTool.Core.Entities;
using PIMTool.Core.Enums;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Repositories.Interfaces;
using PIMTool.Core.Services.Implementations;
using PIMTool.Core.Services.Interfaces;
using PIMTool.Tests.Fixtures;
using PIMTool.Tests.Services.Fixtures;
using PIMTool.Tests.Services.Fixtures.Models;
using PIMTool.Tests.Services.TestData;

namespace PIMTool.Tests.Services;

[Trait("Category", "ServiceTest")]
public class ProjectServiceTests
{
    private readonly ProjectRepositoryFixture _fixture;

    public ProjectServiceTests()
    {
        _fixture = new ProjectRepositoryFixture();
    }

    [Theory]
    [MemberData(nameof(CreateProjectTestData.TestData), MemberType = typeof(CreateProjectTestData))]
    public void TestCreateProject(Project project, ExpectedCreateProjectResult expectedResult)
    {
        // Arrange
        var validVisa = "NTHY";
        var validEmployee = new Employee()
        {
            Visa = validVisa,
        };
        var duplicateProject = new Project
        {
            ProjectNumber = 1,
        };
        var groupRepository = Substitute.For<IGroupRepository>();
        groupRepository.Get(1).Returns(new Group());
        groupRepository.Get(0).Returns(default(Group));
        var employeeRepository = Substitute.For<IEmployeeRepository>();
        employeeRepository.GetAll().Returns(new List<Employee>() { validEmployee });
        var projectRepository = Substitute.For<IProjectRepository>();
        projectRepository.GetProjectByNumber(Arg.Any<int>()).Returns(default(Project));
        projectRepository.GetProjectByNumber(0).Returns(duplicateProject);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        string[] visas = [validVisa];
        IProjectService projectService = new ProjectService(groupRepository, employeeRepository, projectRepository, unitOfWork);

        // Act
        Result<Project> result = projectService.CreateProject(project, visas);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().Be(expectedResult.IsSuccess);
        result.IsFailed.Should().Be(expectedResult.IsFailed);
        var reason = result.Reasons.FirstOrDefault();
        reason.Should().NotBeNull();
        reason!.Message.Should().Be(expectedResult.Message);
    }

    [Fact]
    public void TestGetPaginatedProjects()
    {
        // Arrange
        var projectList = new List<Project>()
        {
            new Project() { Id = 1 },
            new Project() { Id = 2 },
        };
        var groupRepository = Substitute.For<IGroupRepository>();
        var employeeRepository = Substitute.For<IEmployeeRepository>();
        var projectRepository = Substitute.For<IProjectRepository>();
        projectRepository.GetInterval(Arg.Any<int>(), 2).Returns(projectList);
        projectRepository.CountAll().Returns(projectList.Count);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        IProjectService projectService = new ProjectService(groupRepository, employeeRepository, projectRepository, unitOfWork);

        // Act
        Result<(IEnumerable<Project>, int, int)> result = projectService.GetPaginatedProjects(1, 2);

        // Assert
        result.Should().NotBeNull();
        result.IsSuccess.Should().BeTrue();
        result.IsFailed.Should().BeFalse();
        (IEnumerable<Project> projects, int pageIndex, int total) = result.Value;
        projects.Count().Should().Be(2);
        total.Should().Be(2);
        pageIndex.Should().Be(1);
    }

    [Fact]
    public void TestUpdateProject()
    {
        // Arrange
        var groupRepository = Substitute.For<IGroupRepository>();
        groupRepository.Get(1).Returns(new Group());
        groupRepository.Get(0).Returns(default(Group));
        var employeeRepository = Substitute.For<IEmployeeRepository>();
        var projectRepository = Substitute.For<IProjectRepository>();
        var unitOfWork = Substitute.For<IUnitOfWork>();
        string[] visas = [];
        IProjectService projectService = new ProjectService(groupRepository, employeeRepository, projectRepository, unitOfWork);
        Project project = ProjectFixture.UpdatedProject();

        // Act
        Result updateProjectResult = projectService.UpdateProject(10, project, visas);

        // Assert
        updateProjectResult.Should().NotBeNull();
        updateProjectResult.IsSuccess.Should().BeTrue();
        updateProjectResult.IsFailed.Should().BeFalse();
        var reason = updateProjectResult.Reasons.FirstOrDefault();
        reason.Should().NotBeNull();
        reason!.Message.Should().Be(LocalizedMessages.ProjectUpdated);
    }

    [Fact]
    public void TestDeleteProject()
    {
        // Arrange
        var projectId = 0;
        var projectToDelete = new Project()
        {
            Id = projectId,
            Status = ProjectStatus.NEW
        };
        var groupRepository = Substitute.For<IGroupRepository>();
        var employeeRepository = Substitute.For<IEmployeeRepository>();
        var projectRepository = Substitute.For<IProjectRepository>();
        projectRepository.Get(projectId).Returns(projectToDelete);
        var unitOfWork = Substitute.For<IUnitOfWork>();
        IProjectService projectService = new ProjectService(groupRepository, employeeRepository, projectRepository, unitOfWork);

        // Act
        Result<Project> deleteProjectResult = projectService.DeleteProject(projectId);

        // Assert
        if (deleteProjectResult.IsFailed)
        {
            Assert.Fail();
        }
        var reason = deleteProjectResult.Reasons.FirstOrDefault();
        reason.Should().NotBeNull();
        reason!.Message.Should().Be(LocalizedMessages.ProjectDeleted);
    }
}
