using IlVecchioForno.Application.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters;

public sealed class Presenter : IPresenter
{
    private readonly ILogger<Presenter> _logger;

    public Presenter(ILogger<Presenter> logger)
    {
        this._logger = logger;
    }

    public ActionResult<T> Present<T>(IResponse response)
    {
        return response switch
        {
            Response<T> r when r.Type is ResponseType.Command && r.Content is Unit => new NoContentResult(),
            Response<T> r when r.Type is ResponseType.Command => new ObjectResult(r.Content)
            {
                StatusCode = StatusCodes.Status201Created
            },
            Response<T> r when r.Type is ResponseType.Query => new ObjectResult(r.Content)
            {
                StatusCode = StatusCodes.Status200OK
            },
            ErrorResponseWithMessages r => new ObjectResult(
                new ValidationProblemDetails(r.Messages)
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest
                }
            ),
            ErrorResponseWithMessage r when r.Type is ErrorResponseType.InvalidReferenceError => new ObjectResult(
                new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Title = "A validation error occurred.",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = r.Message
                }
            ),
            ErrorResponseWithMessage r when r.Type is ErrorResponseType.EntityNotFoundError => new ObjectResult(
                new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                    Title = "Entity not found.",
                    Status = StatusCodes.Status404NotFound,
                    Detail = r.Message
                }
            ),
            ErrorResponseWithMessage r when r.Type is ErrorResponseType.EntityRegistrationError =>
                new ObjectResult(
                    new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                        Title = "Conflict.",
                        Status = StatusCodes.Status409Conflict,
                        Detail = r.Message
                    }
                ),
            _ => this.LogAndReturnError(response)
        };
    }

    private ObjectResult LogAndReturnError(IResponse response)
    {
        if (this._logger.IsEnabled(LogLevel.Error))
            this._logger.LogError(
                "Unexpected response type received in presenter: {ResponseType}",
                response.GetType().Name
            );

        return new ObjectResult(
            new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                Title = "Internal server error.",
                Status = StatusCodes.Status500InternalServerError,
                Detail = "An unexpected error occurred"
            }
        );
    }
}