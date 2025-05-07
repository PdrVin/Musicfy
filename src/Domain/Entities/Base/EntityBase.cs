namespace Domain.Entities.Base;

public abstract class EntityBase : IEntity
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    protected EntityBase()
    {
        Id = Id == Guid.Empty ? Guid.NewGuid() : Id;
        CreatedAt = DateTime.UtcNow;
        UpdatedAt = DateTime.UtcNow;
    }
}