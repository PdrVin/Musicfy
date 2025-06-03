using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAlbumService : IService<AlbumDto, Album>
{
    Task<IEnumerable<Album>> GetAllAlbumsAsync();
    Task<Album?> GetAlbumByIdAsync(Guid id);

    Task AddManyAlbumsAsync(IEnumerable<AlbumDto> albumDtos);
    Task UpdateAlbumAsync(Album album);

    Task<PagedResult<AlbumDto>> GetPaginatedAlbumsAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
