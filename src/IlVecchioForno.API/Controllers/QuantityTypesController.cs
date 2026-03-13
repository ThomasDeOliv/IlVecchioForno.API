using IlVecchioForno.API.Presenters.QuantityTypes;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.DTOs;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.QuantityTypes;
using IlVecchioForno.Application.UseCases.QuantityTypes.CountQuantityTypes;
using IlVecchioForno.Application.UseCases.QuantityTypes.GetQuantityType;
using IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[AllowAnonymous]
public sealed class QuantityTypesController : ApiControllerBase
{
    private readonly IApiQuantityTypePresenter _presenter;

    public QuantityTypesController(
        IMediator mediator,
        IApiQuantityTypePresenter presenter
    ) : base(mediator)
    {
        this._presenter = presenter;
        this._presenter.Initialize(this);
    }

    [HttpGet]
    [ProducesResponseType<IReadOnlyList<QuantityTypeDto>>(StatusCodes.Status200OK)]
    [ProducesResponseType<ValidationProblemDetails>(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult> GetAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] QuantityTypesSorter sorter = QuantityTypesSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default
    )
    {
        ListQuantityTypesQuery query = new ListQuantityTypesQuery(
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
    [ProducesResponseType<QuantityTypeDto>(StatusCodes.Status200OK)]
    [ProducesResponseType<ProblemDetails>(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> GetByIdAsync(
        [FromRoute] short id,
        CancellationToken cancellationToken = default
    )
    {
        GetQuantityTypeQuery query = new GetQuantityTypeQuery(id);
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
        CountQuantityTypesQuery query = new CountQuantityTypesQuery(search);
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }
}