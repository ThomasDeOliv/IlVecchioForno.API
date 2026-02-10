using IlVecchioForno.API.Controllers;
using IlVecchioForno.API.Exceptions;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Presenters.QuantityTypes;

public sealed class QuantityTypePresenter : IApiQuantityTypePresenter
{
    private QuantityTypesController? _controller;
    private ActionResult? _result;

    public QuantityTypePresenter()
    {
        this._controller = null;
        this._result = null;
    }

    private QuantityTypesController Controller =>
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

    public void Initialize(QuantityTypesController controller)
    {
        this._controller = controller;
    }

    public void EntityFound(QuantityTypeDto entity)
    {
        this._result = this.Controller.Ok(entity);
    }

    public void EntitiesListed(IReadOnlyList<QuantityTypeDto> entities)
    {
        this._result = this.Controller.Ok(entities);
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
}