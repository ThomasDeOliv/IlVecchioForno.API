using IlVecchioForno.Domain.Pizzas.Exceptions;

namespace IlVecchioForno.Domain.Pizzas;

public sealed class PizzaName
{
    private readonly string _value;
    
    public PizzaName(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new PizzaNameException("Name cannot be null, empty or composed of whitespaces.");
        }

        if (PizzaInvariant.NameMaxLength < value.Length)
        {
            throw new PizzaNameException($"Name maximum length reached ({PizzaInvariant.NameMaxLength} characters).");
        }
        
        this._value = value;
    }
    
    public static implicit operator string(PizzaName valueObject) =>  valueObject._value;
}