using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPlaylistService : IService<PlaylistDto, Playlist>
{
    Task AddPlaylistAsync(PlaylistDto playlistDto);
    Task UpdatePlaylistAsync(PlaylistDto playlistDto);
    Task DeletePlaylistAsync(Guid id);
}
