using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.Repositories;

internal sealed class EfIngredientRepository : IIngredientRepository
{
    private readonly IlVecchioFornoDbContext _ctx;
    private readonly IFilterService<Ingredient> _filterService;
    private readonly IPaginationService<Ingredient> _paginationService;
    private readonly ISorterService<Ingredient, IngredientsSorter> _sorterService;

    public EfIngredientRepository(
        IlVecchioFornoDbContext ctx,
        IFilterService<Ingredient> filterService,
        IPaginationService<Ingredient> paginationService,
        ISorterService<Ingredient, IngredientsSorter> _sorterService
    )
    {
        this._ctx = ctx;
        this._filterService = filterService;
        this._paginationService = paginationService;
        this._sorterService = _sorterService;
    }

    public async Task<IReadOnlyCollection<Ingredient>> ListAsync(
        QuerySpec<IngredientsSorter> query,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Ingredient> queryable = this._ctx.Ingredients
            .Include(i => i.QuantityType);

        queryable = this._filterService.Filter(queryable, query.Filters);
        queryable = this._sorterService.OrderBy(queryable, query.Sorter, query.Descending);
        queryable = this._paginationService.Paginate(queryable, query.Page, query.PageSize);

        return await queryable
            .ToListAsync(cancellationToken);
    }

    public async Task<Ingredient?> FindAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        return await this._ctx.Ingredients
            .Include(i => i.QuantityType)
            .Where(i => i.Id == id)
            .SingleOrDefaultAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Ingredient>> ResolveAsync(
        IEnumerable<int> ids,
        CancellationToken cancellationToken = default
    )
    {
        return await this._ctx.Ingredients
            .Include(i => i.QuantityType)
            .Where(i => ids.Contains(i.Id))
            .ToListAsync(cancellationToken);
    }

    public async Task<int> CountAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await this._ctx.Ingredients
            .CountAsync(cancellationToken);
    }

    public void Add(Ingredient newIngredient)
    {
        this._ctx.Ingredients.Add(newIngredient);
    }
}