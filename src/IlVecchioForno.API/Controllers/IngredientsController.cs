using IlVecchioForno.API.Presenters.Ingredients;
using IlVecchioForno.API.Requests.Ingredients;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.Ingredients;
using IlVecchioForno.Application.UseCases.Ingredients.CountIngredients;
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
        this._presenter.Initialize(this);
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<IngredientDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
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
    [ProducesResponseType<IngredientDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetIngredientQuery query = new GetIngredientQuery(id);
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }

    [HttpGet("count")]
    [ProducesResponseType<EntitiesCountDto>(StatusCodes.Status200OK)]
    public async Task<ActionResult> CountAsync(
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default
    )
    {
        CountIngredientsQuery query = new CountIngredientsQuery(search);
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }

    [HttpPost]
    [ProducesResponseType<IngredientDto>(StatusCodes.Status201Created)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status409Conflict)]
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