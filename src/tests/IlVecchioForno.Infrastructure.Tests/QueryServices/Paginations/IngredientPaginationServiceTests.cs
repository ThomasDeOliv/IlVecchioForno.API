#pragma warning disable xUnit1045

using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
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

    public static TheoryData<int, int, List<Ingredient>> PaginatedIngredients =>
        new TheoryData<int, int, List<Ingredient>>
        {
            {
                1, 1, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[0]
                ])
            },
            {
                1, 5, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[0],
                    DbMockedTestsData.TestsIngredients[1],
                    DbMockedTestsData.TestsIngredients[2],
                    DbMockedTestsData.TestsIngredients[3],
                    DbMockedTestsData.TestsIngredients[4]
                ])
            },
            {
                1, 10, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[0],
                    DbMockedTestsData.TestsIngredients[1],
                    DbMockedTestsData.TestsIngredients[2],
                    DbMockedTestsData.TestsIngredients[3],
                    DbMockedTestsData.TestsIngredients[4],
                    DbMockedTestsData.TestsIngredients[5],
                    DbMockedTestsData.TestsIngredients[6],
                    DbMockedTestsData.TestsIngredients[7],
                    DbMockedTestsData.TestsIngredients[8],
                    DbMockedTestsData.TestsIngredients[9]
                ])
            },
            {
                1, 23, new List<Ingredient>(
                    DbMockedTestsData.TestsIngredients
                )
            },
            {
                1, 100, new List<Ingredient>(
                    DbMockedTestsData.TestsIngredients
                )
            },
            {
                2, 5, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[5],
                    DbMockedTestsData.TestsIngredients[6],
                    DbMockedTestsData.TestsIngredients[7],
                    DbMockedTestsData.TestsIngredients[8],
                    DbMockedTestsData.TestsIngredients[9]
                ])
            },
            {
                2, 10, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[10],
                    DbMockedTestsData.TestsIngredients[11],
                    DbMockedTestsData.TestsIngredients[12],
                    DbMockedTestsData.TestsIngredients[13],
                    DbMockedTestsData.TestsIngredients[14],
                    DbMockedTestsData.TestsIngredients[15],
                    DbMockedTestsData.TestsIngredients[16],
                    DbMockedTestsData.TestsIngredients[17],
                    DbMockedTestsData.TestsIngredients[18],
                    DbMockedTestsData.TestsIngredients[19]
                ])
            },
            {
                3, 5, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[10],
                    DbMockedTestsData.TestsIngredients[11],
                    DbMockedTestsData.TestsIngredients[12],
                    DbMockedTestsData.TestsIngredients[13],
                    DbMockedTestsData.TestsIngredients[14]
                ])
            },
            {
                3, 7, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[14],
                    DbMockedTestsData.TestsIngredients[15],
                    DbMockedTestsData.TestsIngredients[16],
                    DbMockedTestsData.TestsIngredients[17],
                    DbMockedTestsData.TestsIngredients[18],
                    DbMockedTestsData.TestsIngredients[19],
                    DbMockedTestsData.TestsIngredients[20]
                ])
            },
            {
                3, 8, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[16],
                    DbMockedTestsData.TestsIngredients[17],
                    DbMockedTestsData.TestsIngredients[18],
                    DbMockedTestsData.TestsIngredients[19],
                    DbMockedTestsData.TestsIngredients[20],
                    DbMockedTestsData.TestsIngredients[21],
                    DbMockedTestsData.TestsIngredients[22]
                ])
            },
            {
                3, 10, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[20],
                    DbMockedTestsData.TestsIngredients[21],
                    DbMockedTestsData.TestsIngredients[22]
                ])
            },
            {
                4, 6, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[18],
                    DbMockedTestsData.TestsIngredients[19],
                    DbMockedTestsData.TestsIngredients[20],
                    DbMockedTestsData.TestsIngredients[21],
                    DbMockedTestsData.TestsIngredients[22]
                ])
            },
            {
                5, 5, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[20],
                    DbMockedTestsData.TestsIngredients[21],
                    DbMockedTestsData.TestsIngredients[22]
                ])
            },
            {
                23, 1, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[22]
                ])
            },
            {
                8, 3, new List<Ingredient>([
                    DbMockedTestsData.TestsIngredients[21],
                    DbMockedTestsData.TestsIngredients[22]
                ])
            },
            {
                24, 1, new List<Ingredient>()
            },
            {
                4, 8, new List<Ingredient>()
            },
            {
                5, 6, new List<Ingredient>()
            },
            {
                100, 10, new List<Ingredient>()
            },
            {
                1, 0, new List<Ingredient>()
            }
        };

    [Theory]
    [MemberData(nameof(PaginatedIngredients))]
    public async Task Paginate_ForIngredients_Return_ExpectedIngredients(
        int page,
        int pageSize,
        List<Ingredient> expected
    )
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._paginationService.Paginate(queryable, page, pageSize);
        List<Ingredient> collection = await queryResult.ToListAsync();
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
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync());
    }

    [Fact]
    public async Task Paginate_ForIngredients_ThrowException_WhenPageIsNegative()
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._paginationService.Paginate(queryable, -1, 100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync());
    }

    [Fact]
    public async Task Paginate_ForIngredients_ThrowException_WhenPageSizeIsNegative()
    {
        // Arrange
        IQueryable<Ingredient> queryable = this._ctx.Ingredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._paginationService.Paginate(queryable, 1, -100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync());
    }
}