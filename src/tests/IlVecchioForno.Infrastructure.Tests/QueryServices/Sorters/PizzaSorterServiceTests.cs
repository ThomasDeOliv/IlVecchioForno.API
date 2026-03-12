using IlVecchioForno.Application.Common.Queries.Sorters;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Infrastructure.Persistence.QueryServices.Sorters;

namespace IlVecchioForno.Infrastructure.Tests.QueryServices.Sorters;

public sealed class PizzaSorterServiceTests
{
    private static readonly List<Pizza> _testsPizzas;

    private readonly ISorterService<Pizza, PizzasSorter> _sorterService;

    static PizzaSorterServiceTests()
    {
        _testsPizzas = new List<Pizza>
        {
            new Pizza(
                new PizzaName("Margherita"),
                new PizzaDescription("The classic Neapolitan pizza"),
                new PizzaPrice(8.00m),
                1
            ),
            new Pizza(
                new PizzaName("Marinara"),
                null,
                new PizzaPrice(7.00m),
                2
            ),
            new Pizza(
                new PizzaName("Quattro Formaggi"),
                new PizzaDescription("Four cheese blend"),
                new PizzaPrice(11.50m),
                3
            ),
            new Pizza(
                new PizzaName("Diavola"),
                new PizzaDescription("Spicy salami and chili flakes"),
                new PizzaPrice(10.00m),
                4
            ),
            new Pizza(
                new PizzaName("Capricciosa"),
                new PizzaDescription("Artichokes, mushrooms, ham and olives"),
                new PizzaPrice(12.00m),
                5
            ),
            new Pizza(
                new PizzaName("Prosciutto e Funghi"),
                null,
                new PizzaPrice(11.00m),
                6
            ),
            new Pizza(
                new PizzaName("Quattro Stagioni"),
                new PizzaDescription("Four seasons, four toppings"),
                new PizzaPrice(12.50m),
                7
            ),
            new Pizza(
                new PizzaName("Bufala"),
                new PizzaDescription("Buffalo mozzarella and cherry tomatoes"),
                new PizzaPrice(13.00m),
                8
            ),
            new Pizza(
                new PizzaName("Calzone"),
                null,
                new PizzaPrice(10.50m),
                9
            ),
            new Pizza(
                new PizzaName("Napoli"),
                new PizzaDescription("Anchovies, capers and olives"),
                new PizzaPrice(9.50m),
                10
            ),
            new Pizza(
                new PizzaName("Pugliese"),
                null,
                new PizzaPrice(9.00m),
                11
            ),
            new Pizza(
                new PizzaName("Ortolana"),
                new PizzaDescription("Grilled seasonal vegetables"),
                new PizzaPrice(10.00m),
                12
            ),
            new Pizza(
                new PizzaName("Tonno e Cipolla"),
                null,
                new PizzaPrice(10.50m),
                13
            ),
            new Pizza(
                new PizzaName("Parmigiana"),
                new PizzaDescription("Eggplant, tomato sauce and parmigiano"),
                new PizzaPrice(11.00m),
                14
            ),
            new Pizza(
                new PizzaName("Tartufo"),
                new PizzaDescription("Black truffle cream and mushrooms"),
                new PizzaPrice(16.00m),
                15
            )
        };

        _testsPizzas[10].UpdateArchived();
        _testsPizzas[11].UpdateArchived();
        _testsPizzas[14].UpdateArchived();
    }

    public PizzaSorterServiceTests()
    {
        this._sorterService = new PizzaSorterService();
    }

