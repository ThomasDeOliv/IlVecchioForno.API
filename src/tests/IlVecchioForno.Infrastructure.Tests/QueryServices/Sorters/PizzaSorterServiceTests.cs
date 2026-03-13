using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class PizzaSorterServiceTests : SeededInfrastructureTestsBase
{
    private readonly ISorterService<Pizza, PizzasSorter> _sorterService;

    public PizzaSorterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._sorterService = new PizzaSorterService();
    }

    public static TheoryData<bool, PizzasSorter, List<Pizza>> SortedPizzas =>
        new TheoryData<bool, PizzasSorter, List<Pizza>>
        {
            // Default
            {
                false, PizzasSorter.Id, DbMockedTestsData.TestsPizzas
            },
            {
                true, PizzasSorter.Id, DbMockedTestsData.TestsPizzas.AsEnumerable().Reverse().ToList()
            },
            // Name
            {
                false, PizzasSorter.Name, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[7],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[4],
                    DbMockedTestsData.TestsPizzas[3],
                    DbMockedTestsData.TestsPizzas[0],
                    DbMockedTestsData.TestsPizzas[1],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[13],
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[2],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[14],
                    DbMockedTestsData.TestsPizzas[12]
                }
            },
            {
                true, PizzasSorter.Name, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[14],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[2],
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[13],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[1],
                    DbMockedTestsData.TestsPizzas[0],
                    DbMockedTestsData.TestsPizzas[3],
                    DbMockedTestsData.TestsPizzas[4],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[7]
                }
            },
            // Price
            {
                false, PizzasSorter.Price, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[1],
                    DbMockedTestsData.TestsPizzas[0],
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[3],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[13],
                    DbMockedTestsData.TestsPizzas[2],
                    DbMockedTestsData.TestsPizzas[4],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[7],
                    DbMockedTestsData.TestsPizzas[14]
                }
            },
            {
                true, PizzasSorter.Price, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[14],
                    DbMockedTestsData.TestsPizzas[7],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[4],
                    DbMockedTestsData.TestsPizzas[2],
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[13],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[3],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[0],
                    DbMockedTestsData.TestsPizzas[1]
                }
            },
            // Archived
            {
                false, PizzasSorter.Archived, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[14],
                    DbMockedTestsData.TestsPizzas[0],
                    DbMockedTestsData.TestsPizzas[1],
                    DbMockedTestsData.TestsPizzas[2],
                    DbMockedTestsData.TestsPizzas[3],
                    DbMockedTestsData.TestsPizzas[4],
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[7],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[13]
                }
            },
            {
                true, PizzasSorter.Archived, new List<Pizza>
                {
                    DbMockedTestsData.TestsPizzas[14],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[0],
                    DbMockedTestsData.TestsPizzas[1],
                    DbMockedTestsData.TestsPizzas[2],
                    DbMockedTestsData.TestsPizzas[3],
                    DbMockedTestsData.TestsPizzas[4],
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[7],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[13]
                }
            }
        };

    [Theory]
    [MemberData(nameof(SortedPizzas))]
    public async Task Sorter_ForPizzas_Return_ExpectedSortedPizzas(
        bool descending,
        PizzasSorter sorter,
        List<Pizza> expected
    )
    {
        // Arrange
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<Pizza> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}