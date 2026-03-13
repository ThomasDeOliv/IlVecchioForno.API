using IlVecchioForno.Domain.Ingredients;
using IlVecchioForno.Domain.PizzaIngredients;
using IlVecchioForno.Domain.Pizzas;
using IlVecchioForno.Domain.Pizzas.Exceptions;
using IlVecchioForno.Domain.QuantityTypes;

namespace IlVecchioForno.Domain.Tests;

public class PizzaTests
{
    private const string pizzaNameBase = "Vegetariana";
    private const string pizzaDescriptionBase = "Seasonal vegetables, mozzarella.";
    private const decimal pizzaPriceBase = 10m;

    private readonly List<Ingredient> _ingredients;
    private readonly List<decimal> _quantities;

    public PizzaTests()
    {
        List<QuantityType> quantityTypes = new List<QuantityType>
        {
            new QuantityType("Milligrams", "mg"),
            new QuantityType("Grams", "g"),
            new QuantityType("Kilograms", "kg"),
            new QuantityType("Milliliters", "mL"),
            new QuantityType("Centiliters", "cL"),
            new QuantityType("Liters", "L")
        };

        this._ingredients = new List<Ingredient>
        {
            new Ingredient("Flour (00)", quantityTypes[2]),
            new Ingredient("Water", quantityTypes[4]),
            new Ingredient("Fresh yeast", quantityTypes[2]),
            new Ingredient("Salt", quantityTypes[2]),
            new Ingredient("Extra virgin olive oil", quantityTypes[4]),
            new Ingredient("San Marzano tomato sauce", quantityTypes[2]),
            new Ingredient("Fresh basil leaves", quantityTypes[0]),
            new Ingredient("Garlic clove", quantityTypes[0]),
            new Ingredient("Oregano", quantityTypes[2]),
            new Ingredient("Mozzarella fior di latte", quantityTypes[2]),
            new Ingredient("Parmigiano Reggiano", quantityTypes[2]),
            new Ingredient("Pecorino Romano", quantityTypes[2]),
            new Ingredient("Prosciutto crudo", quantityTypes[2]),
            new Ingredient("Spianata calabra", quantityTypes[2]),
            new Ingredient("Pancetta", quantityTypes[2]),
            new Ingredient("'Nduja", quantityTypes[2]),
            new Ingredient("Black olives", quantityTypes[0]),
            new Ingredient("Mushrooms", quantityTypes[2]),
            new Ingredient("Artichoke hearts", quantityTypes[0]),
            new Ingredient("Capers", quantityTypes[2]),
            new Ingredient("Arugula", quantityTypes[2]),
            new Ingredient("Cherry tomatoes", quantityTypes[2]),
            new Ingredient("Buffalo mozzarella", quantityTypes[2])
        };

        this._quantities = new List<decimal>
        {
            0m,
            1m,
            2m,
            0.5m,
            0.008m,
            100m,
            250m
        };
    }

    public static TheoryData<string, string, decimal> ValidPizzasWithDescription =>
        new TheoryData<string, string, decimal>
        {
            {
                "Margherita", "Tomato sauce, mozzarella, basil.", 9.50m
            },
            {
                "Marinara", "Tomato sauce, garlic, oregano.", 8.00m
            },
            {
                "Pepperoni", "Tomato sauce, mozzarella, pepperoni.", 12.90m
            },
            {
                "Diavola", "Spicy salami, chili, mozzarella", 13.50m
            },
            {
                "Quattro Formaggi", "Mozzarella, gorgonzola, parmesan, emmental.", 14.50m
            },
            {
                "Prosciutto", "Ham, mozzarella, tomato sauce.", 12.00m
            },
            {
                "Funghi", "Mushrooms, mozzarella, tomato sauce.", 11.00m
            },
            {
                "Capricciosa", "Ham, mushrooms, artichokes, olives.", 13.90m
            },
            {
                "Vegetariana", "Seasonal vegetables, mozzarella.", 12.50m
            },
            {
                "Truffle", "Cream base, mushrooms, truffle oil.", 16.90m
            }
        };

    public static TheoryData<string, decimal> ValidPizzasWithoutDescription =>
        new TheoryData<string, decimal>
        {
            {
                "Gorgonzola", 12.00m
            },
            {
                "Special of the day", 10.00m
            }
        };

    public static TheoryData<decimal> ValidPrices =>
        new TheoryData<decimal>
        {
            0m,
            1m,
            2m,
            0.5m,
            0.008m,
            8m,
            10.50m,
            12m,
            15m,
            100m,
            250m,
            1000m,
            8000m
        };