    public static TheoryData<bool, PizzasSorter, List<Pizza>> SortedPizzas =>
        new TheoryData<bool, PizzasSorter, List<Pizza>>
        {
            // Default
            {
                false,
                PizzasSorter.Id,
                _testsPizzas
            },
            {
                true,
                PizzasSorter.Id,
                _testsPizzas.AsEnumerable().Reverse().ToList()
            },
            // Name
            {
                false,
                PizzasSorter.Name,
                new List<Pizza>
                {
                    _testsPizzas[7],
                    _testsPizzas[8],
                    _testsPizzas[4],
                    _testsPizzas[3],
                    _testsPizzas[0],
                    _testsPizzas[1],
                    _testsPizzas[9],
                    _testsPizzas[11],
                    _testsPizzas[13],
                    _testsPizzas[5],
                    _testsPizzas[10],
                    _testsPizzas[2],
                    _testsPizzas[6],
                    _testsPizzas[14],
                    _testsPizzas[12]
                }
            },
            {
                true,
                PizzasSorter.Name,
                new List<Pizza>
                {
                    _testsPizzas[12],
                    _testsPizzas[14],
                    _testsPizzas[6],
                    _testsPizzas[2],
                    _testsPizzas[10],
                    _testsPizzas[5],
                    _testsPizzas[13],
                    _testsPizzas[11],
                    _testsPizzas[9],
                    _testsPizzas[1],
                    _testsPizzas[0],
                    _testsPizzas[3],
                    _testsPizzas[4],
                    _testsPizzas[8],
                    _testsPizzas[7]
                }
            },
            // Price
            {
                false,
                PizzasSorter.Price,
                new List<Pizza>
                {
                    _testsPizzas[1],
                    _testsPizzas[0],
                    _testsPizzas[10],
                    _testsPizzas[9],
                    _testsPizzas[3],
                    _testsPizzas[11],
                    _testsPizzas[8],
                    _testsPizzas[12],
                    _testsPizzas[5],
                    _testsPizzas[13],
                    _testsPizzas[2],
                    _testsPizzas[4],
                    _testsPizzas[6],
                    _testsPizzas[7],
                    _testsPizzas[14]
                }
            },
            {
                true,
                PizzasSorter.Price,
                new List<Pizza>
                {
                    _testsPizzas[14],
                    _testsPizzas[7],
                    _testsPizzas[6],
                    _testsPizzas[4],
                    _testsPizzas[2],
                    _testsPizzas[5],
                    _testsPizzas[13],
                    _testsPizzas[8],
                    _testsPizzas[12],
                    _testsPizzas[3],
                    _testsPizzas[11],
                    _testsPizzas[9],
                    _testsPizzas[10],
                    _testsPizzas[0],
                    _testsPizzas[1]
                }
            },
            // Archived
            {
                false,
                PizzasSorter.Archived,
                new List<Pizza>
                {
                    _testsPizzas[10],
                    _testsPizzas[11],
                    _testsPizzas[14],
                    _testsPizzas[0],
                    _testsPizzas[1],
                    _testsPizzas[2],
                    _testsPizzas[3],
                    _testsPizzas[4],
                    _testsPizzas[5],
                    _testsPizzas[6],
                    _testsPizzas[7],
                    _testsPizzas[8],
                    _testsPizzas[9],
                    _testsPizzas[12],
                    _testsPizzas[13]
                }
            },
            {
                true,
                PizzasSorter.Archived,
                new List<Pizza>
                {
                    _testsPizzas[14],
                    _testsPizzas[11],
                    _testsPizzas[10],
                    _testsPizzas[0],
                    _testsPizzas[1],
                    _testsPizzas[2],
                    _testsPizzas[3],
                    _testsPizzas[4],
                    _testsPizzas[5],
                    _testsPizzas[6],
                    _testsPizzas[7],
                    _testsPizzas[8],
                    _testsPizzas[9],
                    _testsPizzas[12],
                    _testsPizzas[13]
                }
            }
        };

    [Theory]
    [MemberData(nameof(SortedPizzas))]
    public void Sorter_ForPizzas_Return_ExpectedSortedPizzas(
        bool descending,
        PizzasSorter sorter,
        List<Pizza> expected
    )
    {
        // Arrange
        IQueryable<Pizza> queryable = _testsPizzas.AsQueryable();
        // Act
        IQueryable<Pizza> queryResult = this._sorterService.OrderBy(queryable, sorter, descending);
        List<Pizza> collection = queryResult.ToList();
        // Assert
        Assert.Equal(expected.Count, collection.Count);
        Assert.Equivalent(expected, collection);
    }
}