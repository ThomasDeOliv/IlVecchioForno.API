using IlVecchioForno.API.Presenters;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Common.Responses;
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
    public QuantityTypesController(IMediator mediator, IPresenter presenter) : base(mediator, presenter)
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
        IResponse response = await this._mediator.Send(query, cancellationToken);
        return this._presenter.Present<IReadOnlyList<QuantityTypeDto>>(response);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<QuantityTypeDto>> GetByIdAsync(
        [FromRoute] short id,
        CancellationToken cancellationToken = default
    )
    {
        GetQuantityTypeQuery query = new GetQuantityTypeQuery(id);
        IResponse response = await this._mediator.Send(query, cancellationToken);
        return this._presenter.Present<QuantityTypeDto>(response);
    }
}