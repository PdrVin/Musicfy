using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class PlaylistService : Service<PlaylistDto, Playlist>, IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository;

    public PlaylistService(IPlaylistRepository repository) : base(repository) =>
        _playlistRepository = repository;

    public async Task AddPlaylistAsync(PlaylistDto playlistDto)
    {
        Playlist playlist = new() { Name = playlistDto.Name };
        await _playlistRepository.SaveAsync(playlist);
    }

    public async Task UpdatePlaylistAsync(PlaylistDto playlistDto)
    {
        Playlist playlist = new() { Name = playlistDto.Name };

        _playlistRepository.Update(playlist);
        await Task.CompletedTask;
    }
}
