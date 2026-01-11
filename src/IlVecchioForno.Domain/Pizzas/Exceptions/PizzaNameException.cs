using IlVecchioForno.Domain.Commons.Exceptions;

namespace IlVecchioForno.Domain.Pizzas.Exceptions;

public sealed class PizzaNameException : ValueObjectBaseException
{
    public PizzaNameException(string message) : base(message)
    {
    }
}