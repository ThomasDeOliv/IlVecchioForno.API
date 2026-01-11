using IlVecchioForno.Domain.PizzaIngredients.Exceptions;

namespace IlVecchioForno.Domain.PizzaIngredients;

public sealed class PizzaIngredientQuantity
{
    private readonly decimal _value;
    
    public PizzaIngredientQuantity(decimal value)
    {
        if (value < PizzaIngredientInvariant.MinQuantity)
        {
            throw new PizzaIngredientQuantityException($"Quantity is smaller than the minimum required value of {PizzaIngredientInvariant.MinQuantity}.");
        }
        
        this._value = value;
    }
    
    public static implicit operator decimal(PizzaIngredientQuantity valueObject) =>  valueObject._value;
}