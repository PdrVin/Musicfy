using Domain.Entities.Base;
using Domain.Enums;

namespace Domain.Entities;

public class Album : EntityBase
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }
    public AlbumType Type { get; set; }

    public Guid? ArtistId { get; set; }
    public Artist? Artist { get; set; }

    public ICollection<Music>? Musics { get; set; }

    public Album() { }

    public Album(string title, DateTime releaseDate, Guid? artistId)
    {
        Title = title;
        ReleaseDate = releaseDate;
        Type = AlbumType.Album;
        ArtistId = artistId;
        Musics = new List<Music>();
        CreatedAt = DateTime.Now;
    }

    public void Update(string title, DateTime releaseDate, AlbumType type, Guid? artistId)
    {
        if (!Title.Equals(title)) Title = title;

        if (!ReleaseDate.ToString().Equals(releaseDate.ToString())) ReleaseDate = releaseDate;

        if (Type != type) Type = type;

        if (ArtistId != artistId)
        {
            ArtistId = artistId;
            Artist = null;
        }

        UpdatedAt = DateTime.Now;
    }
}
