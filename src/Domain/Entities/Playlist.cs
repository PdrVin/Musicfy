using Domain.Entities.Base;

namespace Domain.Entities;

public class Playlist : EntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Music>? Musics { get; set; }

    public Playlist() { }

    public Playlist(string name)
    {
        Name = name;
        CreatedAt = DateTime.Now;
        Musics = new List<Music>();
    }

    public void Update(string name)
    {
        Name = name;
        UpdatedAt = DateTime.Now;
    }
}

