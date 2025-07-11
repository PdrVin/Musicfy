using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IMusicRepository : IRepository<Music>
{
    Task<IEnumerable<Music>> GetAllMusicsAsync();
    Task<Music?> GetMusicByIdAsync(Guid id);
    Task<List<Music>> GetManyByIdsAsync(List<Guid> ids);
    Task<(IEnumerable<Music> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}

