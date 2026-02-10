using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Persistence.Repositories;

internal sealed class EfPizzaRepository : IPizzaRepository
{
    private readonly ISorterService<Pizza, ActivePizzasSorter> _activeSorterService;
    private readonly ISorterService<Pizza, ArchivedPizzasSorter> _archivedSorterService;
    private readonly IlVecchioFornoDbContext _ctx;
    private readonly IFilterService<Pizza> _filterService;
    private readonly IPaginationService<Pizza> _paginationService;

    public EfPizzaRepository(
        IlVecchioFornoDbContext ctx,
        IFilterService<Pizza> filterService,
        IPaginationService<Pizza> paginationService,
        ISorterService<Pizza, ActivePizzasSorter> activeSorterService,
        ISorterService<Pizza, ArchivedPizzasSorter> archivedSorterService
    )
    {
        this._ctx = ctx;
        this._filterService = filterService;
        this._paginationService = paginationService;
        this._activeSorterService = activeSorterService;
        this._archivedSorterService = archivedSorterService;
    }

    public async Task<Pizza?> FindAsync(
        int id,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Pizza> queryable = this._ctx.Pizzas
            .Include(p => p.PizzaIngredients)
            .ThenInclude(pi => pi.Ingredient);

        return await queryable
            .SingleOrDefaultAsync(p => p.Id.Equals(id), cancellationToken);
    }

    public async Task<int> CountActiveAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await this._ctx.Pizzas
            .CountAsync(p => !p.ArchivedAt.HasValue,
                cancellationToken
            );
    }

    public async Task<int> CountArchivedAsync(
        CancellationToken cancellationToken = default
    )
    {
        return await this._ctx.Pizzas
            .CountAsync(p => p.ArchivedAt.HasValue,
                cancellationToken
            );
    }

    public void Add(Pizza pizza)
    {
        this._ctx.Pizzas.Add(pizza);
    }

    public async Task<IReadOnlyCollection<Pizza>> ListActiveAsync(
        QuerySpec<ActivePizzasSorter> query,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Pizza> queryable = this._ctx.Pizzas
            .Include(p => p.PizzaIngredients)
            .ThenInclude(pi => pi.Ingredient)
            .Where(p => !p.ArchivedAt.HasValue);

        queryable = this._filterService.Filter(queryable, query.Filters);
        queryable = this._activeSorterService.OrderBy(queryable, query.Sorter, query.Descending);
        queryable = this._paginationService.Paginate(queryable, query.Page, query.PageSize);

        return await queryable
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Pizza>> ListArchivedAsync(
        QuerySpec<ArchivedPizzasSorter> query,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Pizza> queryable = this._ctx.Pizzas
            .Include(p => p.PizzaIngredients)
            .ThenInclude(pi => pi.Ingredient)
            .Where(p => p.ArchivedAt.HasValue);

        queryable = this._filterService.Filter(queryable, query.Filters);
        queryable = this._archivedSorterService.OrderBy(queryable, query.Sorter, query.Descending);
        queryable = this._paginationService.Paginate(queryable, query.Page, query.PageSize);

        return await queryable
            .ToListAsync(cancellationToken);
    }
}