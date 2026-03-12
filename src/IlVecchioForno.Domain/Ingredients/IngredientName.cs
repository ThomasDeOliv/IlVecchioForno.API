using IlVecchioForno.Domain.Ingredients.Exceptions;

namespace IlVecchioForno.Domain.Ingredients;

public sealed class IngredientName
{
    public IngredientName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new IngredientNameException(
                $"{nameof(IngredientName)} cannot be instantiated from null, empty or whitespace value."
            );

        if (IngredientInvariant.NameMaxLength < value.Length)
            throw new IngredientNameException(
                $"{nameof(IngredientName)} exceeds maximum length of {IngredientInvariant.NameMaxLength} characters."
            );

        this.Value = value;
    }

    public string Value { get; }

    public static implicit operator string(IngredientName valueObject)
    {
        return valueObject.Value;
    }

    public static implicit operator IngredientName(string value)
    {
        return new IngredientName(value);
    }
}