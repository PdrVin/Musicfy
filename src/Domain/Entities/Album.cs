using Domain.Entities.Base;

namespace Domain.Entities;

public class Album : EntityBase
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }

    public Guid? ArtistId { get; set; }
    public Artist? Artist { get; set; }

    public ICollection<Music>? Musics { get; set; }

    public Album() { }

    public Album(string title, DateTime releaseDate, Guid? artistId)
    {
        Title = title;
        ReleaseDate = releaseDate;
        ArtistId = artistId;
        Musics = new List<Music>();
        CreatedAt = DateTime.Now;
    }

    public void Update(string title, DateTime releaseDate, Guid? artistId)
    {
        Title = title;
        ReleaseDate = releaseDate;
        ArtistId = artistId;
        Artist = null;
        UpdatedAt = DateTime.Now;
    }
}
