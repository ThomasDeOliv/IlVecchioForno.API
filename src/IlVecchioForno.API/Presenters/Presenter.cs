using IlVecchioForno.Application.Common.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters;

public sealed class Presenter : IPresenter
{
    public ActionResult<T> Present<T>(IResponse response)
    {
        return response switch
        {
            ResponseForQuery<T> r => new ObjectResult(r.Content)
            {
                StatusCode = StatusCodes.Status200OK
            },
            ResponseForCommand<T> r when r.Content is Unit => new NoContentResult(),
            ResponseForCommand<T> r => new ObjectResult(r.Content)
            {
                StatusCode = StatusCodes.Status201Created
            },
            ResponseWithErrorMessages r => new ObjectResult(
                new ValidationProblemDetails(r.Messages)
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Title = "One or more validation errors occurred.",
                    Status = StatusCodes.Status400BadRequest
                }
            ),
            ResponseWithErrorMessage r when r.Type is ErrorMessageType.InvalidReferenceError => new ObjectResult(
                new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                    Title = "A validation error occurred.",
                    Status = StatusCodes.Status400BadRequest,
                    Detail = r.Message
                }
            ),
            ResponseWithErrorMessage r when r.Type is ErrorMessageType.EntityNotFoundError => new ObjectResult(
                new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                    Title = "Entity not found.",
                    Status = StatusCodes.Status404NotFound,
                    Detail = r.Message
                }
            ),
            ResponseWithErrorMessage r when r.Type is ErrorMessageType.EntityRegistrationError =>
                new ObjectResult(
                    new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                        Title = "Conflict.",
                        Status = StatusCodes.Status409Conflict,
                        Detail = r.Message
                    }
                ),
            _ => new ObjectResult(
                new ProblemDetails
                {
                    Type = "https://tools.ietf.org/html/rfc9110#section-15.6.1",
                    Title = "Internal server error.",
                    Status = StatusCodes.Status500InternalServerError,
                    Detail = "An unexpected error occurred"
                }
            )
        };
    }
}