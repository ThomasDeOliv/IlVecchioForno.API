using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.QuantityTypes;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class IngredientSorterServiceTests
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

    private static readonly List<Ingredient> _testsIngredients = new List<Ingredient>
    {
        new Ingredient(new IngredientName("Flour (00)"), _testsQuantityTypes[1], 1),
        new Ingredient(new IngredientName("Water"), _testsQuantityTypes[3], 2),
        new Ingredient(new IngredientName("Fresh yeast"), _testsQuantityTypes[1], 3),
        new Ingredient(new IngredientName("Salt"), _testsQuantityTypes[1], 4),
        new Ingredient(new IngredientName("Extra virgin olive oil"), _testsQuantityTypes[3], 5),
        new Ingredient(new IngredientName("San Marzano tomato sauce"), _testsQuantityTypes[1], 6),
        new Ingredient(new IngredientName("Fresh basil leaves"), null, 7),
        new Ingredient(new IngredientName("Garlic clove"), null, 8),
        new Ingredient(new IngredientName("Oregano"), _testsQuantityTypes[1], 9),
        new Ingredient(new IngredientName("Mozzarella fior di latte"), _testsQuantityTypes[1], 10),
        new Ingredient(new IngredientName("Parmigiano Reggiano"), _testsQuantityTypes[1], 11),
        new Ingredient(new IngredientName("Pecorino Romano"), _testsQuantityTypes[1], 12),
        new Ingredient(new IngredientName("Prosciutto crudo"), _testsQuantityTypes[1], 13),
        new Ingredient(new IngredientName("Spianata calabra"), _testsQuantityTypes[1], 14),
        new Ingredient(new IngredientName("Pancetta"), _testsQuantityTypes[1], 15),
        new Ingredient(new IngredientName("'Nduja"), _testsQuantityTypes[1], 16),
        new Ingredient(new IngredientName("Black olives"), null, 17),
        new Ingredient(new IngredientName("Mushrooms"), _testsQuantityTypes[1], 18),
        new Ingredient(new IngredientName("Artichoke hearts"), null, 19),
        new Ingredient(new IngredientName("Capers"), _testsQuantityTypes[1], 20),
        new Ingredient(new IngredientName("Arugula"), _testsQuantityTypes[1], 21),
        new Ingredient(new IngredientName("Cherry tomatoes"), _testsQuantityTypes[1], 22),
        new Ingredient(new IngredientName("Buffalo mozzarella"), _testsQuantityTypes[1], 23)
    };

    private readonly ISorterService<Ingredient, IngredientsSorter> _sorterService;

    public IngredientSorterServiceTests()
    {
        this._sorterService = new IngredientSorterService();
    }

    public static TheoryData<bool, IngredientsSorter, List<Ingredient>> SortedIngredients =>
        new TheoryData<bool, IngredientsSorter, List<Ingredient>>
        {
            // Default
            {
                false,
                IngredientsSorter.Id,
                _testsIngredients
            },
            {
                true,
                IngredientsSorter.Id,
                _testsIngredients.AsEnumerable().Reverse().ToList()
            },
            // Name
            {
                false,
                IngredientsSorter.Name,
                new List<Ingredient>
                {
                    _testsIngredients[15],
                    _testsIngredients[18],
                    _testsIngredients[20],
                    _testsIngredients[16],
                    _testsIngredients[22],
                    _testsIngredients[19],
                    _testsIngredients[21],
                    _testsIngredients[4],
                    _testsIngredients[0],
                    _testsIngredients[6],
                    _testsIngredients[2],
                    _testsIngredients[7],
                    _testsIngredients[9],
                    _testsIngredients[17],
                    _testsIngredients[8],
                    _testsIngredients[14],
                    _testsIngredients[10],
                    _testsIngredients[11],
                    _testsIngredients[12],
                    _testsIngredients[3],
                    _testsIngredients[5],
                    _testsIngredients[13],
                    _testsIngredients[1]
                }
            },
            {
                true,
                IngredientsSorter.Name,
                new List<Ingredient>
                {
                    _testsIngredients[1],
                    _testsIngredients[13],
                    _testsIngredients[5],
                    _testsIngredients[3],
                    _testsIngredients[12],
                    _testsIngredients[11],
                    _testsIngredients[10],
                    _testsIngredients[14],
                    _testsIngredients[8],
                    _testsIngredients[17],
                    _testsIngredients[9],
                    _testsIngredients[7],
                    _testsIngredients[2],
                    _testsIngredients[6],
                    _testsIngredients[0],
                    _testsIngredients[4],
                    _testsIngredients[21],
                    _testsIngredients[19],
                    _testsIngredients[22],
                    _testsIngredients[16],
                    _testsIngredients[20],
                    _testsIngredients[18],
                    _testsIngredients[15]
                }
            }
        };

    [Theory]
    [MemberData(nameof(SortedIngredients))]
    public void Sorter_ForIngredients_Return_ExpectedSortedIngredients(
        bool descending,
        IngredientsSorter sorter,
        List<Ingredient> expected
    )
    {
        // Arrange
        IQueryable<Ingredient> queryable = _testsIngredients.AsQueryable();
        // Act
        IQueryable<Ingredient> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<Ingredient> collection = queryResult.ToList();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}