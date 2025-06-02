using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IPlaylistRepository : IRepository<Playlist>
{
    Task<IEnumerable<Playlist>> GetAllPlaylistsAsync();
    Task<Playlist?> GetPlaylistByIdAsync(Guid id);
    Task<(IEnumerable<Playlist> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
