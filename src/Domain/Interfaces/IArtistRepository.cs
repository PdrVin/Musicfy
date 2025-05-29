using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IArtistRepository : IRepository<Artist>
{
    Task<IEnumerable<Artist>> GetAllWithDataAsync();
    Task<Artist?> GetByNameAsync(string name);
    Task<List<Artist>> GetByNamesAsync(IEnumerable<string> names);
    Task<List<Artist>> GetTopArtistsByMusicAsync(int top);
    Task<(IEnumerable<Artist> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}

