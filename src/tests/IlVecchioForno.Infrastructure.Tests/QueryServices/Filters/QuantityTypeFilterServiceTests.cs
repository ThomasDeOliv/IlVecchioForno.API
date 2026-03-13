using IlVecchioForno.Application.Gateways.Persistence.Queries.FilterTypes;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Common.Exceptions;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Filters;
using IlVecchioForno.Infrastructure.Tests.Utilities.Models;
using Microsoft.EntityFrameworkCore;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Filters;

public sealed class QuantityTypeFilterServiceTests : SeededInfrastructureTestsBase
{
    private readonly IFilterService<QuantityType> _filterService;

    public QuantityTypeFilterServiceTests(DbContextFixture dbContextFixture) : base(dbContextFixture)
    {
        this._filterService = new QuantityTypeFilterService();
    }

    public static TheoryData<string, List<int>> FilteredQuantityTypes =>
        new TheoryData<string, List<int>>
        {
            {
                string.Empty, [..Enumerable.Range(0, 6)]
            },
            {
                "Milligrams", [0]
            },
            {
                "MillIGrAmS", [0]
            },
            {
                "cl", [4]
            },
            {
                "cL", [4]
            },
            {
                "CL", [4]
            },
            {
                "CL", [4]
            },
            {
                "CM", []
            }
        };

    [Theory]
    [MemberData(nameof(FilteredQuantityTypes))]
    public async Task Filter_ForQuantityTypes_Return_ExpectedFilteredQuantityTypes(
        string search,
        List<int> expectedIndexes
    )
    {
        // Arrange
        List<QuantityType> expected = expectedIndexes.Select(i => this._quantityTypes[i]).ToList();
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new SearchFilterType(search)
            }
        );
        List<QuantityType> collection = await queryResult.ToListAsync(TestContext.Current.CancellationToken);
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }

    [Fact]
    public void Filter_ForQuantityTypes_ThrowNotSupportedFilterException_WhenUnsupportedFilterProvided()
    {
        // Arrange
        IQueryable<QuantityType> queryable = this._ctx.QuantityTypes.AsQueryable();
        // Act & Assert
        NotSupportedFilterException e = Assert.Throws<NotSupportedFilterException>(() => this._filterService.Filter(
            queryable, new List<IFilterType>
            {
                new FakeFilter()
            }
        ));
        Assert.Equal("Provided filter is not supported for entity QuantityType.", e.Message);
        Assert.NotNull(e.InnerException);
        Assert.Equal("Filter type FakeFilter is not supported.", e.InnerException.Message);
    }
}