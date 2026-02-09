using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.Pizzas;

public sealed class PizzaPresenter : PresenterBase, IApiPizzaPresenter
{
    public void EntityFound(ActivePizzaDto entity)
    {
        this._result = new OkObjectResult(entity);
    }

    public void EntitiesListed(IReadOnlyList<ActivePizzaDto> entities)
    {
        this._result = new OkObjectResult(entities);
    }

    public void EntityFound(ArchivedPizzaDto entity)
    {
        this._result = new OkObjectResult(entity);
    }

    public void EntitiesListed(IReadOnlyList<ArchivedPizzaDto> entities)
    {
        this._result = new OkObjectResult(entities);
    }

    public void EntityRegistered(ActivePizzaDto entity)
    {
        this._result = new CreatedAtActionResult(
            this.ActionName,
            this.ControllerName,
            new { id = entity.Id },
            entity
        );
    }

    public void EntityUpdated()
    {
        this._result = new NoContentResult();
    }

    public void InvalidReferenceError(string message)
    {
        this._result = new ObjectResult(
            new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1",
                Title = "A validation error occurred.",
                Status = StatusCodes.Status400BadRequest,
                Detail = message
            }
        );
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

    public void RegistrationError(string message)
    {
        this._result = new ObjectResult(
            new ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.10",
                Title = "Conflict.",
                Status = StatusCodes.Status409Conflict,
                Detail = message
            }
        );
    }
}