#pragma warning disable xUnit1045

using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Paginations;

public sealed class PizzaPaginationServiceTests : SeededInfrastructureTestsBase
{
    private readonly IPaginationService<Pizza> _paginationService;

    public PizzaPaginationServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._paginationService = new PaginationService<Pizza>();
    }

    public static TheoryData<int, int, List<int>> PaginatedPizzas =>
        new TheoryData<int, int, List<int>>
        {
            {
                1, 1, [0]
            },
            {
                1, 4, [0, 1, 2, 3]
            },
            {
                1, 15, [..Enumerable.Range(0, 15)]
            },
            {
                1, 100, [..Enumerable.Range(0, 15)]
            },
            {
                2, 4, [4, 5, 6, 7]
            },
            {
                2, 5, [5, 6, 7, 8, 9]
            },
            {
                3, 4, [8, 9, 10, 11]
            },
            {
                2, 7, [7, 8, 9, 10, 11, 12, 13]
            },
            {
                4, 4, [12, 13, 14]
            },
            {
                3, 5, [10, 11, 12, 13, 14]
            },
            {
                2, 8, [8, 9, 10, 11, 12, 13, 14]
            },
            {
                3, 7, [14]
            },
            {
                15, 1, [14]
            },
            {
                8, 2, [14]
            },
            {
                16, 1, []
            },
            {
                5, 4, []
            },
            {
                3, 8, []
            },
            {
                100, 10, []
            },
            {
                1, 0, []
            }
        };

    [Theory]
    [MemberData(nameof(PaginatedPizzas))]
    public async Task Paginate_ForPizzas_Return_ExpectedPizzas(
        int page,
        int pageSize,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<Pizza> expected = expectedIndexes.Select(i => this._pizzas[i]).ToList();
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._paginationService.Paginate(queryable, page, pageSize);
        List<Pizza> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }

    [Fact]
    public async Task Paginate_ForPizzas_ThrowException_WhenPageIsZero()
    {
        // Arrange
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._paginationService.Paginate(queryable, 0, 100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Paginate_ForPizzas_ThrowException_WhenPageIsNegative()
    {
        // Arrange
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._paginationService.Paginate(queryable, -1, 100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Paginate_ForPizzas_ThrowException_WhenPageSizeIsNegative()
    {
        // Arrange
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._paginationService.Paginate(queryable, 1, -100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }
}