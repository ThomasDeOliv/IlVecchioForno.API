using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class QuantityTypeSorterServiceTests
{
    private static readonly List<QuantityType> _testsQuantityTypes = new List<QuantityType>
    {
        new QuantityType(new QuantityTypeName("Milligrams"), new QuantityTypeUnit("mg"), 1),
        new QuantityType(new QuantityTypeName("Grams"), new QuantityTypeUnit("g"), 2),
        new QuantityType(new QuantityTypeName("Kilograms"), new QuantityTypeUnit("kg"), 3),
        new QuantityType(new QuantityTypeName("Milliliters"), new QuantityTypeUnit("mL"), 4),
        new QuantityType(new QuantityTypeName("Centiliters"), new QuantityTypeUnit("cL"), 5),
        new QuantityType(new QuantityTypeName("Liters"), new QuantityTypeUnit("L"), 6)
    };

    private readonly ISorterService<QuantityType, QuantityTypesSorter> _sorterService;

    public QuantityTypeSorterServiceTests()
    {
        this._sorterService = new QuantityTypeSorterService();
    }

    public static TheoryData<bool, QuantityTypesSorter, List<QuantityType>> SortedQuantityTypes =>
        new TheoryData<bool, QuantityTypesSorter, List<QuantityType>>
        {
            // Default
            {
                false,
                QuantityTypesSorter.Id,
                _testsQuantityTypes
            },
            {
                true,
                QuantityTypesSorter.Id,
                _testsQuantityTypes.AsEnumerable().Reverse().ToList()
            },
            // Name 
            {
                false,
                QuantityTypesSorter.Name,
                new List<QuantityType>
                {
                    _testsQuantityTypes[4],
                    _testsQuantityTypes[1],
                    _testsQuantityTypes[2],
                    _testsQuantityTypes[5],
                    _testsQuantityTypes[0],
                    _testsQuantityTypes[3]
                }
            },
            {
                true,
                QuantityTypesSorter.Name,
                new List<QuantityType>
                {
                    _testsQuantityTypes[4],
                    _testsQuantityTypes[1],
                    _testsQuantityTypes[2],
                    _testsQuantityTypes[5],
                    _testsQuantityTypes[0],
                    _testsQuantityTypes[3]
                }.AsEnumerable().Reverse().ToList()
            },
            // Unit
            {
                false,
                QuantityTypesSorter.Unit,
                new List<QuantityType>
                {
                    _testsQuantityTypes[4],
                    _testsQuantityTypes[1],
                    _testsQuantityTypes[2],
                    _testsQuantityTypes[5],
                    _testsQuantityTypes[0],
                    _testsQuantityTypes[3]
                }
            },
            {
                true,
                QuantityTypesSorter.Unit,
                new List<QuantityType>
                {
                    _testsQuantityTypes[4],
                    _testsQuantityTypes[1],
                    _testsQuantityTypes[2],
                    _testsQuantityTypes[5],
                    _testsQuantityTypes[0],
                    _testsQuantityTypes[3]
                }.AsEnumerable().Reverse().ToList()
            }
        };

    [Theory]
    [MemberData(nameof(SortedQuantityTypes))]
    public void Sorter_ForQuantityTypes_Return_ExpectedSortedQuantityTypes(
        bool descending,
        QuantityTypesSorter sorter,
        List<QuantityType> expected
    )
    {
        // Arrange
        IQueryable<QuantityType> queryable = _testsQuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<QuantityType> collection = queryResult.ToList();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}