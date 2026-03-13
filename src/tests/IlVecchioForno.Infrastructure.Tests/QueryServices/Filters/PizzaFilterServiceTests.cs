using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
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

    public static TheoryData<string, List<Pizza>> FilteredPizzas =>
        new TheoryData<string, List<Pizza>>
        {
            {
                string.Empty, new List<Pizza>(DbMockedTestsData.TestsPizzas)
            },
            {
                "Ananas", new List<Pizza>()
            },
            {
                "Quattro Formaggi", new List<Pizza>([DbMockedTestsData.TestsPizzas[2]])
            },
            {
                "ANA", new List<Pizza>([DbMockedTestsData.TestsPizzas[11], DbMockedTestsData.TestsPizzas[13]])
            },
            {
                "ana", new List<Pizza>([DbMockedTestsData.TestsPizzas[11], DbMockedTestsData.TestsPizzas[13]])
            },
            {
                "aNa", new List<Pizza>([DbMockedTestsData.TestsPizzas[11], DbMockedTestsData.TestsPizzas[13]])
            },
            {
                "classic Neapolitan pizza", new List<Pizza>([DbMockedTestsData.TestsPizzas[0]])
            }
        };

    public static TheoryData<decimal?, decimal?, List<Pizza>> RangeFilteredPizzas =>
        new TheoryData<decimal?, decimal?, List<Pizza>>
        {
            {
                null, null, new List<Pizza>(
                    DbMockedTestsData.TestsPizzas
                )
            },
            {
                12.00m, null, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[4],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[7],
                    DbMockedTestsData.TestsPizzas[14]
                }
            },
            {
                null, 9.00m, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[0],
                    DbMockedTestsData.TestsPizzas[1],
                    DbMockedTestsData.TestsPizzas[10]
                }
            },
            {
                10.00m, 11.00m, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[3],
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[13]
                }
            },
            {
                10.00m, 10.00m, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[3],
                    DbMockedTestsData.TestsPizzas[11]
                }
            },
            {
                8.00m, 8.00m, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[0]
                }
            },
            {
                20.00m, null, new List<Pizza>()
            },
            {
                null, 5.00m, new List<Pizza>()
            }
        };

    [Theory]
    [MemberData(nameof(FilteredPizzas))]
    public async Task Filter_ForPizzas_Return_ExpectedFilteredPizzas(
        string search,
        List<Pizza> expected
    )
    {
        // Arrange
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
        List<Pizza> expected
    )
    {
        // Arrange
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