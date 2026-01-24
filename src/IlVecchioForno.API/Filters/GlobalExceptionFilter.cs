using FluentValidation;
using IlVecchioForno.Application.Common.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace IlVecchioForno.API.Filters;

public sealed class GlobalExceptionFilter : IExceptionFilter
{
    private readonly ILogger<GlobalExceptionFilter> _logger;

    public GlobalExceptionFilter(ILogger<GlobalExceptionFilter> logger)
    {
        this._logger = logger;
    }

    public void OnException(ExceptionContext context)
    {
        ProblemDetails problemDetails = context.Exception switch
        {
            ValidationException ex => new ValidationProblemDetails(GetValidationErrors(ex))
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest
            },
            InvalidReferenceException ex => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "Invalid reference",
                Status = StatusCodes.Status400BadRequest,
                Detail = ex.Message
            },
            EntityNotFoundException ex => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "Entity not found",
                Status = StatusCodes.Status404NotFound,
                Detail = ex.Message
            },
            EntityRegistrationException ex => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                Title = "Conflict",
                Status = StatusCodes.Status409Conflict,
                Detail = ex.Message
            },
            _ => new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Title = "Internal server error",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred"
            }
        };

        if (problemDetails.Status == StatusCodes.Status500InternalServerError)
            this._logger.LogError(context.Exception, "Unhandled exception");

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }

    private static Dictionary<string, string[]> GetValidationErrors(ValidationException ex)
    {
        return ex.Errors
            .GroupBy(e => e.PropertyName)
            .ToDictionary(
                g => g.Key,
                g => g.Select(e => e.ErrorMessage).ToArray()
            );
    }
}