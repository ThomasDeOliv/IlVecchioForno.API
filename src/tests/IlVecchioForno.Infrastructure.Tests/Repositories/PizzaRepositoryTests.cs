using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Application.Gateways.Persistence;
using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using IlVecchioForno.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace IlVecchioForno.Infrastructure.Tests.Repositories;

public sealed class PizzaRepositoryTests : SeededInfrastructureTestsBase
{
    private readonly Mock<IFilterService<Pizza>> _filterServiceMock;
    private readonly Mock<IPaginationService<Pizza>> _paginationServiceMock;
    private readonly Mock<ISorterService<Pizza, PizzasSorter>> _sorterServiceMock;

    public PizzaRepositoryTests(DbContextFixture dbCtxFixture) : base(dbCtxFixture)
    {
        // Mocked dependencies
        this._filterServiceMock = new Mock<IFilterService<Pizza>>();
        this._paginationServiceMock = new Mock<IPaginationService<Pizza>>();
        this._sorterServiceMock = new Mock<ISorterService<Pizza, PizzasSorter>>();

        // Default filter service behavior
        this._filterServiceMock
            .Setup(f => f.Filter(
                    It.IsAny<IQueryable<Pizza>>(),
                    It.IsAny<IEnumerable<IFilterType>>()
                )
            )
            .Returns((IQueryable<Pizza> q, IEnumerable<IFilterType> filters) =>
                filters.Aggregate(q, (current, filter) => filter switch
                    {
                        RangeFilterType<decimal> rangeFilter =>
                            current.Where(p =>
                                (
                                    !rangeFilter.Min.HasValue
                                    || p.Price >= rangeFilter.Min.Value
                                )
                                &&
                                (
                                    !rangeFilter.Max.HasValue
                                    || p.Price <= rangeFilter.Max.Value
                                )
                            ),

                        SearchFilterType searchFilter when !string.IsNullOrEmpty(searchFilter.Search) =>
                            current.Where(p =>
                                EF.Functions.ILike(p.Name, $"%{searchFilter.Search}%")
                                || p.Description != null
                                && EF.Functions.ILike(p.Description, $"%{searchFilter.Search}%")
                            ),

                        SearchFilterType searchFilter when string.IsNullOrEmpty(searchFilter.Search) => current,

                        _ => throw new NotSupportedFilterException(
                            nameof(Pizza),
                            new NotSupportedException($"Filter type {filter.GetType().Name} is not supported.")
                        )
                    }
                )
            );

        // Default pagination service behavior
        this._paginationServiceMock
            .Setup(p => p.Paginate(
                    It.IsAny<IQueryable<Pizza>>(),
                    It.IsAny<int>(),
                    It.IsAny<int>()
                )
            )
            .Returns((IQueryable<Pizza> q, int page, int pageSize) =>
                q.Skip((page - 1) * pageSize)
                    .Take(pageSize)
            );

        // Default sort service behavior
        this._sorterServiceMock
            .Setup(s => s.OrderBy(
                    It.IsAny<IQueryable<Pizza>>(),
                    It.IsAny<PizzasSorter>(),
                    It.IsAny<bool>()
                )
            )
            .Returns((IQueryable<Pizza> q, PizzasSorter sorter, bool descending) => (sorter, descending) switch
                {
                    (PizzasSorter.Name, false) =>
                        q.OrderBy(p => p.Name)
                            .ThenBy(p => p.Id),

                    (PizzasSorter.Name, true) =>
                        q.OrderByDescending(p => p.Name)
                            .ThenBy(p => p.Id),

                    (PizzasSorter.Price, false) =>
                        q.OrderBy(p => p.Price)
                            .ThenBy(p => p.Id),

                    (PizzasSorter.Price, true) =>
                        q.OrderByDescending(p => p.Price)
                            .ThenBy(p => p.Id),

                    (PizzasSorter.Archived, false) =>
                        q.OrderBy(p => p.ArchivedAt.HasValue
                                    ? p.ArchivedAt.Value
                                    : DateTimeOffset.MaxValue // Null last
                            )
                            .ThenBy(p => p.Id),

                    (PizzasSorter.Archived, true) =>
                        q.OrderByDescending(p => p.ArchivedAt.HasValue
                                    ? p.ArchivedAt.Value
                                    : DateTimeOffset.MinValue // Null last
                            )
                            .ThenBy(p => p.Id),

                    (_, true) =>
                        q.OrderByDescending(p => p.Id),

                    _ => q.OrderBy(p => p.Id)
                }
            );
    }

    private EfPizzaRepository CreateNewRepository()
    {
        return new EfPizzaRepository(
            this._ctx,
            this._filterServiceMock.Object,
            this._paginationServiceMock.Object,
            this._sorterServiceMock.Object
        );
    }

    [Fact]
    public async Task FindAsync_WithExistingId_ReturnsExpectedPizza()
    {
        // Arrange
        IPizzaRepository repository = this.CreateNewRepository();
        // Act
        Pizza? result = await repository.FindAsync(1, TestContext.Current.CancellationToken);
        // Assert
        Assert.NotNull(result);
        Assert.Equivalent(this._pizzas[0], result);
    }
}