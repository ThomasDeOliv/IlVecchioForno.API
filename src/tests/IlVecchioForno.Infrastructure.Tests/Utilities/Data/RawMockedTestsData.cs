using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.QuantityTypes;

namespace IlVecchioForno.Infrastructure.Tests.Utilities.Data;

public static class RawMockedTestsData
{
    public static List<QuantityType> CreateQuantityTypes()
    {
        return
        [
            new QuantityType("Milligrams", "mg"),
            new QuantityType("Grams", "g"),
            new QuantityType("Kilograms", "kg"),
            new QuantityType("Milliliters", "mL"),
            new QuantityType("Centiliters", "cL"),
            new QuantityType("Liters", "L")
        ];
    }

    public static List<Ingredient> CreateIngredients(List<QuantityType> quantityTypes)
    {
        return
        [
            new Ingredient("Flour (00)", quantityTypes[1]),
            new Ingredient("Water", quantityTypes[3]),
            new Ingredient("Fresh yeast", quantityTypes[1]),
            new Ingredient("Salt", quantityTypes[1]),
            new Ingredient("Extra virgin olive oil", quantityTypes[3]),
            new Ingredient("San Marzano tomato sauce", quantityTypes[1]),
            new Ingredient("Fresh basil leaves", null),
            new Ingredient("Garlic clove", null),
            new Ingredient("Oregano", quantityTypes[1]),
            new Ingredient("Mozzarella fior di latte", quantityTypes[1]),
            new Ingredient("Parmigiano Reggiano", quantityTypes[1]),
            new Ingredient("Pecorino Romano", quantityTypes[1]),
            new Ingredient("Prosciutto crudo", quantityTypes[1]),
            new Ingredient("Spianata calabra", quantityTypes[1]),
            new Ingredient("Pancetta", quantityTypes[1]),
            new Ingredient("'Nduja", quantityTypes[1]),
            new Ingredient("Black olives", null),
            new Ingredient("Mushrooms", quantityTypes[1]),
            new Ingredient("Artichoke hearts", null),
            new Ingredient("Capers", quantityTypes[1]),
            new Ingredient("Arugula", quantityTypes[1]),
            new Ingredient("Cherry tomatoes", quantityTypes[1]),
            new Ingredient("Buffalo mozzarella", quantityTypes[1])
        ];
    }

    public static List<Pizza> CreatePizzas()
    {
        List<Pizza> pizzas =
        [
            new Pizza("Margherita", new PizzaDescription("The classic Neapolitan pizza"), 8.00m),
            new Pizza("Marinara", null, 7.00m),
            new Pizza("Quattro Formaggi", new PizzaDescription("Four cheese blend"), 11.50m),
            new Pizza("Diavola", new PizzaDescription("Spicy salami and chili flakes"), 10.00m),
            new Pizza("Capricciosa", new PizzaDescription("Artichokes, mushrooms, ham and olives"), 12.00m),
            new Pizza("Prosciutto e Funghi", null, 11.00m),
            new Pizza("Quattro Stagioni", new PizzaDescription("Four seasons, four toppings"), 12.50m),
            new Pizza("Bufala", new PizzaDescription("Buffalo mozzarella and cherry tomatoes"), 13.00m),
            new Pizza("Calzone", null, 10.50m),
            new Pizza("Napoli", new PizzaDescription("Anchovies, capers and olives"), 9.50m),
            new Pizza("Pugliese", null, 9.00m),
            new Pizza("Ortolana", new PizzaDescription("Grilled seasonal vegetables"), 10.00m),
            new Pizza("Tonno e Cipolla", null, 10.50m),
            new Pizza("Parmigiana", new PizzaDescription("Eggplant, tomato sauce and parmigiano"), 11.00m),
            new Pizza("Tartufo", new PizzaDescription("Black truffle cream and mushrooms"), 16.00m)
        ];

        pizzas[10].UpdateArchived();
        pizzas[11].UpdateArchived();
        pizzas[14].UpdateArchived();

        return pizzas;
    }
}