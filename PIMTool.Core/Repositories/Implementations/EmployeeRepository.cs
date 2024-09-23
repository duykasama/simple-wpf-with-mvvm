using PIMTool.Core.Entities;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Repositories.Base;
using PIMTool.Core.Repositories.Interfaces;

namespace PIMTool.Core.Repositories.Implementations;

public class EmployeeRepository : BaseRepository<Employee>, IEmployeeRepository
{
    public EmployeeRepository(IUnitOfWork unitOfWork) : base(unitOfWork)
    {
    }

    public Employee? GetEmployeeByVisa(string visa)
    {
        var allEmployees = _unitOfWork.CurrentSession.Query<Employee>().ToList();
        var employee = allEmployees
            .Where(e => e.Visa.Equals(visa, StringComparison.OrdinalIgnoreCase))
            .FirstOrDefault();

        return employee;
    }
}
