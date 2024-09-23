using PIMTool.Core.Constants;
using PIMTool.Core.Entities;
using PIMTool.Core.Enums;
using PIMTool.Core.Extensions;
using PIMTool.Tests.Services.Fixtures.Models;

namespace PIMTool.Tests.Services.TestData;

public static class CreateProjectTestData
{
    public static IEnumerable<object[]> TestData
    {
        get
        {
            return [
                [
                    new Project
                    {
                        Id = 1,
                        Name = "Project 1",
                        Customer = "Customer 1",
                        ProjectNumber = 1234,
                        Status = ProjectStatus.NEW,
                        GroupId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                    },
                    new ExpectedCreateProjectResult
                    {
                        IsSuccess = true,
                        IsFailed = false,
                        Message = LocalizedMessages.ProjectCreated,
                    }
                ],
                [
                    new Project
                    {
                        Id = 2,
                        Customer = "Customer 2",
                        ProjectNumber = 2345,
                        Status = ProjectStatus.PLA,
                        GroupId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                    },
                    new ExpectedCreateProjectResult
                    {
                        IsSuccess = false,
                        IsFailed = true,
                        Message = LocalizedMessages.MissingProjectName,
                    }
                ],
                [
                    new Project
                    {
                        Id = 3,
                        Name = "Project 3",
                        ProjectNumber = 3456,
                        Status = ProjectStatus.PLA,
                        GroupId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                    },
                    new ExpectedCreateProjectResult
                    {
                        IsSuccess = false,
                        IsFailed = true,
                        Message = LocalizedMessages.MissingCustomer,
                    }
                ],
                [
                    new Project
                    {
                        Id = 4,
                        Name = "Project 4",
                        Customer = "Customer 4",
                        ProjectNumber = 0,
                        Status = ProjectStatus.PLA,
                        GroupId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                    },
                    new ExpectedCreateProjectResult
                    {
                        IsSuccess = false,
                        IsFailed = true,
                        Message = LocalizedMessages.ProjectNumberAlreadyExistsDynamic.WithParameters(0),
                    }
                ],
                [
                    new Project
                    {
                        Id = 5,
                        Name = "Project 5",
                        Customer = "Customer 5",
                        ProjectNumber = -9999,
                        Status = ProjectStatus.PLA,
                        GroupId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                    },
                    new ExpectedCreateProjectResult
                    {
                        IsSuccess = false,
                        IsFailed = true,
                        Message = LocalizedMessages.InvalidProjectNumber,
                    }
                ],
                [
                    new Project
                    {
                        Id = 6,
                        Name = "Project 6",
                        Customer = "Customer 6",
                        ProjectNumber = 10000,
                        Status = ProjectStatus.PLA,
                        GroupId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                    },
                    new ExpectedCreateProjectResult
                    {
                        IsSuccess = false,
                        IsFailed = true,
                        Message = LocalizedMessages.InvalidProjectNumber,
                    }
                ],
                [
                    new Project
                    {
                        Id = 9,
                        Name = "Project 9",
                        Customer = "Customer 9",
                        ProjectNumber = 6789,
                        Status = ProjectStatus.PLA,
                        GroupId = 1,
                        StartDate = DateTime.Now.AddYears(-1),
                        EndDate = DateTime.Now.AddYears(-2),
                    },
                    new ExpectedCreateProjectResult
                    {
                        IsSuccess = false,
                        IsFailed = true,
                        Message = LocalizedMessages.InvalidStartDate,
                    }
                ],
                [
                    new Project
                    {
                        Id = 10,
                        Name = "Project 10",
                        Customer = "Customer 10",
                        ProjectNumber = 9876,
                        Status = ProjectStatus.PLA,
                        StartDate = DateTime.Now.AddYears(-1),
                    },
                    new ExpectedCreateProjectResult
                    {
                        IsSuccess = false,
                        IsFailed = true,
                        Message = LocalizedMessages.GroupNotFound,
                    }
                ],
            ];
        }
    }
}
