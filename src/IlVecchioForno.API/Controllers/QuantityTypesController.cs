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
public sealed class QuantityTypesController : ApiControllerBase
{
    public QuantityTypesController(IMediator mediator) : base(mediator)
    {
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<QuantityTypeDto>>> GetAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] QuantityTypesSorter sorter = QuantityTypesSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default
    )
    {
        ListQuantityTypesQuery query = new ListQuantityTypesQuery(page, pageSize, sorter, descending, search);
        IReadOnlyList<QuantityTypeDto> resources = await this._mediator.Send(query, cancellationToken);
        return this.Ok(resources);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuantityTypeDto>> GetByIdAsync(
        [FromRoute] short id,
        CancellationToken cancellationToken = default
    )
    {
        GetQuantityTypeQuery query = new GetQuantityTypeQuery(id);
        QuantityTypeDto resource = await this._mediator.Send(query, cancellationToken);
        return this.Ok(resource);
    }
}