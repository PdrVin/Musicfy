using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPlaylistService : IService<PlaylistDto, Playlist>
{
    Task<IEnumerable<Playlist>> GetAllWithDataAsync();
    Task AddPlaylistAsync(PlaylistDto playlistDto);
    Task UpdatePlaylistAsync(Playlist playlist);
    Task AddMusicToPlaylistAsync(Guid playlistId, Guid musicId);
    Task AddMusicsToPlaylistAsync(Guid playlistId, List<Guid> musicIds);
}
