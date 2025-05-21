using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IPlaylistRepository : IRepository<Playlist>
{
    Task<List<Playlist>> GetAllWithDataAsync();
    Task<Playlist?> GetByIdWithDataAsync(Guid id);
}
