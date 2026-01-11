using IlVecchioForno.Domain.Commons.Exceptions;

namespace IlVecchioForno.Domain.Pizzas.Exceptions;

public sealed class PizzaDescriptionException : ValueObjectBaseException
{
    public PizzaDescriptionException(string message) : base(message)
    {
    }
}