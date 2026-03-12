using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Data;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Filters;

public sealed class QuantityTypeFilterServiceTests : SeededInfrastructureTestsBase
{
    private readonly IFilterService<QuantityType> _filterService;

    public QuantityTypeFilterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._filterService = new QuantityTypeFilterService();
    }

    public static TheoryData<string, List<QuantityType>> FilteredQuantityTypes =>
        new TheoryData<string, List<QuantityType>>
        {
            {
                string.Empty, new List<QuantityType>(DbMockedTestsData.TestsQuantityTypes)
            }
        };

    [Theory]
    [MemberData(nameof(FilteredQuantityTypes))]
    public async Task Filter_ForQuantityTypes_Return_ExpectedFilteredQuantityTypes(
        string search,
        List<QuantityType> expected
    )
    {
        // Arrange
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new SearchFilterType(search)
            }
        );
        List<QuantityType> collection = await queryResult.ToListAsync();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}