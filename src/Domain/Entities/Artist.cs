using Domain.Entities.Base;

namespace Domain.Entities;

public class Artist : EntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Album>? Albums { get; set; }
    public ICollection<Music>? Musics { get; set; }
}
