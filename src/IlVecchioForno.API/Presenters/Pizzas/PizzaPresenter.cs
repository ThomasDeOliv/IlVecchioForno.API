using IlVecchioForno.API.Controllers;
using IlVecchioForno.API.Exceptions;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.Pizzas;

public sealed class PizzaPresenter : IApiPizzaPresenter
{
    private PizzasController? _controller;
    private ActionResult? _result;

    public PizzaPresenter()
    {
        this._controller = null;
        this._result = null;
    }

    private PizzasController Controller =>
        this._controller
        ?? throw new PresenterNotInitializedException();

    public ActionResult Result
    {
        get
        {
            ActionResult result = this._result
                ?? throw new PresenterResultNotSetException();
            this._result = null;
            return result;
        }
    }

    public void Initialize(PizzasController controller)
    {
        this._controller = controller;
    }

    public void EntityFound(ActivePizzaDto entity)
    {
        this._result = this.Controller.Ok(entity);
    }

    public void EntitiesListed(IReadOnlyList<ActivePizzaDto> entities)
    {
        this._result = this.Controller.Ok(entities);
    }

    public void EntityFound(ArchivedPizzaDto entity)
    {
        this._result = this.Controller.Ok(entity);
    }

    public void EntitiesListed(IReadOnlyList<ArchivedPizzaDto> entities)
    {
        this._result = this.Controller.Ok(entities);
    }

    public void EntityRegistered(ActivePizzaDto entity)
    {
        this._result = this.Controller.CreatedAtAction(
            nameof(this.Controller.GetActiveByIdAsync).Replace("Async", string.Empty),
            new { id = entity.Id },
            entity
        );
    }

    public void EntityUpdated()
    {
        this._result = this.Controller.NoContent();
    }

    public void InvalidReferenceError(string message)
    {
        this._result = this.Controller.Problem(
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.1",
            title: "A validation error occurred.",
            statusCode: StatusCodes.Status400BadRequest,
            detail: message
        );
    }

    public void EntityNotFound(string message)
    {
        this._result = this.Controller.Problem(
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.5",
            title: "Entity not found.",
            statusCode: StatusCodes.Status404NotFound,
            detail: message
        );
    }

    public void ValidationErrors(Dictionary<string, string[]> errors)
    {
        this._result = this.Controller.ValidationProblem(
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
        this._result = this.Controller.Problem(
            type: "https://tools.ietf.org/html/rfc9110#section-15.5.10",
            title: "Conflict.",
            statusCode: StatusCodes.Status409Conflict,
            detail: message
        );
    }
}