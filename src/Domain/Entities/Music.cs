using Domain.Entities.Base;

namespace Domain.Entities;

public class Music : EntityBase
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public TimeSpan Duration { get; set; }

    public Guid? ArtistId { get; set; }
    public Artist? Artist { get; set; }

    public Guid? AlbumId { get; set; }
    public Album? Album { get; set; }

    public ICollection<Playlist>? Playlists { get; set; }

    public Music() { }

    public Music(string title, TimeSpan duration, Guid? artistId, Guid? albumId)
    {
        Title = title;
        Duration = duration;
        ArtistId = artistId;
        AlbumId = albumId;
        CreatedAt = DateTime.Now;
        Playlists = new List<Playlist>();
    }

    public void Update(string title, TimeSpan duration, Guid? artistId, Guid? albumId)
    {
        Title = title;
        Duration = duration;
        ArtistId = artistId;
        Artist = null;
        AlbumId = albumId;
        Album = null;
        UpdatedAt = DateTime.Now;
    }
}