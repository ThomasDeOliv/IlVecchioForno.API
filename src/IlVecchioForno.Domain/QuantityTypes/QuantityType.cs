using IlVecchioForno.Domain.Common;

namespace IlVecchioForno.Domain.QuantityTypes;

public sealed class QuantityType : EntityBase
{
    // EF
    private QuantityType() : base()
    {
        this.Id = 0;
        this.Name = null!;
        this.Unit = null!;
    }

    public QuantityType(QuantityTypeName name, QuantityTypeUnit unit, short id = 0) : this()
    {
        this.Id = id;
        this.Name = name;
        this.Unit = unit;
    }

    public short Id { get; private set; }
    public QuantityTypeName Name { get; private set; }
    public QuantityTypeUnit Unit { get; private set; }
}