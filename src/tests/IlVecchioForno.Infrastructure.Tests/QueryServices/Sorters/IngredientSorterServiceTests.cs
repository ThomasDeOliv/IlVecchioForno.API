using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class IngredientSorterServiceTests : SeededInfrastructureTestsBase
{
    private readonly ISorterService<Ingredient, IngredientsSorter> _sorterService;

    public IngredientSorterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._sorterService = new IngredientSorterService();
    }

    public static TheoryData<bool, IngredientsSorter, List<int>> SortedIngredients =>
        new TheoryData<bool, IngredientsSorter, List<int>>
        {
            // Default
            {
                false, IngredientsSorter.Id, [..Enumerable.Range(0, 23)]
            },
            {
                true, IngredientsSorter.Id, [..Enumerable.Range(0, 23).Reverse()]
            },
            // Name
            {
                false, IngredientsSorter.Name, [15, 18, 20, 16, 22, 19, 21, 4, 0, 6, 2, 7, 9, 17, 8, 14, 10, 11, 12, 3, 5, 13, 1]
            },
            {
                true, IngredientsSorter.Name, [1, 13, 5, 3, 12, 11, 10, 14, 8, 17, 9, 7, 2, 6, 0, 4, 21, 19, 22, 16, 20, 18, 15]
            }
        };

    [Theory]
    [MemberData(nameof(SortedIngredients))]
    public async Task Sorter_ForIngredients_Return_ExpectedSortedIngredients(
        bool descending,
        IngredientsSorter sorter,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<Ingredient> expected = expectedIndexes.Select(i => this._ingredients[i]).ToList();
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<Ingredient> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}