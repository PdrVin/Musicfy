using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IPlaylistService : IService<PlaylistDto, Playlist>
{
    Task<IEnumerable<Playlist>> GetAllPlaylistsAsync();
    Task<Playlist?> GetPlaylistByIdAsync(Guid id);

    Task AddPlaylistAsync(PlaylistDto playlistDto);
    Task UpdatePlaylistAsync(Playlist playlist);

    Task AddMusicToPlaylistAsync(Guid playlistId, Guid musicId);
    Task AddMusicsToPlaylistAsync(Guid playlistId, List<Guid> musicIds);

    Task<PagedResult<PlaylistDto>> GetPaginatedPlaylistsAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
