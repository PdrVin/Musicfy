using Domain.Entities.Base;

namespace Domain.Entities;

public class Playlist : EntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Music>? Musics { get; set; } = new List<Music>();
}