    public static TheoryData<decimal> InvalidPrices =>
        new TheoryData<decimal>
        {
            -1m,
            -2m,
            -0.5m,
            -0.008m,
            -8m,
            -10.50m,
            -12m,
            -15m,
            -100m,
            -250m,
            -1000m,
            -8000m
        };

    private static Pizza CreatePizza(string name, string? description, decimal price)
    {
        return new Pizza(
            name,
            !string.IsNullOrEmpty(description)
                ? new PizzaDescription(description)
                : null,
            price
        );
    }

    [Theory]
    [MemberData(nameof(ValidPizzasWithDescription))]
    public void Pizza_CreateInstance_Succeeds_WhenProvidingValidValuesWithDescription(string name, string description,
        decimal price)
    {
        // Arrange & Act
        Pizza pizza = CreatePizza(name, description, price);
        // Assert
        Assert.Equal(name, pizza.Name);
        Assert.Equal(price, (decimal)pizza.Price);
        Assert.Null(pizza.ArchivedAt);
        Assert.Empty(pizza.PizzaIngredients);
        Assert.NotNull(pizza.Description);
        Assert.Equal(description, pizza.Description);
    }

    [Theory]
    [MemberData(nameof(ValidPizzasWithoutDescription))]
    public void Pizza_CreateInstance_Succeeds_WhenProvidingValidValuesWithoutDescription(string name, decimal price)
    {
        // Arrange & Act
        Pizza pizza = CreatePizza(name, null, price);
        // Assert
        Assert.Equal(name, pizza.Name);
        Assert.Equal(price, (decimal)pizza.Price);
        Assert.Null(pizza.ArchivedAt);
        Assert.Empty(pizza.PizzaIngredients);
        Assert.Null(pizza.Description);
    }

    [Theory]
    [InlineData("Gorgonzola")]
    [InlineData("Margherita")]
    [InlineData("Caprese")]
    [InlineData("Regina")]
    [InlineData("Tonno")]
    public void Pizza_UpdateName_Succeeds_WhenProvidingValidValue(string name)
    {
        // Arrange
        Pizza pizza = CreatePizza(pizzaNameBase, pizzaDescriptionBase, pizzaPriceBase);
        // Act
        pizza.UpdateName(name);
        // Assert
        Assert.Equal(name, pizza.Name);
        Assert.NotNull(pizza.Description);
        Assert.Equal(pizzaDescriptionBase, pizza.Description);
        Assert.Equal(pizzaPriceBase, (decimal)pizza.Price);
        Assert.Null(pizza.ArchivedAt);
        Assert.Empty(pizza.PizzaIngredients);
    }

    [Fact]
    public void Pizza_UpdateDescription_Succeeds_WhenProvidingValidValue()
    {
        // Arrange
        string description = "Seasonal vegetables, mozzarella and pecorino fioretto.";
        Pizza pizza = CreatePizza(pizzaNameBase, pizzaDescriptionBase, pizzaPriceBase);
        // Act
        pizza.UpdateDescription(description);
        // Assert
        Assert.Equal(pizzaNameBase, pizza.Name);
        Assert.NotNull(pizza.Description);
        Assert.Equal(description, pizza.Description);
        Assert.Equal(pizzaPriceBase, (decimal)pizza.Price);
        Assert.Null(pizza.ArchivedAt);
        Assert.Empty(pizza.PizzaIngredients);
    }

    [Theory]
    [MemberData(nameof(ValidPrices))]
    public void Pizza_UpdatePrice_Succeeds_WhenProvidingValidValue(decimal price)
    {
        // Arrange
        Pizza pizza = CreatePizza(pizzaNameBase, pizzaDescriptionBase, pizzaPriceBase);
        // Act
        pizza.UpdatePrice(price);
        // Assert
        Assert.Equal(pizzaNameBase, pizza.Name);
        Assert.NotNull(pizza.Description);
        Assert.Equal(pizzaDescriptionBase, pizza.Description);
        Assert.Equal(price, (decimal)pizza.Price);
        Assert.Null(pizza.ArchivedAt);
        Assert.Empty(pizza.PizzaIngredients);
    }

