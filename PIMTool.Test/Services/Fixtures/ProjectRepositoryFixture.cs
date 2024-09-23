using NSubstitute;
using PIMTool.Core.Entities;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Repositories.Interfaces;
using PIMTool.Tests.Fixtures;

namespace PIMTool.Tests.Services.Fixtures;

internal class ProjectRepositoryFixture
{
    public IProjectRepository ProjectRepositoryMock { get; private set; }
    public IGroupRepository GroupRepositoryMock { get; private set; }
    public IUnitOfWork UnitOfWorkMock { get; private set; }

    public ProjectRepositoryFixture()
    {
        UnitOfWorkMock = Substitute.For<IUnitOfWork>();

        ProjectRepositoryMock = Substitute.For<IProjectRepository>();
        ProjectRepositoryMock.GetAll().Returns(ProjectFixture.ProjectList().AsQueryable());
        ProjectRepositoryMock.Get(Arg.Any<int>()).Returns(default(Project));
        ProjectRepositoryMock.Get(10).Returns(new Project());

        GroupRepositoryMock = Substitute.For<IGroupRepository>();
        GroupRepositoryMock.Get(1).Returns(new Group());
        GroupRepositoryMock.Get(0).Returns(default(Group));
    }
}
