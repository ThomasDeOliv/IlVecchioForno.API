using IlVecchioForno.Domain.PizzaIngredients.Exceptions;

namespace IlVecchioForno.Domain.PizzaIngredients;

public sealed class PizzaIngredientQuantity
{
    public PizzaIngredientQuantity(decimal value)
    {
        if (value < PizzaIngredientInvariant.MinQuantity)
            throw new PizzaIngredientQuantityException(
                $"{nameof(PizzaIngredientQuantity)} is smaller than the minimum required value of {PizzaIngredientInvariant.MinQuantity}.");

        this.Value = value;
    }

    public decimal Value { get; }

    public static implicit operator decimal(PizzaIngredientQuantity valueObject)
    {
        return valueObject.Value;
    }

    public static implicit operator PizzaIngredientQuantity(decimal value)
    {
        return new PizzaIngredientQuantity(value);
    }
}