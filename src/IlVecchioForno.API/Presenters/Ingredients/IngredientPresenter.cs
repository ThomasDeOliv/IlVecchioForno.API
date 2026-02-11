using IlVecchioForno.API.Controllers;
using IlVecchioForno.API.Exceptions;
using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.Ingredients;

public sealed class IngredientPresenter : IApiIngredientPresenter
{
    private IngredientsController? _controller;
    private ActionResult? _result;

    public IngredientPresenter()
    {
        this._controller = null;
        this._result = null;
    }

    private IngredientsController Controller =>
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

    public void Initialize(IngredientsController controller)
    {
        this._controller = controller;
    }

    public void EntityFound(IngredientDto entity)
    {
        this._result = this.Controller.Ok(entity);
    }

    public void EntitiesListed(IReadOnlyList<IngredientDto> entities)
    {
        this._result = this.Controller.Ok(entities);
    }

    public void EntitiesCount(EntitiesCountDto count)
    {
        this._result = this.Controller.Ok(count);
    }

    public void EntityRegistered(IngredientDto entity)
    {
        this._result = this.Controller.CreatedAtAction(
            nameof(this.Controller.GetByIdAsync).Replace("Async", string.Empty),
            new { id = entity.Id },
            entity
        );
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