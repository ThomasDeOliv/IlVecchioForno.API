#pragma warning disable xUnit1045

using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Paginations;

public sealed class QuantityTypePaginationServiceTests
{
    private static readonly List<QuantityType> _testsQuantityTypes = new List<QuantityType>
    {
        new QuantityType(new QuantityTypeName("Milligrams"), new QuantityTypeUnit("mg")),
        new QuantityType(new QuantityTypeName("Grams"), new QuantityTypeUnit("g")),
        new QuantityType(new QuantityTypeName("Kilograms"), new QuantityTypeUnit("kg")),
        new QuantityType(new QuantityTypeName("Milliliters"), new QuantityTypeUnit("mL")),
        new QuantityType(new QuantityTypeName("Centiliters"), new QuantityTypeUnit("cL")),
        new QuantityType(new QuantityTypeName("Liters"), new QuantityTypeUnit("L"))
    };

    private readonly IPaginationService<QuantityType> _paginationService;

    public QuantityTypePaginationServiceTests()
    {
        this._paginationService = new PaginationService<QuantityType>();
    }

    public static TheoryData<int, int, List<QuantityType>> PaginatedQuantityTypes =>
        new TheoryData<int, int, List<QuantityType>>
        {
            // Positive cases
            { 1, 1, new List<QuantityType>([_testsQuantityTypes[0]]) },
            { 1, 2, new List<QuantityType>([_testsQuantityTypes[0], _testsQuantityTypes[1]]) },
            { 1, 3, new List<QuantityType>([_testsQuantityTypes[0], _testsQuantityTypes[1], _testsQuantityTypes[2]]) },
            { 1, 6, new List<QuantityType>(_testsQuantityTypes) },
            { 1, 100, new List<QuantityType>(_testsQuantityTypes) },
            { 1, 10000, new List<QuantityType>(_testsQuantityTypes) },
            { 2, 2, new List<QuantityType>([_testsQuantityTypes[2], _testsQuantityTypes[3]]) },
            { 2, 3, new List<QuantityType>([_testsQuantityTypes[3], _testsQuantityTypes[4], _testsQuantityTypes[5]]) },
            { 2, 4, new List<QuantityType>([_testsQuantityTypes[4], _testsQuantityTypes[5]]) },
            { 3, 2, new List<QuantityType>([_testsQuantityTypes[4], _testsQuantityTypes[5]]) },
            { 3, 3, new List<QuantityType>() },
            { 5, 1, new List<QuantityType>([_testsQuantityTypes[4]]) },
            { 5, 1000, new List<QuantityType>() },
            { 6, 1, new List<QuantityType>([_testsQuantityTypes[5]]) },
            { 10, 1, new List<QuantityType>() },

            // Negative/zero cases
            { 0, 3, new List<QuantityType>([_testsQuantityTypes[0], _testsQuantityTypes[1], _testsQuantityTypes[2]]) },
            { -1, 2, new List<QuantityType>([_testsQuantityTypes[0], _testsQuantityTypes[1]]) },
            { 1, 0, new List<QuantityType>() },
            { 1, -5, new List<QuantityType>() },
            { 0, 0, new List<QuantityType>() },
            { -1, -1, new List<QuantityType>() },
            { -2, -3, new List<QuantityType>() }
        };

    [Theory]
    [MemberData(nameof(PaginatedQuantityTypes))]
    public void Paginate_ForQuantityTypes_Return_ExpectedQuantityTypes(
        int page,
        int pageSize,
        List<QuantityType> expected
    )
    {
        // Arrange
        IQueryable<QuantityType> queryable = _testsQuantityTypes.AsQueryable();
        // Act
        IQueryable<QuantityType> queryResult = this._paginationService.Paginate(queryable, page, pageSize);
        List<QuantityType> collection = queryResult.ToList();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}