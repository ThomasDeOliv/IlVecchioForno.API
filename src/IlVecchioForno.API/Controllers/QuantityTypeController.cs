using IlVecchioForno.API.Presenters;
using IlVecchioForno.API.Resources.QuantityType;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.QuantityTypes.DTOs;
using IlVecchioForno.Application.UseCases.QuantityTypes.GetQuantityType;
using IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[AllowAnonymous]
public sealed class QuantityTypeController : ApiControllerBase
{
    private readonly IMediator _mediator;
    private readonly IPresenter<QuantityTypeDto, QuantityTypeResource> _presenter;

    public QuantityTypeController(
        IMediator mediator,
        IPresenter<QuantityTypeDto, QuantityTypeResource> presenter
    ) : base()
    {
        this._mediator = mediator;
        this._presenter = presenter;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<QuantityTypeResource>>> GetAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] QuantityTypesSorter sorter = QuantityTypesSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default
    )
    {
        ListQuantityTypesQuery query = new ListQuantityTypesQuery(page, pageSize, sorter, descending, search);
        IReadOnlyList<QuantityTypeDto> result = await this._mediator.Send(query, cancellationToken);
        IReadOnlyList<QuantityTypeResource> resources = result
            .Select(q => this._presenter.Present(q))
            .ToList();
        return this.Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuantityTypeResource>> GetByIdAsync(
        [FromRoute] short id,
        CancellationToken cancellationToken = default
    )
    {
        GetQuantityTypeQuery query = new GetQuantityTypeQuery(id);
        QuantityTypeDto item = await this._mediator.Send(query, cancellationToken);
        QuantityTypeResource resource = this._presenter.Present(item);
        return this.Ok(resource);
    }
}