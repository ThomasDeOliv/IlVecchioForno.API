using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Models;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Filters;

public sealed class IngredientFilterServiceTests : SeededInfrastructureTestsBase
{
    private readonly IFilterService<Ingredient> _filterService;

    public IngredientFilterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._filterService = new IngredientFilterService();
    }

    public static TheoryData<string, List<int>> FilteredIngredients =>
        new TheoryData<string, List<int>>
        {
            {
                string.Empty, [..Enumerable.Range(0, 23)]
            },
            {
                "Sa", [3, 5]
            },
            {
                "sa", [3, 5]
            },
            {
                "SA", [3, 5]
            },
            {
                "sA", [3, 5]
            },
            {
                "'Nduja", [15]
            },
            {
                "kl", []
            }
        };

    [Theory]
    [MemberData(nameof(FilteredIngredients))]
    public async Task Filter_ForIngredients_Return_ExpectedFilteredIngredients(
        string search,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<Ingredient> expected = expectedIndexes.Select(i => this._ingredients[i]).ToList();
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new SearchFilterType(search)
            }
        );
        List<Ingredient> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }

    [Fact]
    public void Filter_ForIngredients_ThrowNotSupportedFilterException_WhenUnsupportedFilterProvided()
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act & Assert
        NotSupportedFilterException e = Assert.Throws<NotSupportedFilterException>(() => this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new FakeFilter()
            }
        ));
        Assert.Equal("Provided filter is not supported for entity Ingredient.", e.Message);
        Assert.NotNull(e.InnerException);
        Assert.Equal("Filter type FakeFilter is not supported.", e.InnerException.Message);
    }
}