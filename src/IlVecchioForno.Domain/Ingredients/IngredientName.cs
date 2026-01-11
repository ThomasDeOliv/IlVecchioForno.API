using IlVecchioForno.Domain.Ingredients.Exceptions;

namespace IlVecchioForno.Domain.Ingredients;

public sealed class IngredientName
{
    private readonly string _value;
    
    public IngredientName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new IngredientNameException("Provided value cannot be null or whitespace.");
        }

        if (IngredientInvariant.NameMaxLength < value.Length)
        {
            throw new IngredientNameException($"Name maximum length reached ({IngredientInvariant.NameMaxLength} characters).");
        }
        
        this._value = value;
    }
    
    public static implicit operator string(IngredientName valueObject) =>  valueObject._value;
}