namespace IlVecchioForno.Domain.Pizzas;

public static class PizzaInvariant
{
    public const int NameMinLength = 1;
    public const int NameMaxLength = 256;
    public const int DescriptionMinLength = 1;
    public const int DescriptionMaxLength = 1024;
    public const int MinPrice = 0;
}