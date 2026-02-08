#pragma warning disable xUnit1045

using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Paginations;

public sealed class IngredientPaginationServiceTests
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

    private static readonly List<Ingredient> _testsIngredients = new List<Ingredient>
    {
        new Ingredient(new IngredientName("Flour (00)"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Water"), _testsQuantityTypes[3]),
        new Ingredient(new IngredientName("Fresh yeast"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Salt"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Extra virgin olive oil"), _testsQuantityTypes[3]),
        new Ingredient(new IngredientName("San Marzano tomato sauce"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Fresh basil leaves"), null),
        new Ingredient(new IngredientName("Garlic clove"), null),
        new Ingredient(new IngredientName("Oregano"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Mozzarella fior di latte"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Parmigiano Reggiano"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Pecorino Romano"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Prosciutto crudo"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Spianata calabra"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Pancetta"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("'Nduja"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Black olives"), null),
        new Ingredient(new IngredientName("Mushrooms"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Artichoke hearts"), null),
        new Ingredient(new IngredientName("Capers"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Arugula"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Cherry tomatoes"), _testsQuantityTypes[1]),
        new Ingredient(new IngredientName("Buffalo mozzarella"), _testsQuantityTypes[1])
    };

    private readonly IPaginationService<Ingredient> _paginationService;

    public IngredientPaginationServiceTests()
    {
        this._paginationService = new PaginationService<Ingredient>();
    }

    public static TheoryData<int, int, List<Ingredient>> PaginatedIngredients =>
        new TheoryData<int, int, List<Ingredient>>
        {
            // Positive cases
            {
                1,
                1,
                new List<Ingredient>([
                    _testsIngredients[0]
                ])
            },
            {
                1,
                5,
                new List<Ingredient>([
                    _testsIngredients[0],
                    _testsIngredients[1],
                    _testsIngredients[2],
                    _testsIngredients[3],
                    _testsIngredients[4]
                ])
            },
            {
                1,
                10,
                new List<Ingredient>([
                    _testsIngredients[0],
                    _testsIngredients[1],
                    _testsIngredients[2],
                    _testsIngredients[3],
                    _testsIngredients[4],
                    _testsIngredients[5],
                    _testsIngredients[6],
                    _testsIngredients[7],
                    _testsIngredients[8],
                    _testsIngredients[9]
                ])
            },
            {
                1,
                23,
                new List<Ingredient>(
                    _testsIngredients
                )
            },
            {
                1,
                100,
                new List<Ingredient>(
                    _testsIngredients
                )
            },
            {
                2,
                5,
                new List<Ingredient>([
                    _testsIngredients[5],
                    _testsIngredients[6],
                    _testsIngredients[7],
                    _testsIngredients[8],
                    _testsIngredients[9]
                ])
            },
            {
                2,
                10,
                new List<Ingredient>([
                    _testsIngredients[10],
                    _testsIngredients[11],
                    _testsIngredients[12],
                    _testsIngredients[13],
                    _testsIngredients[14],
                    _testsIngredients[15],
                    _testsIngredients[16],
                    _testsIngredients[17],
                    _testsIngredients[18],
                    _testsIngredients[19]
                ])
            },
            {
                3,
                5,
                new List<Ingredient>([
                    _testsIngredients[10],
                    _testsIngredients[11],
                    _testsIngredients[12],
                    _testsIngredients[13],
                    _testsIngredients[14]
                ])
            },
            {
                3,
                7,
                new List<Ingredient>([
                    _testsIngredients[14],
                    _testsIngredients[15],
                    _testsIngredients[16],
                    _testsIngredients[17],
                    _testsIngredients[18],
                    _testsIngredients[19],
                    _testsIngredients[20]
                ])
            },
            {
                3,
                8,
                new List<Ingredient>([
                    _testsIngredients[16],
                    _testsIngredients[17],
                    _testsIngredients[18],
                    _testsIngredients[19],
                    _testsIngredients[20],
                    _testsIngredients[21],
                    _testsIngredients[22]
                ])
            },
            {
                3,
                10,
                new List<Ingredient>([
                    _testsIngredients[20],
                    _testsIngredients[21],
                    _testsIngredients[22]
                ])
            },
            {
                4,
                6,
                new List<Ingredient>([
                    _testsIngredients[18],
                    _testsIngredients[19],
                    _testsIngredients[20],
                    _testsIngredients[21],
                    _testsIngredients[22]
                ])
            },
            {
                5,
                5,
                new List<Ingredient>([
                    _testsIngredients[20],
                    _testsIngredients[21],
                    _testsIngredients[22]
                ])
            },
            {
                23,
                1,
                new List<Ingredient>([
                    _testsIngredients[22]
                ])
            },
            {
                8,
                3,
                new List<Ingredient>([
                    _testsIngredients[21],
                    _testsIngredients[22]
                ])
            },
            {
                24,
                1,
                new List<Ingredient>()
            },
            {
                4,
                8,
                new List<Ingredient>()
            },
            {
                5,
                6,
                new List<Ingredient>()
            },
            {
                100,
                10,
                new List<Ingredient>()
            },

            // Negative/zero cases
            {
                0,
                5,
                new List<Ingredient>([
                    _testsIngredients[0],
                    _testsIngredients[1],
                    _testsIngredients[2],
                    _testsIngredients[3],
                    _testsIngredients[4]
                ])
            },
            {
                -1,
                10,
                new List<Ingredient>([
                    _testsIngredients[0],
                    _testsIngredients[1],
                    _testsIngredients[2],
                    _testsIngredients[3],
                    _testsIngredients[4],
                    _testsIngredients[5],
                    _testsIngredients[6],
                    _testsIngredients[7],
                    _testsIngredients[8],
                    _testsIngredients[9]
                ])
            },
            {
                1,
                0,
                new List<Ingredient>()
            },
            {
                1,
                -5,
                new List<Ingredient>()
            },
            {
                0,
                0,
                new List<Ingredient>()
            },
            {
                -1,
                -1,
                new List<Ingredient>()
            },
            {
                -5,
                -2,
                new List<Ingredient>()
            }
        };

    [Theory]
    [MemberData(nameof(PaginatedIngredients))]
    public void Paginate_ForIngredients_Return_ExpectedIngredients(
        int page,
        int pageSize,
        List<Ingredient> expected
    )
    {
        // Arrange
        IQueryable<Ingredient> queryable = _testsIngredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._paginationService.Paginate(queryable, page, pageSize);
        List<Ingredient> collection = queryResult.ToList();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}