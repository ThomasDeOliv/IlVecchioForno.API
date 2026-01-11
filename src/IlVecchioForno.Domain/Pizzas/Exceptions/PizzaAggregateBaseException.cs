using IlVecchioForno.Domain.Commons.Exceptions;

namespace IlVecchioForno.Domain.Pizzas.Exceptions;

public sealed class PizzaAggregateBaseException : AggregateBaseException
{
    public PizzaAggregateBaseException(string message) : base(message)
    {
    }
}