using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IMusicRepository : IRepository<Music>
{
    Task<IEnumerable<Music>> GetAllWithDataAsync();
    Task<Music?> GetByIdWithDataAsync(Guid id);
    Task<List<Music>> GetManyByIdsAsync(List<Guid> ids);
}

