using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IMusicRepository : IRepository<Music>
{
    Task<IEnumerable<Music>> GetMusicsWithAlbumAndArtistAsync();
}

