using System.Collections;
using Domain.Entities.Base;

namespace Domain.Entities;

public class Album : EntityBase
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public DateTime ReleaseDate { get; set; }

    public Guid ArtistId { get; set; }
    public Artist Artist { get; set; }

    public ICollection<Music> Musics { get; set; } = new List<Music>();
}