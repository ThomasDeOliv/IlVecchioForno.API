using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class QuantityTypeSorterServiceTests : SeededInfrastructureTestsBase
{
    private readonly ISorterService<QuantityType, QuantityTypesSorter> _sorterService;

    public QuantityTypeSorterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._sorterService = new QuantityTypeSorterService();
    }

    public static TheoryData<bool, QuantityTypesSorter, List<int>> SortedQuantityTypes =>
        new TheoryData<bool, QuantityTypesSorter, List<int>>
        {
            // Default
            {
                false, QuantityTypesSorter.Id, [..Enumerable.Range(0, 6)]
            },
            {
                true, QuantityTypesSorter.Id, [..Enumerable.Range(0, 6).Reverse()]
            },
            // Name
            {
                false, QuantityTypesSorter.Name, [4, 1, 2, 5, 0, 3]
            },
            {
                true, QuantityTypesSorter.Name, [3, 0, 5, 2, 1, 4]
            },
            // Unit
            {
                false, QuantityTypesSorter.Unit, [4, 1, 2, 5, 0, 3]
            },
            {
                true, QuantityTypesSorter.Unit, [3, 0, 5, 2, 1, 4]
            }
        };

    [Theory]
    [MemberData(nameof(SortedQuantityTypes))]
    public async Task Sorter_ForQuantityTypes_Return_ExpectedSortedQuantityTypes(
        bool descending,
        QuantityTypesSorter sorter,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<QuantityType> expected = expectedIndexes.Select(i => this._quantityTypes[i]).ToList();
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<QuantityType> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}