using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;

namespace IlVecchioForno.Infrastructure.Tests.Utilities.Data;

public static class DbMockedTestsData
{
    public static readonly List<QuantityType> TestsQuantityTypes;
    public static readonly List<Ingredient> TestsIngredients;
    public static readonly List<Pizza> TestsPizzas;

    static DbMockedTestsData()
    {
        TestsQuantityTypes = new List<QuantityType>
        {
            new QuantityType(new QuantityTypeName("Milligrams"), new QuantityTypeUnit("mg")),
            new QuantityType(new QuantityTypeName("Grams"), new QuantityTypeUnit("g")),
            new QuantityType(new QuantityTypeName("Kilograms"), new QuantityTypeUnit("kg")),
            new QuantityType(new QuantityTypeName("Milliliters"), new QuantityTypeUnit("mL")),
            new QuantityType(new QuantityTypeName("Centiliters"), new QuantityTypeUnit("cL")),
            new QuantityType(new QuantityTypeName("Liters"), new QuantityTypeUnit("L"))
        };

        TestsIngredients = new List<Ingredient>
        {
            new Ingredient(new IngredientName("Flour (00)"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Water"), TestsQuantityTypes[3]),
            new Ingredient(new IngredientName("Fresh yeast"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Salt"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Extra virgin olive oil"), TestsQuantityTypes[3]),
            new Ingredient(new IngredientName("San Marzano tomato sauce"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Fresh basil leaves"), null),
            new Ingredient(new IngredientName("Garlic clove"), null),
            new Ingredient(new IngredientName("Oregano"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Mozzarella fior di latte"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Parmigiano Reggiano"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Pecorino Romano"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Prosciutto crudo"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Spianata calabra"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Pancetta"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("'Nduja"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Black olives"), null),
            new Ingredient(new IngredientName("Mushrooms"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Artichoke hearts"), null),
            new Ingredient(new IngredientName("Capers"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Arugula"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Cherry tomatoes"), TestsQuantityTypes[1]),
            new Ingredient(new IngredientName("Buffalo mozzarella"), TestsQuantityTypes[1])
        };

        TestsPizzas = new List<Pizza>
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
                new PizzaPrice(11.00m)
            ),
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

        TestsPizzas[10].UpdateArchived();
        TestsPizzas[11].UpdateArchived();
        TestsPizzas[14].UpdateArchived();
    }
}