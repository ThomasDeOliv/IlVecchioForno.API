using IlVecchioForno.API.Attributes;
using IlVecchioForno.API.Presenters;
using IlVecchioForno.API.Requests.Ingredients;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Common.Responses;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
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
    public IngredientsController(IMediator mediator, IPresenter presenter) : base(mediator, presenter)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<IngredientDto>>> GetAllAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] IngredientsSorter sorter = IngredientsSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default
    )
    {
        ListIngredientsQuery query = new ListIngredientsQuery(page, pageSize, sorter, descending, search);
        IResponse response = await this._mediator.Send(query, cancellationToken);
        return this._presenter.Present<IReadOnlyList<IngredientDto>>(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IngredientDto>> GetByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetIngredientQuery query = new GetIngredientQuery(id);
        IResponse response = await this._mediator.Send(query, cancellationToken);
        return this._presenter.Present<IngredientDto>(response);
    }

    [HttpPost]
    [CreatedAtAction(nameof(this.GetByIdAsync))]
    public async Task<ActionResult<IngredientDto>> PostAsync(
        [FromBody] RegisterIngredientRequest request,
        CancellationToken cancellationToken = default
    )
    {
        RegisterIngredientCommand command = new RegisterIngredientCommand(request.Name, request.QuantityTypeId);
        IResponse response = await this._mediator.Send(command, cancellationToken);
        return this._presenter.Present<IngredientDto>(response);
    }
}