using IlVecchioForno.API.Requests.Pizzas;
using IlVecchioForno.API.Utilities;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.Pizzas.ArchivePizza;
using IlVecchioForno.Application.UseCases.Pizzas.ChangePizzaDetails;
using IlVecchioForno.Application.UseCases.Pizzas.DTOs;
using IlVecchioForno.Application.UseCases.Pizzas.GetActivePizza;
using IlVecchioForno.Application.UseCases.Pizzas.GetArchivedPizza;
using IlVecchioForno.Application.UseCases.Pizzas.ListActivePizzas;
using IlVecchioForno.Application.UseCases.Pizzas.ListArchivedPizzas;
using IlVecchioForno.Application.UseCases.Pizzas.RegisterPizza;
using IlVecchioForno.Application.UseCases.Pizzas.UnarchivePizza;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[AllowAnonymous]
public sealed class PizzasController : ApiControllerBase
{
    public PizzasController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet("active")]
    public async Task<ActionResult<IReadOnlyList<ActivePizzaDto>>> GetActivePizzasAsync(
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
        ListActivePizzasQuery query =
            new ListActivePizzasQuery(page, pageSize, sorter, descending, search, minPrice, maxPrice);
        IReadOnlyList<ActivePizzaDto> resources = await this._mediator.Send(query, cancellationToken);
        return this.Ok(resources);
    }

    [HttpGet("active/{id:int}")]
    public async Task<ActionResult<ActivePizzaDto>> GetActiveByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetActivePizzaQuery query = new GetActivePizzaQuery(id);
        ActivePizzaDto resource = await this._mediator.Send(query, cancellationToken);
        return this.Ok(resource);
    }

    [HttpGet("archived")]
    public async Task<ActionResult<IReadOnlyList<ArchivedPizzaDto>>> GetArchivedPizzasAsync(
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
        ListArchivedPizzasQuery query =
            new ListArchivedPizzasQuery(page, pageSize, sorter, descending, search, minPrice, maxPrice);
        IReadOnlyList<ArchivedPizzaDto> resources = await this._mediator.Send(query, cancellationToken);
        return this.Ok(resources);
    }

    [HttpGet("archived/{id:int}")]
    public async Task<ActionResult<ArchivedPizzaDto>> GetArchivedByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetArchivedPizzaQuery query = new GetArchivedPizzaQuery(id);
        ArchivedPizzaDto resource = await this._mediator.Send(query, cancellationToken);
        return this.Ok(resource);
    }

    [HttpPost]
    public async Task<CreatedAtActionResult> PostAsync(
        [FromBody] RegisterPizzaRequest request,
        CancellationToken cancellationToken = default
    )
    {
        RegisterPizzaCommand command = new RegisterPizzaCommand(
            request.Name,
            request.Description,
            request.Price,
            request.IngredientsAndQuantities
        );
        int result = await this._mediator.Send(command, cancellationToken);
        return this.CreatedAtAction(
            ActionUtility.ActionName(nameof(this.GetActiveByIdAsync)),
            new { id = result },
            result
        );
    }

    [HttpPatch("{id:int}")]
    public async Task<NoContentResult> PatchDetailsAsync(
        [FromRoute] int id,
        [FromBody] ChangePizzaDetailsRequest request,
        CancellationToken cancellationToken = default
    )
    {
        ChangePizzaDetailsCommand command = new ChangePizzaDetailsCommand(
            id,
            request.Description,
            request.Price,
            request.IngredientsAndQuantities
        );
        await this._mediator.Send(command, cancellationToken);
        return this.NoContent();
    }

    [HttpPatch("{id:int}/archive")]
    public async Task<NoContentResult> PatchArchiveAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        ArchivePizzaCommand command = new ArchivePizzaCommand(id);
        await this._mediator.Send(command, cancellationToken);
        return this.NoContent();
    }

    [HttpPatch("{id:int}/unarchive")]
    public async Task<NoContentResult> PatchUnarchiveAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        UnarchivePizzaCommand command = new UnarchivePizzaCommand(id);
        await this._mediator.Send(command, cancellationToken);
        return this.NoContent();
    }
}