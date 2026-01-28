using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.Repositories;

internal sealed class EfQuantityTypeRepository : IQuantityTypeRepository
{
    private readonly IlVecchioFornoDbContext _ctx;
    private readonly IFilterService<QuantityType> _filterService;
    private readonly IPaginationService<QuantityType> _paginationService;
    private readonly ISorterService<QuantityType, QuantityTypesSorter> _sorterService;

    public EfQuantityTypeRepository(
        IlVecchioFornoDbContext ctx,
        IFilterService<QuantityType> filterService,
        IPaginationService<QuantityType> paginationService,
        ISorterService<QuantityType, QuantityTypesSorter> sorterService
    )
    {
        this._ctx = ctx;
        this._filterService = filterService;
        this._paginationService = paginationService;
        this._sorterService = sorterService;
    }

    public async Task<IReadOnlyCollection<QuantityType>> ListAsync(
        QuerySpec<QuantityTypesSorter> query,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes;

        queryable = this._filterService.Filter(queryable, query.Filters);
        queryable = this._sorterService.OrderBy(queryable, query.Sorter, query.Descending);
        queryable = this._paginationService.Paginate(queryable, query.Page, query.PageSize);

        return await queryable
            .ToListAsync(cancellationToken);
    }

    public async Task<QuantityType?> FindAsync(short id, CancellationToken cancellationToken = default)
    {
        return await this._ctx.QuantityTypes
            .FindAsync([id], cancellationToken);
    }
}