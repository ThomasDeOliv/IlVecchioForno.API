using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Filters;

public sealed class IngredientFilterServiceTests : SeededInfrastructureTestsBase
{
    private readonly IFilterService<Ingredient> _filterService;

    public IngredientFilterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._filterService = new IngredientFilterService();
    }

    public static TheoryData<string, List<Ingredient>> FilteredIngredients =>
        new TheoryData<string, List<Ingredient>>
        {
            {
                string.Empty, new List<Ingredient>(DbMockedTestsData.TestsIngredients)
            },
            {
                "Sa", new List<Ingredient>([DbMockedTestsData.TestsIngredients[3], DbMockedTestsData.TestsIngredients[5]])
            }
        };

    [Theory]
    [MemberData(nameof(FilteredIngredients))]
    public async Task Filter_ForIngredients_Return_ExpectedFilteredIngredients(
        string search,
        List<Ingredient> expected
    )
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new SearchFilterType(search)
            }
        );
        List<Ingredient> collection = await queryResult.ToListAsync();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}