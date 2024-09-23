using FluentResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;
using PIMTool.Api.Constants;
using PIMTool.Api.Resources;
using PIMTool.Core.Enums;
using PIMTool.Core.Pattern.Interfaces;
using Serilog;
using System.Diagnostics;
using System.Text.Json;

namespace PIMTool.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public abstract class BaseApiController : ControllerBase
{
    protected readonly IStringLocalizer _localizer;
    private readonly IUnitOfWork _unitOfWork;

    protected BaseApiController(IStringLocalizerFactory factory, IUnitOfWork unitOfWork)
    {
        var type = typeof(SharedResource);
        _localizer = factory.Create(type);
        _unitOfWork = unitOfWork;
    }

    protected ObjectResult BuildErrorResponse(IEnumerable<IReason> reasons, string title, string detail)
    {
        _unitOfWork.Rollback();
        var errors = new List<string>();
        var problemDetails = new ProblemDetails
        {
            Title = LocalizeMessage(title),
            Detail = LocalizeMessage(detail),
        };

        foreach (var reason in reasons)
        {
            reason.Metadata.TryGetValue(nameof(ErrorType), out var errorType);
            if (errorType != null)
            {
                string errorUriReference;
                int statusCode;

                switch (errorType)
                {
                    case ErrorType.ValidationError:
                        errorUriReference = ApiErrorType.BadRequest;
                        statusCode = StatusCodes.Status400BadRequest;
                        break;
                    case ErrorType.ResourceMissingError:
                        errorUriReference = ApiErrorType.NotFound;
                        statusCode = StatusCodes.Status404NotFound;
                        break;
                    case ErrorType.BusinessError:
                        errorUriReference = ApiErrorType.Conflict;
                        statusCode = StatusCodes.Status409Conflict;
                        break;
                    default:
                        errorUriReference = ApiErrorType.InternalServerError;
                        statusCode = StatusCodes.Status500InternalServerError;
                        break;
                }

                problemDetails.Status = statusCode;
                problemDetails.Type = errorUriReference;
            }

            var culture = Thread.CurrentThread.CurrentCulture;
            errors.Add(LocalizeMessage(reason.Message));
        }

        problemDetails.Extensions.Add("errors", errors);
        problemDetails.Extensions.Add("traceId", Activity.Current?.Id ?? HttpContext.TraceIdentifier);
        Log.Error($"Business error occurred, error detail: \"{JsonSerializer.Serialize(problemDetails)}\"");

        return new ObjectResult(problemDetails);
    }

    protected string LocalizeMessage(string content)
    {
        if (!content.Contains(':'))
        {
            return _localizer[content].Value;
        }

        var parts = content.Split(':');
        var contentTemplate = parts[0];
        var parameters = parts[1].Split(',');

        return _localizer[contentTemplate, parameters].Value;
    }
}
