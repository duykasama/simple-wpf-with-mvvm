using FluentAssertions;
using NHibernate;
using PIMTool.Core.Entities;
using PIMTool.Core.Enums;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Repositories.Implementations;
using PIMTool.Core.Repositories.Interfaces;
using PIMTool.Tests.Fixtures;
using PIMTool.Tests.RepositoryTests.Fixtures;

namespace PIMTool.Tests.RepositoryTests;

[Trait("Category", "RepositoryTest")]
public class ProjectRepositoryTests
{
    private readonly UnitOfWorkFixture _unitOfWorkFixture;

    public ProjectRepositoryTests()
    {
        _unitOfWorkFixture = new UnitOfWorkFixture();
    }

    [Fact]
    public void TestGetAllProjects()
    {
        // Arrange
        IUnitOfWork unitOfWorkForCreating = _unitOfWorkFixture.CreateUnitOfWork();
        IUnitOfWork unitOfWorkForReading = _unitOfWorkFixture.CreateUnitOfWork();
        IProjectRepository projectRepositoryForCreating = new ProjectRepository(unitOfWorkForCreating);
        IProjectRepository projectRepositoryForReading = new ProjectRepository(unitOfWorkForReading);
        IEnumerable<Project> projectList = ProjectFixture.ProjectList();

        // Act
        // Session 1: Persist entities
        foreach (var project in projectList)
        {
            projectRepositoryForCreating.Add(project);
        }
        unitOfWorkForCreating.Complete();

        // Session 2: Get all persisted entities
        var projects = projectRepositoryForReading.GetAll().ToList();
        unitOfWorkForReading.Complete();

        // Assert
        projects.Should().NotBeEmpty();
    }

    [Fact]
    public void TestAddProject()
    {
        // Arrange
        IUnitOfWork unitOfWorkForCreating = _unitOfWorkFixture.CreateUnitOfWork();
        IUnitOfWork unitOfWorkForReading = _unitOfWorkFixture.CreateUnitOfWork();
        IProjectRepository projectRepositoryForCreating = new ProjectRepository(unitOfWorkForCreating);
        IProjectRepository projectRepositoryForReading = new ProjectRepository(unitOfWorkForReading);
        var project = new Project
        {
            Name = "Fake project",
            Customer = "Fake customer",
            Status = ProjectStatus.NEW,
            ProjectNumber = 9999,
            StartDate = new DateTime(2020, 12, 31),
        };

        // Act
        // Session 1: Persist entity
        projectRepositoryForCreating.Add(project);
        unitOfWorkForCreating.Complete();

        // Session 2: Check if entity was persisted
        var persistedProject = projectRepositoryForReading
            .GetAll()
            .ToList()
            .OrderBy(p => p.Id) // The entity with the highest value of "Id"
            .LastOrDefault();   // should be the one that has just been persisted
        unitOfWorkForReading.Complete();

        // Assert
        persistedProject.Should().NotBeNull();

        Assert.NotNull(persistedProject); // This statement tells the IDE that persistedProject is not null for the remaining lines

        persistedProject.Name
            .Should().Be("Fake project");

        persistedProject.Customer
            .Should().Be("Fake customer");

        persistedProject.Status
            .Should().Be(ProjectStatus.NEW);

        persistedProject.ProjectNumber
            .Should().Be(9999);

        persistedProject.StartDate.Year
            .Should().Be(2020);

        persistedProject.StartDate.Month
            .Should().Be(12);

        persistedProject.StartDate.Day
            .Should().Be(31);
    }

