using IlVecchioForno.Domain.Common.Exceptions;

namespace IlVecchioForno.Domain.PizzaIngredients.Exceptions;

public sealed class PizzaIngredientQuantityException : ValueObjectBaseException
{
    public PizzaIngredientQuantityException(string message) : base(message)
    {
    }
}