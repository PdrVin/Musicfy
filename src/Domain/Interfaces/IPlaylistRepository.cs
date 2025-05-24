using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IPlaylistRepository : IRepository<Playlist>
{
    Task<IEnumerable<Playlist>> GetAllWithDataAsync();
    Task<Playlist?> GetByIdWithDataAsync(Guid id);
}
