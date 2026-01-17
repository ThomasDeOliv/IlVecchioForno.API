using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.Ingredients.ListIngredients;
using IlVecchioForno.Application.UseCases.Ingredients.RegisterIngredient;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[AllowAnonymous]
public sealed class IngredientController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public IngredientController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<Result<IReadOnlyList<IngredientDTO>>>> GetAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] IngredientsSorter sorter = IngredientsSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default
    )
    {
        ListIngredientsQuery query = new ListIngredientsQuery(page, pageSize, sorter, descending, search);
        Result<IReadOnlyList<IngredientDTO>> result = await this._mediator.Send(query, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<Result<int>>> PostAsync(
        [FromBody] RegisterIngredientCommand command,
        CancellationToken cancellationToken = default
    )
    {
        Result<int> result = await this._mediator.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }
}