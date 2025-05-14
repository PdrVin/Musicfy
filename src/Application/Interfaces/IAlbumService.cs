using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAlbumService : IService<AlbumDto, Album>
{
    Task<IEnumerable<Album>> GetAllWithDataAsync();
    Task<Album?> GetByIdWithDataAsync(Guid id);
    Task AddAlbumAsync(AlbumDto albumDto);
    Task UpdateAlbumAsync(Album album);
}