    [Fact]
    public void Pizza_UpdateArchived_SetsArchivedDate_WhenPizzaIsNotArchived()
    {
        // Arrange
        Pizza pizza = CreatePizza(pizzaNameBase, pizzaDescriptionBase, pizzaPriceBase);
        // Act
        pizza.UpdateArchived();
        // Assert
        Assert.Equal(pizzaNameBase, pizza.Name);
        Assert.NotNull(pizza.Description);
        Assert.Equal(pizzaDescriptionBase, pizza.Description);
        Assert.Equal(pizzaPriceBase, (decimal)pizza.Price);
        Assert.NotNull(pizza.ArchivedAt);
        Assert.Empty(pizza.PizzaIngredients);
    }

    [Fact]
    public void Pizza_UpdateArchived_ClearsArchivedDate_WhenPizzaIsArchived()
    {
        // Arrange
        Pizza pizza = CreatePizza(pizzaNameBase, pizzaDescriptionBase, pizzaPriceBase);
        pizza.UpdateArchived();
        DateTimeOffset? archivedValue = pizza.ArchivedAt;
        // Act
        pizza.UpdateArchived();
        // Assert
        Assert.Equal(pizzaNameBase, pizza.Name);
        Assert.NotNull(pizza.Description);
        Assert.Equal(pizzaDescriptionBase, pizza.Description);
        Assert.Equal(pizzaPriceBase, (decimal)pizza.Price);
        Assert.Null(pizza.ArchivedAt);
        Assert.Empty(pizza.PizzaIngredients);
        Assert.NotNull(archivedValue);
    }

    [Fact]
    public void Pizza_UpdateIngredients_Succeeds_WhenProvidingValidValues()
    {
        // Arrange
        Pizza pizza = CreatePizza(pizzaNameBase, pizzaDescriptionBase, pizzaPriceBase);
        int beforeInsertIngredientsCount = pizza.PizzaIngredients.Count;
        PizzaIngredientQuantity pizzaIngredientQuantity1 = new PizzaIngredientQuantity(this._quantities[0]);
        PizzaIngredientQuantity pizzaIngredientQuantity2 = new PizzaIngredientQuantity(this._quantities[1]);
        PizzaIngredientQuantity pizzaIngredientQuantity3 = new PizzaIngredientQuantity(this._quantities[0]);
        PizzaIngredientQuantity pizzaIngredientQuantity4 = new PizzaIngredientQuantity(this._quantities[2]);
        List<PizzaIngredient> newPizzaIngredients = new List<PizzaIngredient>
        {
            new PizzaIngredient(pizzaIngredientQuantity1, this._ingredients[0]),
            new PizzaIngredient(pizzaIngredientQuantity2, this._ingredients[1]),
            new PizzaIngredient(pizzaIngredientQuantity3, this._ingredients[2]),
            new PizzaIngredient(pizzaIngredientQuantity4, this._ingredients[3])
        };
        // Act
        pizza.UpdateIngredients(newPizzaIngredients);
        int afterInsertIngredientsCount = pizza.PizzaIngredients.Count;
        // Assert
        Assert.Equal(pizzaNameBase, pizza.Name);
        Assert.NotNull(pizza.Description);
        Assert.Equal(pizzaDescriptionBase, pizza.Description);
        Assert.Equal(pizzaPriceBase, (decimal)pizza.Price);
        Assert.Null(pizza.ArchivedAt);
        Assert.Equal(0, beforeInsertIngredientsCount);
        Assert.Equal(newPizzaIngredients.Count, afterInsertIngredientsCount);
        Assert.All(newPizzaIngredients, element => Assert.Contains(element, pizza.PizzaIngredients));
    }

