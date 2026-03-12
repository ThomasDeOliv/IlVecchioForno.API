#pragma warning disable xUnit1045

using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Paginations;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Paginations;

public sealed class PizzaPaginationServiceTests
{
    private static readonly List<Pizza> _testsPizzas = new List<Pizza>
    {
        new Pizza(
            new PizzaName("Margherita"),
            new PizzaDescription("The classic Neapolitan pizza"),
            new PizzaPrice(8.00m)
        ),
        new Pizza(
            new PizzaName("Marinara"),
            null,
            new PizzaPrice(7.00m)
        ),
        new Pizza(
            new PizzaName("Quattro Formaggi"),
            new PizzaDescription("Four cheese blend"),
            new PizzaPrice(11.50m)
        ),
        new Pizza(
            new PizzaName("Diavola"),
            new PizzaDescription("Spicy salami and chili flakes"),
            new PizzaPrice(10.00m)
        ),
        new Pizza(
            new PizzaName("Capricciosa"),
            new PizzaDescription("Artichokes, mushrooms, ham and olives"),
            new PizzaPrice(12.00m)
        ),
        new Pizza(
            new PizzaName("Prosciutto e Funghi"),
            null,
            new PizzaPrice(11.00m)),
        new Pizza(
            new PizzaName("Quattro Stagioni"),
            new PizzaDescription("Four seasons, four toppings"),
            new PizzaPrice(12.50m)
        ),
        new Pizza(
            new PizzaName("Bufala"),
            new PizzaDescription("Buffalo mozzarella and cherry tomatoes"),
            new PizzaPrice(13.00m)
        ),
        new Pizza(
            new PizzaName("Calzone"),
            null,
            new PizzaPrice(10.50m)
        ),
        new Pizza(
            new PizzaName("Napoli"),
            new PizzaDescription("Anchovies, capers and olives"),
            new PizzaPrice(9.50m)
        ),
        new Pizza(
            new PizzaName("Pugliese"),
            null,
            new PizzaPrice(9.00m)
        ),
        new Pizza(
            new PizzaName("Ortolana"),
            new PizzaDescription("Grilled seasonal vegetables"),
            new PizzaPrice(10.00m)
        ),
        new Pizza(
            new PizzaName("Tonno e Cipolla"),
            null,
            new PizzaPrice(10.50m)
        ),
        new Pizza(
            new PizzaName("Parmigiana"),
            new PizzaDescription("Eggplant, tomato sauce and parmigiano"),
            new PizzaPrice(11.00m)
        ),
        new Pizza(
            new PizzaName("Tartufo"),
            new PizzaDescription("Black truffle cream and mushrooms"),
            new PizzaPrice(16.00m)
        )
    };

    private readonly IPaginationService<Pizza> _paginationService;

    public PizzaPaginationServiceTests()
    {
        this._paginationService = new PaginationService<Pizza>();
    }

    public static TheoryData<int, int, List<Pizza>> PaginatedPizzas =>
        new TheoryData<int, int, List<Pizza>>
        {
            // Positive cases
            {
                1,
                1,
                new List<Pizza>([
                    _testsPizzas[0]
                ])
            },
            {
                1,
                4,
                new List<Pizza>([
                    _testsPizzas[0],
                    _testsPizzas[1],
                    _testsPizzas[2],
                    _testsPizzas[3]
                ])
            },
            {
                1,
                15,
                new List<Pizza>(_testsPizzas)
            },
            {
                1,
                100,
                new List<Pizza>(_testsPizzas)
            },
            {
                2,
                4,
                new List<Pizza>([
                    _testsPizzas[4],
                    _testsPizzas[5],
                    _testsPizzas[6],
                    _testsPizzas[7]
                ])
            },
            {
                2,
                5,
                new List<Pizza>([
                    _testsPizzas[5],
                    _testsPizzas[6],
                    _testsPizzas[7],
                    _testsPizzas[8],
                    _testsPizzas[9]
                ])
            },
            {
                3,
                4,
                new List<Pizza>([
                    _testsPizzas[8],
                    _testsPizzas[9],
                    _testsPizzas[10],
                    _testsPizzas[11]
                ])
            },
            {
                2,
                7,
                new List<Pizza>([
                    _testsPizzas[7],
                    _testsPizzas[8],
                    _testsPizzas[9],
                    _testsPizzas[10],
                    _testsPizzas[11],
                    _testsPizzas[12],
                    _testsPizzas[13]
                ])
            },
            {
                4,
                4,
                new List<Pizza>([
                    _testsPizzas[12],
                    _testsPizzas[13],
                    _testsPizzas[14]
                ])
            },
            {
                3,
                5,
                new List<Pizza>([
                    _testsPizzas[10],
                    _testsPizzas[11],
                    _testsPizzas[12],
                    _testsPizzas[13],
                    _testsPizzas[14]
                ])
            },
            {
                2,
                8,
                new List<Pizza>([
                    _testsPizzas[8],
                    _testsPizzas[9],
                    _testsPizzas[10],
                    _testsPizzas[11],
                    _testsPizzas[12],
                    _testsPizzas[13],
                    _testsPizzas[14]
                ])
            },
            {
                3,
                7,
                new List<Pizza>([
                    _testsPizzas[14]
                ])
            },
            {
                15,
                1,
                new List<Pizza>([
                    _testsPizzas[14]
                ])
            },
            {
                8,
                2,
                new List<Pizza>([
                    _testsPizzas[14]
                ])
            },
            {
                16,
                1,
                new List<Pizza>()
            },
            {
                5,
                4,
                new List<Pizza>()
            },
            {
                3,
                8,
                new List<Pizza>()
            },
            {
                100,
                10,
                new List<Pizza>()
            },

            // Negative/zero cases
            {
                0,
                4,
                new List<Pizza>([
                    _testsPizzas[0],
                    _testsPizzas[1],
                    _testsPizzas[2],
                    _testsPizzas[3]
                ])
            },
            {
                -2,
                5,
                new List<Pizza>([
                    _testsPizzas[0],
                    _testsPizzas[1],
                    _testsPizzas[2],
                    _testsPizzas[3],
                    _testsPizzas[4]
                ])
            },
            {
                1,
                0,
                new List<Pizza>()
            },
            {
                1,
                -3,
                new List<Pizza>()
            },
            {
                0,
                0,
                new List<Pizza>()
            },
            {
                -1,
                -1,
                new List<Pizza>()
            },
            {
                -1,
                -3,
                new List<Pizza>()
            }
        };

    [Theory]
    [MemberData(nameof(PaginatedPizzas))]
    public void Paginate_ForPizzas_Return_ExpectedPizzas(
        int page,
        int pageSize,
        List<Pizza> expected
    )
    {
        // Arrange
        IQueryable<Pizza> queryable = _testsPizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._paginationService.Paginate(queryable, page, pageSize);
        List<Pizza> collection = queryResult.ToList();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}