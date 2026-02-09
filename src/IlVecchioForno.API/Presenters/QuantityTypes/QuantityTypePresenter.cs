using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.QuantityTypes;

public sealed class QuantityTypePresenter : PresenterBase, IApiQuantityTypePresenter
{
    public void EntityFound(QuantityTypeDto entity)
    {
        this._result = new OkObjectResult(entity);
    }

    public void EntitiesListed(IReadOnlyList<QuantityTypeDto> entities)
    {
        this._result = new OkObjectResult(entities);
    }

    public void EntityNotFound(string message)
    {
        this._result = new ObjectResult(
            new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.5",
                Title = "Entity not found.",
                Status = StatusCodes.Status404NotFound,
                Detail = message
            }
        );
    }

    public void ValidationErrors(Dictionary<string, string[]> errors)
    {
        this._result = new ObjectResult(
            new ValidationProblemDetails(errors)
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "One or more validation errors occurred.",
                Status = StatusCodes.Status400BadRequest
            }
        );
    }
}