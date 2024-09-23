using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PIMTool.Core.Constants;
using PIMTool.Core.Pattern.Interfaces;
using PIMTool.Core.Services.Interfaces;

namespace PIMTool.Api.Controllers;

public sealed class EmployeesController : BaseApiController
{
    private readonly IEmployeeService _employeeService;

    public EmployeesController(IEmployeeService employeeService, IStringLocalizerFactory localizerFactory, IUnitOfWork unitOfWork) : base(localizerFactory, unitOfWork)
    {
        _employeeService = employeeService;
    }

    [HttpGet]
    public IActionResult GetAll()
    {
        var result = _employeeService.GetAllEmployees();
        if (result.IsSuccess)
        {
            return Ok(result.Value);
        }

        return BuildErrorResponse(result.Reasons, LocalizedMessages.Error, LocalizedMessages.ServerError);
    }
}
