using IlVecchioForno.Domain.Common;
using IlVecchioForno.Domain.QuantityTypes;

namespace IlVecchioForno.Domain.Ingredients;

public sealed class Ingredient : EntityBase
{
    // EF
    private Ingredient() : base()
    {
        this.Id = 0;
        this.Name = null!;
        this.QuantityType = null;
    }

    public Ingredient(IngredientName name, QuantityType? quantityType, int id = 0) : this()
    {
        this.Id = id;
        this.Name = name;
        this.QuantityType = quantityType;
    }

    public int Id { get; private set; }
    public IngredientName Name { get; private set; }
    public QuantityType? QuantityType { get; private set; }
}