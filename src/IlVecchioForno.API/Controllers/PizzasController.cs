using IlVecchioForno.API.Attributes;
using IlVecchioForno.API.Presenters;
using IlVecchioForno.API.Requests.Pizzas;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Common.Responses;
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
    public PizzasController(IMediator mediator, IPresenter presenter) : base(mediator, presenter)
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
        IResponse response = await this._mediator.Send(query, cancellationToken);
        return this._presenter.Present<IReadOnlyList<ActivePizzaDto>>(response);
    }

    [HttpGet("active/{id:int}")]
    public async Task<ActionResult<ActivePizzaDto>> GetActiveByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetActivePizzaQuery query = new GetActivePizzaQuery(id);
        IResponse response = await this._mediator.Send(query, cancellationToken);
        return this._presenter.Present<ActivePizzaDto>(response);
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
        IResponse response = await this._mediator.Send(query, cancellationToken);
        return this._presenter.Present<IReadOnlyList<ArchivedPizzaDto>>(response);
    }

    [HttpGet("archived/{id:int}")]
    public async Task<ActionResult<ArchivedPizzaDto>> GetArchivedByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetArchivedPizzaQuery query = new GetArchivedPizzaQuery(id);
        IResponse response = await this._mediator.Send(query, cancellationToken);
        return this._presenter.Present<ArchivedPizzaDto>(response);
    }

    [HttpPost]
    [CreatedAtAction(nameof(this.GetActiveByIdAsync))]
    public async Task<ActionResult<ActivePizzaDto>> PostAsync(
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
        IResponse response = await this._mediator.Send(command, cancellationToken);
        return this._presenter.Present<ActivePizzaDto>(response);
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult<Unit>> PatchDetailsAsync(
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
        IResponse response = await this._mediator.Send(command, cancellationToken);
        return this._presenter.Present<Unit>(response);
    }

    [HttpPatch("{id:int}/archive")]
    public async Task<ActionResult<Unit>> PatchArchiveAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        ArchivePizzaCommand command = new ArchivePizzaCommand(id);
        IResponse response = await this._mediator.Send(command, cancellationToken);
        return this._presenter.Present<Unit>(response);
    }

    [HttpPatch("{id:int}/unarchive")]
    public async Task<ActionResult<Unit>> PatchUnarchiveAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        UnarchivePizzaCommand command = new UnarchivePizzaCommand(id);
        IResponse response = await this._mediator.Send(command, cancellationToken);
        return this._presenter.Present<Unit>(response);
    }
}