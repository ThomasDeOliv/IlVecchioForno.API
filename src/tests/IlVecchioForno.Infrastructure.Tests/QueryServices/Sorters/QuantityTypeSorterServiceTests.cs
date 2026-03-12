using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class QuantityTypeSorterServiceTests : SeededInfrastructureTestsBase
{
    private readonly ISorterService<QuantityType, QuantityTypesSorter> _sorterService;

    public QuantityTypeSorterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._sorterService = new QuantityTypeSorterService();
    }

    public static TheoryData<bool, QuantityTypesSorter, List<QuantityType>> SortedQuantityTypes =>
        new TheoryData<bool, QuantityTypesSorter, List<QuantityType>>
        {
            // Default
            {
                false, QuantityTypesSorter.Id, DbMockedTestsData.TestsQuantityTypes
            },
            {
                true, QuantityTypesSorter.Id, DbMockedTestsData.TestsQuantityTypes.AsEnumerable().Reverse().ToList()
            },
            // Name 
            {
                false, QuantityTypesSorter.Name, new List<QuantityType>
                {
                    DbMockedTestsData.TestsQuantityTypes[4],
                    DbMockedTestsData.TestsQuantityTypes[1],
                    DbMockedTestsData.TestsQuantityTypes[2],
                    DbMockedTestsData.TestsQuantityTypes[5],
                    DbMockedTestsData.TestsQuantityTypes[0],
                    DbMockedTestsData.TestsQuantityTypes[3]
                }
            },
            {
                true, QuantityTypesSorter.Name, new List<QuantityType>
                {
                    DbMockedTestsData.TestsQuantityTypes[4],
                    DbMockedTestsData.TestsQuantityTypes[1],
                    DbMockedTestsData.TestsQuantityTypes[2],
                    DbMockedTestsData.TestsQuantityTypes[5],
                    DbMockedTestsData.TestsQuantityTypes[0],
                    DbMockedTestsData.TestsQuantityTypes[3]
                }.AsEnumerable().Reverse().ToList()
            },
            // Unit
            {
                false, QuantityTypesSorter.Unit, new List<QuantityType>
                {
                    DbMockedTestsData.TestsQuantityTypes[4],
                    DbMockedTestsData.TestsQuantityTypes[1],
                    DbMockedTestsData.TestsQuantityTypes[2],
                    DbMockedTestsData.TestsQuantityTypes[5],
                    DbMockedTestsData.TestsQuantityTypes[0],
                    DbMockedTestsData.TestsQuantityTypes[3]
                }
            },
            {
                true, QuantityTypesSorter.Unit, new List<QuantityType>
                {
                    DbMockedTestsData.TestsQuantityTypes[4],
                    DbMockedTestsData.TestsQuantityTypes[1],
                    DbMockedTestsData.TestsQuantityTypes[2],
                    DbMockedTestsData.TestsQuantityTypes[5],
                    DbMockedTestsData.TestsQuantityTypes[0],
                    DbMockedTestsData.TestsQuantityTypes[3]
                }.AsEnumerable().Reverse().ToList()
            }
        };

    [Theory]
    [MemberData(nameof(SortedQuantityTypes))]
    public async Task Sorter_ForQuantityTypes_Return_ExpectedSortedQuantityTypes(
        bool descending,
        QuantityTypesSorter sorter,
        List<QuantityType> expected
    )
    {
        // Arrange
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<QuantityType> collection = await queryResult.ToListAsync();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}