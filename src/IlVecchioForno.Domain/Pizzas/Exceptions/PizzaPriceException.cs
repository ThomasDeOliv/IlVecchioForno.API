using IlVecchioForno.Domain.Commons.Exceptions;

namespace IlVecchioForno.Domain.Pizzas.Exceptions;

public sealed class PizzaPriceException : ValueObjectBaseException
{
    public PizzaPriceException(string message) : base(message)
    {
    }
}