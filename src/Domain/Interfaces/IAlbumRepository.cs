using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    Task<IEnumerable<Album>> GetAllAlbumsAsync();
    Task<Album?> GetAlbumByIdAsync(Guid id);
    Task<Album?> GetByTitleAsync(string title);
    Task<Dictionary<string, Album>> GetDictByTitlesAsync(IEnumerable<string> titles);
    Task<(IEnumerable<Album> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
