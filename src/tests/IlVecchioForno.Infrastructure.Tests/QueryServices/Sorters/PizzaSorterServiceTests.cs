using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class PizzaSorterServiceTests : SeededInfrastructureTestsBase
{
    private readonly ISorterService<Pizza, PizzasSorter> _sorterService;

    public PizzaSorterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._sorterService = new PizzaSorterService();
    }

    public static TheoryData<bool, PizzasSorter, List<int>> SortedPizzas =>
        new TheoryData<bool, PizzasSorter, List<int>>
        {
            // Default
            {
                false, PizzasSorter.Id, [..Enumerable.Range(0, 15)]
            },
            {
                true, PizzasSorter.Id, [..Enumerable.Range(0, 15).Reverse()]
            },
            // Name
            {
                false, PizzasSorter.Name, [7, 8, 4, 3, 0, 1, 9, 11, 13, 5, 10, 2, 6, 14, 12]
            },
            {
                true, PizzasSorter.Name, [12, 14, 6, 2, 10, 5, 13, 11, 9, 1, 0, 3, 4, 8, 7]
            },
            // Price
            {
                false, PizzasSorter.Price, [1, 0, 10, 9, 3, 11, 8, 12, 5, 13, 2, 4, 6, 7, 14]
            },
            {
                true, PizzasSorter.Price, [14, 7, 6, 4, 2, 5, 13, 8, 12, 3, 11, 9, 10, 0, 1]
            },
            // Archived
            {
                false, PizzasSorter.Archived, [10, 11, 14, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 12, 13]
            },
            {
                true, PizzasSorter.Archived, [14, 11, 10, 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 12, 13]
            }
        };

    [Theory]
    [MemberData(nameof(SortedPizzas))]
    public async Task Sorter_ForPizzas_Return_ExpectedSortedPizzas(
        bool descending,
        PizzasSorter sorter,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<Pizza> expected = expectedIndexes.Select(i => this._pizzas[i]).ToList();
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<Pizza> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}