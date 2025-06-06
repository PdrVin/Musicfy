using Domain.Entities.Base;

namespace Domain.Entities;

public class Artist : EntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Album>? Albums { get; set; } = new List<Album>();
    public ICollection<Music>? Musics { get; set; } = new List<Music>();
}
