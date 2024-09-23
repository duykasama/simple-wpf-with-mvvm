using FluentResults;
using PIMTool.Core.Entities;
using PIMTool.Core.Repositories.Interfaces;
using PIMTool.Core.Services.Interfaces;

namespace PIMTool.Core.Services.Implementations;

public class EmployeeService : IEmployeeService
{
    private readonly IEmployeeRepository _employeeRepository;

    public EmployeeService(IEmployeeRepository employeeRepository)
    {
        _employeeRepository = employeeRepository;
    }

    public Result<IEnumerable<Employee>> GetAllEmployees()
    {
        var employees = _employeeRepository.GetAll();

        return Result.Ok(employees);
    }
}
