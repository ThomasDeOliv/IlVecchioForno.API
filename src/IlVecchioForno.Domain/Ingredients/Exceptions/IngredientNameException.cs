using IlVecchioForno.Domain.Common.Exceptions;

namespace IlVecchioForno.Domain.Ingredients.Exceptions;

public sealed class IngredientNameException : ValueObjectBaseException
{
    public IngredientNameException(string message) : base(message)
    {
    }
}