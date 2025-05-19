using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;

namespace Application.Services;

public class PlaylistService : Service<PlaylistDto, Playlist>, IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlaylistService(
        IPlaylistRepository repository,
        IUnitOfWork unitOfWork
    )
        : base(repository, unitOfWork)
    {
        _playlistRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddPlaylistAsync(PlaylistDto playlistDto)
    {
        Playlist playlist = new()
        {
            Name = playlistDto.Name,
            CreatedAt = DateTime.Now,
        };

        await _playlistRepository.SaveAsync(playlist);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdatePlaylistAsync(Playlist editPlaylist)
    {
        Playlist playlist = await _playlistRepository.GetByIdAsync(editPlaylist.Id);

        playlist.Name = editPlaylist.Name;
        playlist.UpdatedAt = DateTime.Now;

        _playlistRepository.Update(playlist);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }
}
