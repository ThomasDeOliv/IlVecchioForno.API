using IlVecchioForno.API.Requests.Ingredients;
using IlVecchioForno.API.Utilities;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.Ingredients.DTOs;
using IlVecchioForno.Application.UseCases.Ingredients.GetIngredient;
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

    public IngredientController(IMediator mediator) : base()
    {
        this._mediator = mediator;
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
        IReadOnlyList<IngredientDto> resources = await this._mediator.Send(query, cancellationToken);
        return this.Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<IngredientDto>> GetByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetIngredientQuery query = new GetIngredientQuery(id);
        IngredientDto resource = await this._mediator.Send(query, cancellationToken);
        return this.Ok(resource);
    }

    [HttpPost]
    public async Task<CreatedAtActionResult> PostAsync(
        [FromBody] RegisterIngredientRequest request,
        CancellationToken cancellationToken = default
    )
    {
        RegisterIngredientCommand command = new RegisterIngredientCommand(request.Name, request.QuantityTypeId);
        int result = await this._mediator.Send(command, cancellationToken);
        return this.CreatedAtAction(
            ActionUtility.ActionName(nameof(this.GetByIdAsync)),
            new { id = result },
            result
        );
    }
}