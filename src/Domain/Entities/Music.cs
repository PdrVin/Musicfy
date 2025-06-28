using Domain.Entities.Base;

namespace Domain.Entities;

public class Music : EntityBase
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }

    public Guid? AlbumId { get; set; }
    public Album? Album { get; set; }

    public Guid? ArtistId { get; set; }
    public Artist? Artist { get; set; }

    public ICollection<Playlist>? Playlists { get; set; }

    public Music() { }

    public Music(string title, TimeSpan duration, Guid? albumId, Guid? artistId)
    {
        Title = title;
        Duration = duration;
        AlbumId = albumId;
        Album = null;
        ArtistId = artistId;
        Artist = null;
        CreatedAt = DateTime.Now;
        Playlists = new List<Playlist>();
    }

    public void Update(string title, TimeSpan duration, Guid? albumId, Guid? artistId)
    {
        if (!Title.Equals(title)) Title = title;

        if (!Duration.ToString().Equals(duration.ToString())) Duration = duration;

        if (AlbumId != albumId)
        {
            AlbumId = albumId;
            Album = null;
        }

        if (ArtistId != artistId)
        {
            ArtistId = artistId;
            Artist = null;
        }

        UpdatedAt = DateTime.Now;
    }
}