namespace Domain.Entities.Base;

public abstract class EntityBase : IEntity
{
    protected EntityBase() =>
        Id = Id == Guid.Empty ? Guid.NewGuid() : Id;

    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}