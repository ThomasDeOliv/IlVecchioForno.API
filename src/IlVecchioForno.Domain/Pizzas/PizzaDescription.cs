using IlVecchioForno.Domain.Pizzas.Exceptions;

namespace IlVecchioForno.Domain.Pizzas;

public sealed class PizzaDescription
{
    public PizzaDescription(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new PizzaDescriptionException("Description cannot be composed of whitespace characters only.");

        if (PizzaInvariant.DescriptionMaxLength < value.Length)
            throw new PizzaDescriptionException(
                $"Description maximum length reached ({PizzaInvariant.DescriptionMaxLength} characters).");

        this.Value = value;
    }

    public string Value { get; }

    public static implicit operator string(PizzaDescription valueObject)
    {
        return valueObject.Value;
    }
}