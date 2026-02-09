using IlVecchioForno.API.Presenters.QuantityTypes;
using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
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
        this._presenter.Initialize(
            nameof(this.GetByIdAsync).Replace("Async", string.Empty),
            nameof(QuantityTypesController)
        );
    }

    [HttpGet]
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
    public async Task<ActionResult> GetByIdAsync(
        [FromRoute] short id,
        CancellationToken cancellationToken = default
    )
    {
        GetQuantityTypeQuery query = new GetQuantityTypeQuery(id);
        await this._mediator.Send(query, cancellationToken);
        return this._presenter.Result;
    }
}