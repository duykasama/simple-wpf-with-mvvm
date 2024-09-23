using PIMTool.Core.Entities;
using PIMTool.Core.Repositories.Base;

namespace PIMTool.Core.Repositories.Interfaces;

public interface IEmployeeRepository : IBaseRepository<Employee>
{
    Employee? GetEmployeeByVisa(string visa);
}
