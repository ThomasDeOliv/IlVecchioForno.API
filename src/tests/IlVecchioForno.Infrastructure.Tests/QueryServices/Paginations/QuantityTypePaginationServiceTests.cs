#pragma warning disable xUnit1045

using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using Microsoft.EntityFrameworkCore;
using Npgsql;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Paginations;

public sealed class QuantityTypePaginationServiceTests : SeededInfrastructureTestsBase
{
    private readonly IPaginationService<QuantityType> _paginationService;

    public QuantityTypePaginationServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._paginationService = new PaginationService<QuantityType>();
    }

    public static TheoryData<int, int, List<int>> PaginatedQuantityTypes =>
        new TheoryData<int, int, List<int>>
        {
            {
                1, 1, [0]
            },
            {
                1, 2, [0, 1]
            },
            {
                1, 3, [0, 1, 2]
            },
            {
                1, 6, [..Enumerable.Range(0, 6)]
            },
            {
                1, 100, [..Enumerable.Range(0, 6)]
            },
            {
                1, 10000, [..Enumerable.Range(0, 6)]
            },
            {
                2, 2, [2, 3]
            },
            {
                2, 3, [3, 4, 5]
            },
            {
                2, 4, [4, 5]
            },
            {
                3, 2, [4, 5]
            },
            {
                3, 3, []
            },
            {
                5, 1, [4]
            },
            {
                5, 1000, []
            },
            {
                6, 1, [5]
            },
            {
                10, 1, []
            },
            {
                1, 0, []
            }
        };

    [Theory]
    [MemberData(nameof(PaginatedQuantityTypes))]
    public async Task Paginate_ForQuantityTypes_Return_ExpectedQuantityTypes(
        int page,
        int pageSize,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<QuantityType> expected = expectedIndexes.Select(i => this._quantityTypes[i]).ToList();
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._paginationService.Paginate(queryable, page, pageSize);
        List<QuantityType> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }

    [Fact]
    public async Task Paginate_ForQuantityTypes_ThrowException_WhenPageIsZero()
    {
        // Arrange
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._paginationService.Paginate(queryable, 0, 100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Paginate_ForQuantityTypes_ThrowException_WhenPageIsNegative()
    {
        // Arrange
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._paginationService.Paginate(queryable, -1, 100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }

    [Fact]
    public async Task Paginate_ForPizzas_ThrowException_WhenPageSizeIsNegative()
    {
        // Arrange
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._paginationService.Paginate(queryable, 1, -100);
        // Assert
        await Assert.ThrowsAsync<PostgresException>(() => queryResult.ToListAsync(TestContext.Current.CancellationToken));
    }
}