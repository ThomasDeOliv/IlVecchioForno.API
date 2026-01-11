using IlVecchioForno.Domain.Commons;

namespace IlVecchioForno.Domain.QuantityTypes;

public sealed class QuantityType : EntityBase
{
    // EF
    private QuantityType() : base()
    {
        this.Id = -1;
        this.Name = null!;
        this.Unit = null!;
    }

    public QuantityType(QuantityTypeName name, QuantityTypeUnit? unit, short id = -1) : this()
    {
        this.Id = id;
        this.Name = name;
        this.Unit = unit;
    }

    public short Id { get; private set; }
    public QuantityTypeName Name { get; private set; }
    public QuantityTypeUnit? Unit { get; private set; }
}