#pragma warning disable xUnit1045

using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
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

    public static TheoryData<int, int, List<QuantityType>> PaginatedQuantityTypes =>
        new TheoryData<int, int, List<QuantityType>>
        {
            {
                1, 1, new List<QuantityType>([
                    DbMockedTestsData.TestsQuantityTypes[0]
                ])
            },
            {
                1, 2, new List<QuantityType>([DbMockedTestsData.TestsQuantityTypes[0], DbMockedTestsData.TestsQuantityTypes[1]])
            },
            {
                1, 3, new List<QuantityType>([DbMockedTestsData.TestsQuantityTypes[0], DbMockedTestsData.TestsQuantityTypes[1], DbMockedTestsData.TestsQuantityTypes[2]])
            },
            {
                1, 6, new List<QuantityType>(DbMockedTestsData.TestsQuantityTypes)
            },
            {
                1, 100, new List<QuantityType>(DbMockedTestsData.TestsQuantityTypes)
            },
            {
                1, 10000, new List<QuantityType>(DbMockedTestsData.TestsQuantityTypes)
            },
            {
                2, 2, new List<QuantityType>([DbMockedTestsData.TestsQuantityTypes[2], DbMockedTestsData.TestsQuantityTypes[3]])
            },
            {
                2, 3, new List<QuantityType>([DbMockedTestsData.TestsQuantityTypes[3], DbMockedTestsData.TestsQuantityTypes[4], DbMockedTestsData.TestsQuantityTypes[5]])
            },
            {
                2, 4, new List<QuantityType>([DbMockedTestsData.TestsQuantityTypes[4], DbMockedTestsData.TestsQuantityTypes[5]])
            },
            {
                3, 2, new List<QuantityType>([
                    DbMockedTestsData.TestsQuantityTypes[4],
                    DbMockedTestsData.TestsQuantityTypes[5]
                ])
            },
            {
                3, 3, new List<QuantityType>()
            },
            {
                5, 1, new List<QuantityType>([
                    DbMockedTestsData.TestsQuantityTypes[4]
                ])
            },
            {
                5, 1000, new List<QuantityType>()
            },
            {
                6, 1, new List<QuantityType>([
                    DbMockedTestsData.TestsQuantityTypes[5]
                ])
            },
            {
                10, 1, new List<QuantityType>()
            },
            {
                1, 0, new List<QuantityType>()
            }
        };

    [Theory]
    [MemberData(nameof(PaginatedQuantityTypes))]
    public async Task Paginate_ForQuantityTypes_Return_ExpectedQuantityTypes(
        int page,
        int pageSize,
        List<QuantityType> expected
    )
    {
        // Arrange
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