    [Fact]
    public void TestDeleteProject()
    {
        // Arrange
        IUnitOfWork unitOfWorkForCreating = _unitOfWorkFixture.CreateUnitOfWork();
        IUnitOfWork unitOfWorkForDeleting = _unitOfWorkFixture.CreateUnitOfWork();
        IUnitOfWork unitOfWorkForReading = _unitOfWorkFixture.CreateUnitOfWork();
        IProjectRepository projectRepositoryForCreating = new ProjectRepository(unitOfWorkForCreating);
        IProjectRepository projectRepositoryForDeleting = new ProjectRepository(unitOfWorkForDeleting);
        IProjectRepository projectRepositoryForReading = new ProjectRepository(unitOfWorkForReading);
        var project = new Project
        {
            Name = "Project to delete",
            Customer = "Fake customer",
            Status = ProjectStatus.NEW,
            ProjectNumber = 1,
            StartDate = new DateTime(2020, 1, 1),
        };

        // Act
        // Session 1: Persist entity
        projectRepositoryForCreating.Add(project);
        var persistedProject = projectRepositoryForCreating
            .GetAll()
            .ToList()
            .OrderBy(p => p.Id) // The entity with the highest value of "Id"
            .LastOrDefault();   // should be the one that has just been persisted
        var persistedProjectId = persistedProject?.Id;
        unitOfWorkForCreating.Complete();

        // Session 2: Delete the persisted entity
        projectRepositoryForDeleting.Delete(project);
        unitOfWorkForDeleting.Complete();

        // Session 3: Get the deleted entity
        var deletedProject = projectRepositoryForReading.Get((int)persistedProjectId);
        NHibernateUtil.Initialize(deletedProject?.Employees);
        unitOfWorkForReading.Complete();

        // Assert
        persistedProject.Should().NotBeNull(); // Ensure entity was persisted
        deletedProject.Should().BeNull();
    }

    [Fact]
    public void TestUpdateProject()
    {
        // Arrange
        IUnitOfWork unitOfWorkForCreating = _unitOfWorkFixture.CreateUnitOfWork();
        IUnitOfWork unitOfWorkForUpdating = _unitOfWorkFixture.CreateUnitOfWork();
        IUnitOfWork unitOfWorkForReading = _unitOfWorkFixture.CreateUnitOfWork();
        IProjectRepository projectRepositoryForCreating = new ProjectRepository(unitOfWorkForCreating);
        IProjectRepository projectRepositoryForUpdating = new ProjectRepository(unitOfWorkForUpdating);
        IProjectRepository projectRepositoryForReading = new ProjectRepository(unitOfWorkForReading);
        var project = new Project
        {
            Name = "Project to update",
            Customer = "Fake customer",
            Status = ProjectStatus.NEW,
            ProjectNumber = 1,
            StartDate = new DateTime(2020, 1, 1),
        };

        // Act
        // Session 1: Persist entity
        projectRepositoryForCreating.Add(project);
        var persistedProject = projectRepositoryForCreating
            .GetAll()
            .ToList()
            .OrderBy(p => p.Id) // The entity with the highest value of "Id"
            .LastOrDefault();   // should be the one that has just been persisted
        unitOfWorkForCreating.Complete();

        // Session 2: Update the persisted entity
        var retrievedProject = projectRepositoryForUpdating
            .GetAll()
            .ToList()
            .OrderBy(p => p.Id) // The entity with the highest value of "Id"
            .LastOrDefault();   // should be the one that has just been persisted
        retrievedProject.Name = "Project name has been updated";
        retrievedProject.ProjectNumber = 1234;
        retrievedProject.Customer = "Customer has been updated";
        projectRepositoryForUpdating.Update(retrievedProject);
        unitOfWorkForUpdating.Complete();

        // Session 3: Get the updated entity
        var updatedProject = projectRepositoryForReading
            .GetAll()
            .ToList()
            .OrderBy(p => p.Id) // The entity with the highest value of "Id"
            .LastOrDefault();   // should be the one that has just been persisted
        unitOfWorkForReading.Complete();

        // Assert
        persistedProject.Should().NotBeNull();
        updatedProject.Should().NotBeNull();
        Assert.NotNull(updatedProject); // This statement tells the IDE that persistedProject is not null for the remaining lines
        updatedProject.Name.Should().Be("Project name has been updated");
        updatedProject.ProjectNumber.Should().Be(1234);
        updatedProject.Customer.Should().Be("Customer has been updated");
    }
}