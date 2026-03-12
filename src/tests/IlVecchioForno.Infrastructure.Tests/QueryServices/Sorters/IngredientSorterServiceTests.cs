using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class IngredientSorterServiceTests : SeededInfrastructureTestsBase
{
    private readonly ISorterService<Ingredient, IngredientsSorter> _sorterService;

    public IngredientSorterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._sorterService = new IngredientSorterService();
    }

    public static TheoryData<bool, IngredientsSorter, List<Ingredient>> SortedIngredients =>
        new TheoryData<bool, IngredientsSorter, List<Ingredient>>
        {
            // Default
            {
                false, IngredientsSorter.Id, DbMockedTestsData.TestsIngredients
            },
            {
                true, IngredientsSorter.Id, DbMockedTestsData.TestsIngredients.AsEnumerable().Reverse().ToList()
            },
            // Name
            {
                false, IngredientsSorter.Name, new List<Ingredient>
                {
                    DbMockedTestsData.TestsIngredients[15],
                    DbMockedTestsData.TestsIngredients[18],
                    DbMockedTestsData.TestsIngredients[20],
                    DbMockedTestsData.TestsIngredients[16],
                    DbMockedTestsData.TestsIngredients[22],
                    DbMockedTestsData.TestsIngredients[19],
                    DbMockedTestsData.TestsIngredients[21],
                    DbMockedTestsData.TestsIngredients[4],
                    DbMockedTestsData.TestsIngredients[0],
                    DbMockedTestsData.TestsIngredients[6],
                    DbMockedTestsData.TestsIngredients[2],
                    DbMockedTestsData.TestsIngredients[7],
                    DbMockedTestsData.TestsIngredients[9],
                    DbMockedTestsData.TestsIngredients[17],
                    DbMockedTestsData.TestsIngredients[8],
                    DbMockedTestsData.TestsIngredients[14],
                    DbMockedTestsData.TestsIngredients[10],
                    DbMockedTestsData.TestsIngredients[11],
                    DbMockedTestsData.TestsIngredients[12],
                    DbMockedTestsData.TestsIngredients[3],
                    DbMockedTestsData.TestsIngredients[5],
                    DbMockedTestsData.TestsIngredients[13],
                    DbMockedTestsData.TestsIngredients[1]
                }
            },
            {
                true, IngredientsSorter.Name, new List<Ingredient>
                {
                    DbMockedTestsData.TestsIngredients[1],
                    DbMockedTestsData.TestsIngredients[13],
                    DbMockedTestsData.TestsIngredients[5],
                    DbMockedTestsData.TestsIngredients[3],
                    DbMockedTestsData.TestsIngredients[12],
                    DbMockedTestsData.TestsIngredients[11],
                    DbMockedTestsData.TestsIngredients[10],
                    DbMockedTestsData.TestsIngredients[14],
                    DbMockedTestsData.TestsIngredients[8],
                    DbMockedTestsData.TestsIngredients[17],
                    DbMockedTestsData.TestsIngredients[9],
                    DbMockedTestsData.TestsIngredients[7],
                    DbMockedTestsData.TestsIngredients[2],
                    DbMockedTestsData.TestsIngredients[6],
                    DbMockedTestsData.TestsIngredients[0],
                    DbMockedTestsData.TestsIngredients[4],
                    DbMockedTestsData.TestsIngredients[21],
                    DbMockedTestsData.TestsIngredients[19],
                    DbMockedTestsData.TestsIngredients[22],
                    DbMockedTestsData.TestsIngredients[16],
                    DbMockedTestsData.TestsIngredients[20],
                    DbMockedTestsData.TestsIngredients[18],
                    DbMockedTestsData.TestsIngredients[15]
                }
            }
        };

    [Theory]
    [MemberData(nameof(SortedIngredients))]
    public async Task Sorter_ForIngredients_Return_ExpectedSortedIngredients(
        bool descending,
        IngredientsSorter sorter,
        List<Ingredient> expected
    )
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<Ingredient> collection = await queryResult.ToListAsync();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}