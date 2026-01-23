using IlVecchioForno.API.Presenters;
using IlVecchioForno.API.Requests.Pizza;
using IlVecchioForno.API.Resources.ActivePizza;
using IlVecchioForno.API.Resources.ArchivedPizza;
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
public sealed class PizzaController : ApiControllerBase
{
    private readonly IPresenter<ActivePizzaDto, ActivePizzaResource> _activePizzaPresenter;
    private readonly IPresenter<ArchivedPizzaDto, ArchivedPizzaResource> _archivedPizzaPresenter;
    private readonly IMediator _mediator;

    public PizzaController(
        IPresenter<ActivePizzaDto, ActivePizzaResource> activePizzaPresenter,
        IPresenter<ArchivedPizzaDto, ArchivedPizzaResource> archivedPizzaPresenter,
        IMediator mediator
    ) : base()
    {
        this._activePizzaPresenter = activePizzaPresenter;
        this._archivedPizzaPresenter = archivedPizzaPresenter;
        this._mediator = mediator;
    }

    [HttpGet("active")]
    public async Task<ActionResult<IReadOnlyList<ActivePizzaResource>>> GetActivePizzasAsync(
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
        IReadOnlyList<ActivePizzaDto> result = await this._mediator.Send(query, cancellationToken);
        IReadOnlyList<ActivePizzaResource> resources = result
            .Select(a => this._activePizzaPresenter.Present(a))
            .ToList();
        return this.Ok(resources);
    }

    [HttpGet("active/{id:int}")]
    public async Task<ActionResult<ActivePizzaResource>> GetActiveByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetActivePizzaQuery query = new GetActivePizzaQuery(id);
        ActivePizzaDto? item = await this._mediator.Send(query, cancellationToken);

        if (item is null)
            return this.NotFound();

        ActivePizzaResource resource = this._activePizzaPresenter.Present(item);
        return this.Ok(resource);
    }

    [HttpGet("archived")]
    public async Task<ActionResult<IReadOnlyList<ArchivedPizzaResource>>> GetArchivedPizzasAsync(
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
        IReadOnlyList<ArchivedPizzaDto> result = await this._mediator.Send(query, cancellationToken);
        IReadOnlyList<ArchivedPizzaResource> resources = result
            .Select(a => this._archivedPizzaPresenter.Present(a))
            .ToList();
        return this.Ok(resources);
    }

    [HttpGet("archived/{id:int}")]
    public async Task<ActionResult<ArchivedPizzaResource>> GetArchivedByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetArchivedPizzaQuery query = new GetArchivedPizzaQuery(id);
        ArchivedPizzaDto? item = await this._mediator.Send(query, cancellationToken);

        if (item is null)
            return this.NotFound();

        ArchivedPizzaResource resource = this._archivedPizzaPresenter.Present(item);
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