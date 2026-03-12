using IlVecchioForno.Domain.Pizzas.Exceptions;

namespace IlVecchioForno.Domain.Pizzas;

public sealed class PizzaPrice
{
    public PizzaPrice(decimal value)
    {
        if (value < PizzaInvariant.MinPrice)
            throw new PizzaPriceException(
                $"{nameof(PizzaPrice)} is smaller than the minimum required value of {PizzaInvariant.MinPrice}."
            );

        this.Value = value;
    }

    public decimal Value { get; }

    public static implicit operator decimal(PizzaPrice valueObject)
    {
        return valueObject.Value;
    }
}