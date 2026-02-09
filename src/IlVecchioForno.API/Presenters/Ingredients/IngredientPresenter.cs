using IlVecchioForno.API.Controllers;
using IlVecchioForno.API.Exceptions;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.Ingredients;

public sealed class IngredientPresenter : IApiIngredientPresenter
{
    private IngredientsController? _ingredientsController;
    private ActionResult? _result;

    public IngredientPresenter()
    {
        this._ingredientsController = null;
        this._result = null;
    }

    public IngredientsController IngredientsController =>
        this._ingredientsController
        ?? throw new PresenterControllerNotSetException();

    public ActionResult Result =>
        this._result
        ?? throw new PresenterResultNotSetException();

    public void Initialize(ApiControllerBase controller)
    {
        if (controller is not IngredientsController ingredientsController)
            throw new PresenterInvalidControllerProvidedException(typeof(IngredientsController), controller.GetType());

        this._ingredientsController = ingredientsController;
    }

    public void EntityFound(IngredientDto entity)
    {
        this._result = new OkObjectResult(entity);
    }

    public void EntitiesListed(IReadOnlyList<IngredientDto> entities)
    {
        this._result = new OkObjectResult(entities);
    }

    public void EntityRegistered(IngredientDto entity)
    {
        this._result = new CreatedAtActionResult(
            nameof(this.IngredientsController.GetByIdAsync).Replace("Async", string.Empty),
            this.IngredientsController.GetType().Name.Replace("Controller", string.Empty),
            new { id = entity.Id },
            entity
        );
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