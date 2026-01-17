using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.Pizzas.ArchivePizza;
using IlVecchioForno.Application.UseCases.Pizzas.ChangePizzaDetails;
using IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;
using IlVecchioForno.Application.UseCases.Pizzas.ListArchivedPizzas;
using IlVecchioForno.Application.UseCases.Pizzas.RegisterPizza;
using IlVecchioForno.Application.UseCases.Pizzas.UnarchivePizza;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[AllowAnonymous]
public sealed class PizzaController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public PizzaController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet("active")]
    public async Task<ActionResult<Result<IReadOnlyList<ActivePizzaDTO>>>> GetActivePizzasAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] ActivePizzasSorter sorter = ActivePizzasSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        CancellationToken cancellationToken = default
    )
    {
        ListActivePizzasQuery query = new ListActivePizzasQuery(page, pageSize, sorter, descending, search, minPrice, maxPrice);
        Result<IReadOnlyList<ActivePizzaDTO>> result = await this._mediator.Send(query, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpGet("archived")]
    public async Task<ActionResult<Result<IReadOnlyList<ArchivedPizzaDTO>>>> GetArchivedPizzasAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] ArchivedPizzasSorter sorter = ArchivedPizzasSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        [FromQuery] decimal? minPrice = null,
        [FromQuery] decimal? maxPrice = null,
        CancellationToken cancellationToken = default
    )
    {
        ListArchivedPizzasQuery query = new ListArchivedPizzasQuery(page, pageSize, sorter, descending, search, minPrice, maxPrice);
        Result<IReadOnlyList<ArchivedPizzaDTO>> result = await this._mediator.Send(query, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<Result<int>>> PostAsync(
        [FromBody] RegisterPizzaCommand command,
        CancellationToken cancellationToken = default
    )
    {
        Result<int> result = await this._mediator.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpPatch("details")]
    public async Task<ActionResult<Result<int>>> PatchDetailsAsync(
        [FromBody] ChangePizzaDetailsCommand command,
        CancellationToken cancellationToken = default
    )
    {
        Result<int> result = await this._mediator.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpPatch("archive")]
    public async Task<ActionResult<Result<int>>> PatchArchivedStatusAsync(
        [FromBody] ArchivePizzaCommand command,
        CancellationToken cancellationToken = default
    )
    {
        Result<int> result = await this._mediator.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }

    [HttpPatch("unarchive")]
    public async Task<ActionResult<Result<int>>> PatchUnarchivedStatusAsync(
        [FromBody] UnarchivePizzaCommand command,
        CancellationToken cancellationToken = default
    )
    {
        Result<int> result = await this._mediator.Send(command, cancellationToken);
        return this.ToActionResult(result);
    }
}