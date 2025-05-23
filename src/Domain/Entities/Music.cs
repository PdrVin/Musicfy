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
}
