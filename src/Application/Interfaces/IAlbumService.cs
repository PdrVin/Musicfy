using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAlbumService : IService<AlbumDto, Album>
{
    Task AddAlbumAsync(AlbumDto albumDto);
    Task UpdateAlbumAsync(AlbumDto albumDto);
    Task DeleteAlbumAsync(Guid id);
}
