using IlVecchioForno.API.Presenters.Pizzas;
using IlVecchioForno.API.Requests.Pizzas;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.Pizzas.ArchivePizza;
using IlVecchioForno.Application.UseCases.Pizzas.ChangePizzaDetails;
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
    private readonly IApiPizzaPresenter _presenter;

    public PizzasController(
        IMediator mediator,
        IApiPizzaPresenter presenter
    ) : base(mediator)
    {
        this._presenter = presenter;
        this._presenter.Initialize(this);
    }

    [HttpGet("active")]
    public async Task<ActionResult> GetActivePizzasAsync(
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
        ListActivePizzasQuery query = new ListActivePizzasQuery(
            page,
            pageSize,
            sorter,
            descending,
            search,
            minPrice,
            maxPrice
        );
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }

    [HttpGet("active/{id:int}")]
    public async Task<ActionResult> GetActiveByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetActivePizzaQuery query = new GetActivePizzaQuery(id);
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }

    [HttpGet("archived")]
    public async Task<ActionResult> GetArchivedPizzasAsync(
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
        ListArchivedPizzasQuery query = new ListArchivedPizzasQuery(
            page,
            pageSize,
            sorter,
            descending,
            search,
            minPrice,
            maxPrice
        );
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }

    [HttpGet("archived/{id:int}")]
    public async Task<ActionResult> GetArchivedByIdAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        GetArchivedPizzaQuery query = new GetArchivedPizzaQuery(id);
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }

    [HttpPost]
    public async Task<ActionResult> PostAsync(
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
        await this._mediator.Send(command, cancellationToken);
        return this._presenter.Result;
    }

    [HttpPatch("{id:int}")]
    public async Task<ActionResult> PatchDetailsAsync(
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
        return this._presenter.Result;
    }

    [HttpPatch("{id:int}/archive")]
    public async Task<ActionResult> PatchArchiveAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        ArchivePizzaCommand command = new ArchivePizzaCommand(id);
        await this._mediator.Send(command, cancellationToken);
        return this._presenter.Result;
    }

    [HttpPatch("{id:int}/unarchive")]
    public async Task<ActionResult> PatchUnarchiveAsync(
        [FromRoute] int id,
        CancellationToken cancellationToken = default
    )
    {
        UnarchivePizzaCommand command = new UnarchivePizzaCommand(id);
        await this._mediator.Send(command, cancellationToken);
        return this._presenter.Result;
    }
}