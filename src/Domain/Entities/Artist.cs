using Domain.Entities.Base;

namespace Domain.Entities;

public class Artist : EntityBase
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public ICollection<Album>? Albums { get; set; }
    public ICollection<Music>? Musics { get; set; }

    public Artist() { }

    public Artist(string name)
    {
        Name = name;
        Albums = new List<Album>();
        Musics = new List<Music>();
        CreatedAt = DateTime.Now;
    }

    public void Update(string name)
    {
        if (!Name.Equals(name)) Name = name;
        UpdatedAt = DateTime.Now;
    }
}

