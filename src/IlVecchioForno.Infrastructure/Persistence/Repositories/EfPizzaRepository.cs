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
    private readonly IlVecchioFornoDbContext _ctx;
    private readonly IFilterService<Pizza> _filterService;
    private readonly IPaginationService<Pizza> _paginationService;
    private readonly ISorterService<Pizza, PizzasSorter> _sorterService;

    public EfPizzaRepository(
        IlVecchioFornoDbContext ctx,
        IFilterService<Pizza> filterService,
        IPaginationService<Pizza> paginationService,
        ISorterService<Pizza, PizzasSorter> sorterService
    )
    {
        this._ctx = ctx;
        this._filterService = filterService;
        this._paginationService = paginationService;
        this._sorterService = sorterService;
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

    public async Task<int> TotalCountActiveAsync(
        TotalCountQuerySpec querySpec,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Pizza> queryable = this._ctx.Pizzas;
        queryable = this._filterService.Filter(queryable, querySpec.Filters);
        return await queryable
            .CountAsync(
                p => !p.ArchivedAt.HasValue,
                cancellationToken
            );
    }

    public async Task<int> TotalCountArchivedAsync(
        TotalCountQuerySpec querySpec,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Pizza> queryable = this._ctx.Pizzas;
        queryable = this._filterService.Filter(queryable, querySpec.Filters);
        return await queryable
            .CountAsync(
                p => p.ArchivedAt.HasValue,
                cancellationToken
            );
    }

    public void Add(Pizza pizza)
    {
        this._ctx.Pizzas.Add(pizza);
    }

    public async Task<IReadOnlyCollection<Pizza>> ListActiveAsync(
        ListQuerySpec<PizzasSorter> querySpec,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Pizza> queryable = this._ctx.Pizzas
            .Include(p => p.PizzaIngredients)
            .ThenInclude(pi => pi.Ingredient)
            .Where(p => !p.ArchivedAt.HasValue);

        queryable = this._filterService.Filter(queryable, querySpec.Filters);
        queryable = this._sorterService.OrderBy(queryable, querySpec.Sorter, querySpec.Descending);
        queryable = this._paginationService.Paginate(queryable, querySpec.Page, querySpec.PageSize);

        return await queryable
            .ToListAsync(cancellationToken);
    }

    public async Task<IReadOnlyCollection<Pizza>> ListArchivedAsync(
        ListQuerySpec<PizzasSorter> querySpec,
        CancellationToken cancellationToken = default
    )
    {
        IQueryable<Pizza> queryable = this._ctx.Pizzas
            .Include(p => p.PizzaIngredients)
            .ThenInclude(pi => pi.Ingredient)
            .Where(p => p.ArchivedAt.HasValue);

        queryable = this._filterService.Filter(queryable, querySpec.Filters);
        queryable = this._sorterService.OrderBy(queryable, querySpec.Sorter, querySpec.Descending);
        queryable = this._paginationService.Paginate(queryable, querySpec.Page, querySpec.PageSize);

        return await queryable
            .ToListAsync(cancellationToken);
    }
}