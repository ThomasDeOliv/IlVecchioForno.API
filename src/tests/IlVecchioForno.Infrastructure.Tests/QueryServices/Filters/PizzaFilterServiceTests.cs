using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
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
            }
        };

    public static TheoryData<decimal?, decimal?, List<Pizza>> RangeFilteredPizzas =>
        new TheoryData<decimal?, decimal?, List<Pizza>>();

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
        List<Pizza> collection = await queryResult.ToListAsync();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}