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
        (int statusCode, string message) = context.Exception switch
        {
            ValidationException ex => (StatusCodes.Status400BadRequest, ex.Message),
            InvalidReferenceException ex => (StatusCodes.Status400BadRequest, ex.Message),
            EntityNotFoundException ex => (StatusCodes.Status404NotFound, ex.Message),
            EntityRegistrationException ex => (StatusCodes.Status409Conflict, ex.Message),
            _ => (StatusCodes.Status500InternalServerError, "Internal server error")
        };

        if (statusCode == StatusCodes.Status500InternalServerError)
            this._logger.LogError(context.Exception, "Unhandled exception");

        context.Result = new ObjectResult(new
        {
            Success = false,
            ErrorMessage = message
        })
        {
            StatusCode = statusCode
        };

        context.ExceptionHandled = true;
    }
}