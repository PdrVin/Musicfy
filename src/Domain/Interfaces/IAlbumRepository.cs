using Domain.Entities;
using Domain.Interfaces.Base;

namespace Domain.Interfaces;

public interface IAlbumRepository : IRepository<Album>
{
    Task<IEnumerable<Album>> GetAllWithDataAsync();
    Task<Album?> GetByIdWithDataAsync(Guid id);
    Task<Album?> GetByTitleAsync(string title);
    Task<List<Album>> GetByTitlesAsync(IEnumerable<string> titles);
}
