using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    Task<IEnumerable<Album>> GetAlbumsWithArtistAsync();
}
