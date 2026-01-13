using IlVecchioForno.Application.Common;
using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.UseCases.QuantityTypes.ListQuantityTypes;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IlVecchioForno.API.Controllers;

[AllowAnonymous]
public sealed class QuantityTypeController : ApiControllerBase
{
    private readonly IMediator _mediator;

    public QuantityTypeController(IMediator mediator)
    {
        this._mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<Result<IReadOnlyList<QuantityTypeDTO>>>> GetAsync(
        [FromQuery] int page = QueryDefaultValues.PageNumberMin,
        [FromQuery] int pageSize = QueryDefaultValues.PageSizeDefault,
        [FromQuery] QuantityTypesSorter sorter = QuantityTypesSorter.Id,
        [FromQuery] bool descending = false,
        [FromQuery] string? search = null,
        CancellationToken cancellationToken = default
    )
    {
        ListQuantityTypesQuery query = new ListQuantityTypesQuery(page, pageSize, sorter, descending, search);
        Result<IReadOnlyList<QuantityTypeDTO>> result = await this._mediator.Send(query, cancellationToken);
        return this.ToActionResult(result);
    }
}