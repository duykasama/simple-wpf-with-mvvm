using FluentResults;
using PIMTool.Core.Entities;

namespace PIMTool.Core.Services.Interfaces;

public interface IEmployeeService
{
    Result<IEnumerable<Employee>> GetAllEmployees();
}
