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
        if (this._logger.IsEnabled(LogLevel.Error))
            this._logger.LogError(
                context.Exception,
                "Unhandled exception on {Method} {Path}",
                context.HttpContext.Request.Method,
                context.HttpContext.Request.Path
            );

        ProblemDetails problemDetails = new ProblemDetails
        {
            Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
            Title = "Internal server error",
            Status = StatusCodes.Status500InternalServerError,
            Detail = "An unexpected error occurred."
        };

        context.Result = new ObjectResult(problemDetails)
        {
            StatusCode = problemDetails.Status
        };

        context.ExceptionHandled = true;
    }
}