    [Fact]
    public void Pizza_UpdateIngredients_ThrowsException_WhenProvidingDuplicateIngredients()
    {
        // Arrange
        Pizza pizza = CreatePizza(pizzaNameBase, pizzaDescriptionBase, pizzaPriceBase);
        PizzaIngredientQuantity pizzaIngredientQuantity1 = new PizzaIngredientQuantity(this._quantities[0]);
        PizzaIngredientQuantity pizzaIngredientQuantity2 = new PizzaIngredientQuantity(this._quantities[1]);
        PizzaIngredientQuantity pizzaIngredientQuantity3 = new PizzaIngredientQuantity(this._quantities[0]);
        PizzaIngredientQuantity pizzaIngredientQuantity4 = new PizzaIngredientQuantity(this._quantities[2]);
        List<PizzaIngredient> newPizzaIngredients = new List<PizzaIngredient>
        {
            new PizzaIngredient(pizzaIngredientQuantity1, this._ingredients[0]),
            new PizzaIngredient(pizzaIngredientQuantity2, this._ingredients[1]),
            new PizzaIngredient(pizzaIngredientQuantity3, this._ingredients[1]),
            new PizzaIngredient(pizzaIngredientQuantity4, this._ingredients[3])
        };
        // Act
        PizzaAggregateBaseException exception =
            Assert.Throws<PizzaAggregateBaseException>(() => pizza.UpdateIngredients(newPizzaIngredients));
        // Assert
        Assert.Equal(
            "Provided ingredients must be unique.",
            exception.Message
        );
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    [InlineData("                          ")]
    public void PizzaName_CreateInstance_ThrowsException_WhenProvidingInvalidValue(string? name)
    {
        // Arrange & Act
        PizzaNameException exception = Assert.Throws<PizzaNameException>(() => new PizzaName(name!));
        // Assert
        Assert.Equal(
            $"{nameof(PizzaName)} cannot be instantiated from null, empty or whitespace value.",
            exception.Message
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void PizzaName_CreateInstance_ThrowsException_WhenExceedingMaxLength(int length)
    {
        // Arrange
        string name = new string('a', PizzaInvariant.NameMaxLength + length);
        // Act
        PizzaNameException exception = Assert.Throws<PizzaNameException>(() => new PizzaName(name));
        // Assert
        Assert.Equal(
            $"{nameof(PizzaName)} exceeds maximum length of {PizzaInvariant.NameMaxLength} characters.",
            exception.Message
        );
    }

    [Fact]
    public void PizzaName_CanBeImplicitlyCastToString()
    {
        // Arrange
        const string rawValue = "A random name...";
        PizzaName valueObject = new PizzaName(rawValue);
        // Act
        string result = valueObject;
        // Assert
        Assert.IsType<string>(result);
        Assert.Equal(rawValue, result);
    }

    [Fact]
    public void PizzaName_CanBeImplicitlyCreatedFromValidString()
    {
        // Arrange
        const string rawValue = "A random name...";
        // Act
        PizzaName result = rawValue;
        // Assert
        Assert.IsType<PizzaName>(result);
        Assert.Equal(rawValue, result.Value);
    }

    [Fact]
    public void PizzaName_CannotBeImplicitlyCreatedFromNull()
    {
        // Arrange
        const string? rawValue = null;
        // Act & Assert
        PizzaNameException ex = Assert.Throws<PizzaNameException>(() =>
        {
            PizzaName _ = rawValue!;
        });
        Assert.Equal($"{nameof(PizzaName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void PizzaName_CannotBeImplicitlyCreatedFromEmptyString()
    {
        // Arrange
        const string rawValue = "";
        // Act & Assert
        PizzaNameException ex = Assert.Throws<PizzaNameException>(() =>
        {
            PizzaName _ = rawValue;
        });
        Assert.Equal($"{nameof(PizzaName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void PizzaName_CannotBeImplicitlyCreatedFromStringFilledWithWhiteSpaces()
    {
        // Arrange
        const string rawValue = "                  ";
        // Act & Assert
        PizzaNameException ex = Assert.Throws<PizzaNameException>(() =>
        {
            PizzaName _ = rawValue;
        });
        Assert.Equal($"{nameof(PizzaName)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void PizzaName_CannotBeImplicitlyCreatedFromStringExceedingMaxLength()
    {
        // Arrange
        string rawValue = new string('c', PizzaInvariant.NameMaxLength + 1);
        // Act & Assert
        PizzaNameException ex = Assert.Throws<PizzaNameException>(() =>
        {
            PizzaName _ = rawValue;
        });
        Assert.Equal($"{nameof(PizzaName)} exceeds maximum length of {PizzaInvariant.NameMaxLength} characters.", ex.Message);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("     ")]
    [InlineData("                          ")]
    public void PizzaDescription_CreateInstance_ThrowsException_WhenProvidingInvalidValue(string? description)
    {
        // Arrange & Act
        PizzaDescriptionException exception =
            Assert.Throws<PizzaDescriptionException>(() => new PizzaDescription(description!));
        // Assert
        Assert.Equal(
            $"{nameof(PizzaDescription)} cannot be instantiated from null, empty or whitespace value.",
            exception.Message
        );
    }

    [Theory]
    [InlineData(1)]
    [InlineData(10)]
    [InlineData(50)]
    [InlineData(100)]
    [InlineData(1000)]
    public void PizzaDescription_CreateInstance_ThrowsException_WhenExceedingMaxLength(int length)
    {
        // Arrange
        string description = new string('a', PizzaInvariant.DescriptionMaxLength + length);
        // Act
        PizzaDescriptionException exception =
            Assert.Throws<PizzaDescriptionException>(() => new PizzaDescription(description));
        // Assert
        Assert.Equal(
            $"{nameof(PizzaDescription)} exceeds maximum length of {PizzaInvariant.DescriptionMaxLength} characters.",
            exception.Message
        );
    }

    [Fact]
    public void PizzaDescription_CanBeImplicitlyCastToString()
    {
        // Arrange
        const string rawValue = "A random description...";
        PizzaDescription valueObject = new PizzaDescription(rawValue);
        // Act
        string result = valueObject;
        // Assert
        Assert.IsType<string>(result);
        Assert.Equal(rawValue, result);
    }

    [Fact]
    public void PizzaDescription_CanBeImplicitlyCreatedFromValidString()
    {
        // Arrange
        const string rawValue = "A random description...";
        // Act
        PizzaDescription result = rawValue;
        // Assert
        Assert.IsType<PizzaDescription>(result);
        Assert.Equal(rawValue, result.Value);
    }

    [Fact]
    public void PizzaDescription_CannotBeImplicitlyCreatedFromNull()
    {
        // Arrange
        const string? rawValue = null;
        // Act & Assert
        PizzaDescriptionException ex = Assert.Throws<PizzaDescriptionException>(() =>
        {
            PizzaDescription _ = rawValue!;
        });
        Assert.Equal($"{nameof(PizzaDescription)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void PizzaDescription_CannotBeImplicitlyCreatedFromEmptyString()
    {
        // Arrange
        const string rawValue = "";
        // Act & Assert
        PizzaDescriptionException ex = Assert.Throws<PizzaDescriptionException>(() =>
        {
            PizzaDescription _ = rawValue;
        });
        Assert.Equal($"{nameof(PizzaDescription)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void PizzaDescription_CannotBeImplicitlyCreatedFromStringFilledWithWhiteSpaces()
    {
        // Arrange
        const string rawValue = "                  ";
        // Act & Assert
        PizzaDescriptionException ex = Assert.Throws<PizzaDescriptionException>(() =>
        {
            PizzaDescription _ = rawValue;
        });
        Assert.Equal($"{nameof(PizzaDescription)} cannot be instantiated from null, empty or whitespace value.", ex.Message);
    }

    [Fact]
    public void PizzaDescription_CannotBeImplicitlyCreatedFromStringExceedingMaxLength()
    {
        // Arrange
        string rawValue = new string('c', PizzaInvariant.DescriptionMaxLength + 1);
        // Act & Assert
        PizzaDescriptionException ex = Assert.Throws<PizzaDescriptionException>(() =>
        {
            PizzaDescription _ = rawValue;
        });
        Assert.Equal($"{nameof(PizzaDescription)} exceeds maximum length of {PizzaInvariant.DescriptionMaxLength} characters.", ex.Message);
    }

    [Theory]
    [MemberData(nameof(InvalidPrices))]
    public void PizzaPrice_CreateInstance_ThrowsException_WhenProvidingNegativeValue(decimal price)
    {
        // Arrange & Act
        PizzaPriceException exception = Assert.Throws<PizzaPriceException>(() => new PizzaPrice(price));
        // Assert
        Assert.Equal(
            $"{nameof(PizzaPrice)} is smaller than the minimum required value of {PizzaInvariant.MinPrice}.",
            exception.Message
        );
    }

    [Fact]
    public void PizzaPrice_CanBeImplicitlyCastToDecimal()
    {
        // Arrange
        const decimal rawValue = 15.0m;
        PizzaPrice valueObject = new PizzaPrice(rawValue);
        // Act
        decimal result = valueObject;
        // Assert
        Assert.IsType<decimal>(result);
        Assert.Equal(rawValue, result);
    }

    [Fact]
    public void PizzaPrice_CanBeImplicitlyCreatedFromDecimal()
    {
        // Arrange
        const decimal rawValue = 15.0m;
        // Act
        PizzaPrice result = rawValue;
        // Assert
        Assert.IsType<PizzaPrice>(result);
        Assert.Equal(rawValue, result.Value);
    }

    [Fact]
    public void PizzaPrice_CannotBeImplicitlyCreatedFromDecimalBelowMinPrice()
    {
        // Arrange
        const decimal rawValue = -15.0m;
        // Act & Assert
        PizzaPriceException ex = Assert.Throws<PizzaPriceException>(() =>
        {
            PizzaPrice _ = rawValue;
        });
        Assert.Equal($"{nameof(PizzaPrice)} is smaller than the minimum required value of {PizzaInvariant.MinPrice}.", ex.Message);
    }
}