#pragma warning disable xUnit1045

using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Paginations;

public sealed class IngredientPaginationServiceTests : SeededInfrastructureTestsBase
{
    private readonly IPaginationService<Ingredient> _paginationService;

    public IngredientPaginationServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._paginationService = new PaginationService<Ingredient>();
    }

    public static TheoryData<int, int, List<int>> PaginatedIngredients =>
        new TheoryData<int, int, List<int>>
        {
            {
                1, 1, [0]
            },
            {
                1, 5, [0, 1, 2, 3, 4]
            },
            {
                1, 10, [0, 1, 2, 3, 4, 5, 6, 7, 8, 9]
            },
            {
                1, 23, [..Enumerable.Range(0, 23)]
            },
            {
                1, 100, [..Enumerable.Range(0, 23)]
            },
            {
                2, 5, [5, 6, 7, 8, 9]
            },
            {
                2, 10, [10, 11, 12, 13, 14, 15, 16, 17, 18, 19]
            },
            {
                3, 5, [10, 11, 12, 13, 14]
            },
            {
                3, 7, [14, 15, 16, 17, 18, 19, 20]
            },
            {
                3, 8, [16, 17, 18, 19, 20, 21, 22]
            },
            {
                3, 10, [20, 21, 22]
            },
            {
                4, 6, [18, 19, 20, 21, 22]
            },
            {
                5, 5, [20, 21, 22]
            },
            {
                23, 1, [22]
            },
            {
                8, 3, [21, 22]
            },
            {
                24, 1, []
            },
            {
                4, 8, []
            },
            {
                5, 6, []
            },
            {
                100, 10, []
            },
            {
                1, 0, []
            }
        };

    [Theory]
    [MemberData(nameof(PaginatedIngredients))]
    public async Task Paginate_ForIngredients_Return_ExpectedIngredients(
        int page,
        int pageSize,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<Ingredient> expected = expectedIndexes.Select(i => this._ingredients[i]).ToList();
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._paginationService.Paginate(queryable, page, pageSize);
        List<Ingredient> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }

    [Fact]
    public async Task Paginate_ForIngredients_ThrowException_WhenPageIsZero()
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._paginationService.Paginate(queryable, 0, 100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Paginate_ForIngredients_ThrowException_WhenPageIsNegative()
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._paginationService.Paginate(queryable, -1, 100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Paginate_ForIngredients_ThrowException_WhenPageSizeIsNegative()
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._paginationService.Paginate(queryable, 1, -100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }
}