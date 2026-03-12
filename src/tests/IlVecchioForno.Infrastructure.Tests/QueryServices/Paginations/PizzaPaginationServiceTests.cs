#pragma warning disable xUnit1045

using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
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

    public static TheoryData<int, int, List<Pizza>> PaginatedPizzas =>
        new TheoryData<int, int, List<Pizza>>
        {
            {
                1, 1, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[0]
                ])
            },
            {
                1, 4, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[0],
                    DbMockedTestsData.TestsPizzas[1],
                    DbMockedTestsData.TestsPizzas[2],
                    DbMockedTestsData.TestsPizzas[3]
                ])
            },
            {
                1, 15, new List<Pizza>(DbMockedTestsData.TestsPizzas)
            },
            {
                1, 100, new List<Pizza>(DbMockedTestsData.TestsPizzas)
            },
            {
                2, 4, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[4],
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[7]
                ])
            },
            {
                2, 5, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[5],
                    DbMockedTestsData.TestsPizzas[6],
                    DbMockedTestsData.TestsPizzas[7],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[9]
                ])
            },
            {
                3, 4, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[11]
                ])
            },
            {
                2, 7, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[7],
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[13]
                ])
            },
            {
                4, 4, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[13],
                    DbMockedTestsData.TestsPizzas[14]
                ])
            },
            {
                3, 5, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[13],
                    DbMockedTestsData.TestsPizzas[14]
                ])
            },
            {
                2, 8, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[8],
                    DbMockedTestsData.TestsPizzas[9],
                    DbMockedTestsData.TestsPizzas[10],
                    DbMockedTestsData.TestsPizzas[11],
                    DbMockedTestsData.TestsPizzas[12],
                    DbMockedTestsData.TestsPizzas[13],
                    DbMockedTestsData.TestsPizzas[14]
                ])
            },
            {
                3, 7, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[14]
                ])
            },
            {
                15, 1, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[14]
                ])
            },
            {
                8, 2, new List<Pizza>([
                    DbMockedTestsData.TestsPizzas[14]
                ])
            },
            {
                16, 1, new List<Pizza>()
            },
            {
                5, 4, new List<Pizza>()
            },
            {
                3, 8, new List<Pizza>()
            },
            {
                100, 10, new List<Pizza>()
            },
            {
                1, 0, new List<Pizza>()
            }
        };

    [Theory]
    [MemberData(nameof(PaginatedPizzas))]
    public async Task Paginate_ForPizzas_Return_ExpectedPizzas(
        int page,
        int pageSize,
        List<Pizza> expected
    )
    {
        // Arrange
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._paginationService.Paginate(queryable, page, pageSize);
        List<Pizza> collection = await queryResult.ToListAsync();
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
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync());
    }

    [Fact]
    public async Task Paginate_ForPizzas_ThrowException_WhenPageIsNegative()
    {
        // Arrange
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._paginationService.Paginate(queryable, -1, 100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync());
    }

    [Fact]
    public async Task Paginate_ForPizzas_ThrowException_WhenPageSizeIsNegative()
    {
        // Arrange
        IQueryable<Pizza> queryable = this._ctx.Pizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._paginationService.Paginate(queryable, 1, -100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync());
    }
}