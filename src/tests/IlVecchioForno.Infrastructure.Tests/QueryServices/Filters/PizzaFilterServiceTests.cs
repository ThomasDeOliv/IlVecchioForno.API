using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Models;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Filters;

public sealed class PizzaFilterServiceTests : SeededInfrastructureTestsBase
{
    private readonly IFilterService<Pizza> _filterService;

    public PizzaFilterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._filterService = new PizzaFilterService();
    }

    public static TheoryData<string, List<int>> FilteredPizzas =>
        new TheoryData<string, List<int>>
        {
            {
                string.Empty, [..Enumerable.Range(0, 15)]
            },
            {
                "Ananas", []
            },
            {
                "Quattro Formaggi", [2]
            },
            {
                "ANA", [11, 13]
            },
            {
                "ana", [11, 13]
            },
            {
                "aNa", [11, 13]
            },
            {
                "classic Neapolitan pizza", [0]
            }
        };

    public static TheoryData<decimal?, decimal?, List<int>> RangeFilteredPizzas =>
        new TheoryData<decimal?, decimal?, List<int>>
        {
            {
                null, null, [..Enumerable.Range(0, 15)]
            },
            {
                12.00m, null, [4, 6, 7, 14]
            },
            {
                null, 9.00m, [0, 1, 10]
            },
            {
                10.00m, 11.00m, [3, 5, 8, 11, 12, 13]
            },
            {
                10.00m, 10.00m, [3, 11]
            },
            {
                8.00m, 8.00m, [0]
            },
            {
                20.00m, null, []
            },
            {
                null, 5.00m, []
            }
        };

    [Theory]
    [MemberData(nameof(FilteredPizzas))]
    public async Task Filter_ForPizzas_Return_ExpectedFilteredPizzas(
        string search,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<Pizza> expected = expectedIndexes.Select(i => this._pizzas[i]).ToList();
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new SearchFilterType(search)
            }
        );
        List<Pizza> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }

    [Theory]
    [MemberData(nameof(RangeFilteredPizzas))]
    public async Task RangeFilter_ForPizzas_Return_ExpectedFilteredPizzas(
        decimal? min,
        decimal? max,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<Pizza> expected = expectedIndexes.Select(i => this._pizzas[i]).ToList();
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new RangeFilterType<decimal>(min, max)
            }
        );
        List<Pizza> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }

    [Fact]
    public async Task Filter_ForPizzas_ThrowNotSupportedFilterException_WhenUnsupportedFilterProvided()
    {
        // Arrange
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act & Assert
        NotSupportedFilterException e = Assert.Throws<NotSupportedFilterException>(() => this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new FakeFilter()
            }
        ));
        Assert.Equal("Provided filter is not supported for entity Pizza.", e.Message);
        Assert.NotNull(e.InnerException);
        Assert.Equal("Filter type FakeFilter is not supported.", e.InnerException.Message);
    }
}