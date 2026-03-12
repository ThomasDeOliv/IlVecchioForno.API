using IlVecchioForno.Domain.Common;
using IlVecchioForno.Domain.Ingredients;

namespace IlVecchioForno.Domain.PizzaIngredients;

public sealed class PizzaIngredient : EntityBase
{
    // EF
    private PizzaIngredient() : base()
    {
        this.Quantity = null!;
        this.Ingredient = null!;
    }

    public PizzaIngredient(PizzaIngredientQuantity quantity, Ingredient ingredient) : this()
    {
        this.Quantity = quantity;
        this.Ingredient = ingredient;
    }

    public PizzaIngredientQuantity Quantity { get; private set; }
    public Ingredient Ingredient { get; private set; }
}