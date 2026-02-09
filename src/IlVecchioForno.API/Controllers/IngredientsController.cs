using IlVecchioForno.API.Presenters.Ingredients;
using IlVecchioForno.API.Requests.Ingredients;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.Ingredients.GetIngredient;
using IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;
using IlVecchioForno.Application.UseCases.Ingredients.RegisterIngredient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[AllowAnonymous]
public sealed class IngredientsController : ApiControllerBase
{
    private readonly IApiIngredientPresenter _presenter;

    public IngredientsController(
        IMediator mediator,
        IApiIngredientPresenter presenter
    ) : base(mediator)
    {
        this._presenter = presenter;
        this._presenter.Initialize(
            nameof(this.GetByIdAsync).Replace("Async", string.Empty),
            nameof(IngredientsController)
        );
    }

    [HttpGet]
    public async Task<ActionResult> GetAllAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] IngredientsSorter sorter = IngredientsSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default
    )
    {
        ListIngredientsQuery query = new ListIngredientsQuery(
            page,
            pageSize,
            sorter,
            descending,
            search
        );
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult> GetByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetIngredientQuery query = new GetIngredientQuery(id);
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(
        [FromBody] RegisterIngredientRequest request,
        CancellationToken cancellationToken = default
    )
    {
        RegisterIngredientCommand command = new RegisterIngredientCommand(
            request.Name,
            request.QuantityTypeId
        );
        await this._mediator.Send(command, cancellationToken);
        return this._presenter.Result;
    }
}