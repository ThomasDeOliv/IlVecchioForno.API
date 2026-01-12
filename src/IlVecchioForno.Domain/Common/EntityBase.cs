namespace IlVecchioForno.Domain.Common;

public abstract class EntityBase : IAuditable
{
    protected EntityBase()
    {
        this.CreatedAt = default!;
        this.UpdatedAt = default!;
    }

    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }
}