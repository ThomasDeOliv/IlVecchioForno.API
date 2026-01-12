using IlVecchioForno.Domain.Pizzas.Exceptions;

namespace IlVecchioForno.Domain.Pizzas;

public sealed class PizzaName
{
    public PizzaName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new PizzaNameException(
                $"{nameof(PizzaName)} cannot be instantiated from null, empty or whitespace value."
            );

        if (PizzaInvariant.NameMaxLength < value.Length)
            throw new PizzaNameException(
                $"{nameof(PizzaName)} exceeds maximum length of {PizzaInvariant.NameMaxLength} characters."
            );

        this.Value = value;
    }

    public string Value { get; }

    public static implicit operator string(PizzaName valueObject)
    {
        return valueObject.Value;
    }
}