using IlVecchioForno.Domain.Pizzas.Exceptions;

namespace IlVecchioForno.Domain.Pizzas;

public sealed class PizzaDescription
{
    public PizzaDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new PizzaDescriptionException(
                $"{nameof(PizzaDescription)} cannot be instantiated from null, empty or whitespace value."
            );

        if (PizzaInvariant.DescriptionMaxLength < value.Length)
            throw new PizzaDescriptionException(
                $"{nameof(PizzaDescription)} exceeds maximum length of {PizzaInvariant.DescriptionMaxLength} characters."
            );

        this.Value = value;
    }

    public string Value { get; }

    public static implicit operator string(PizzaDescription valueObject)
    {
        return valueObject.Value;
    }
}