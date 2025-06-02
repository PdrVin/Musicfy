using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IArtistRepository : IRepository<Artist>
{
    Task<IEnumerable<Artist>> GetAllArtistsAsync();
    Task<Artist?> GetByNameAsync(string name);
    Task<Dictionary<string, Artist>> GetDictByNamesAsync(IEnumerable<string> names);
    Task<IEnumerable<Artist>> GetTopArtistsByMusicAsync(int top);
    Task<IEnumerable<Artist>> GetTopArtistsByAlbumAsync(int top);
    Task<(IEnumerable<Artist> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}

