using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IPlaylistRepository : IRepository<Playlist>
{
    Task<IEnumerable<Playlist>> GetAllWithDataAsync();
    Task<Playlist?> GetByIdWithDataAsync(Guid id);
    Task<(IEnumerable<Playlist> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
