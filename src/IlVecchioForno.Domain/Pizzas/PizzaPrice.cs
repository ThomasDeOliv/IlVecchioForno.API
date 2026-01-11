using IlVecchioForno.Domain.Pizzas.Exceptions;

namespace IlVecchioForno.Domain.Pizzas;

public sealed class PizzaPrice
{
    private readonly decimal _value;
    
    public PizzaPrice(decimal value)
    {
        if (value < PizzaInvariant.MinPrice)
        {
            throw new PizzaPriceException($"Provided price is smaller than the minimum required value of {PizzaInvariant.MinPrice}.");
        }
        
        this._value = value;
    }
    
    public static implicit operator decimal(PizzaPrice valueObject) =>  valueObject._value;
}