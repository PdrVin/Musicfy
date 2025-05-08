using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class PlaylistService : Service<PlaylistDto, Playlist>, IPlaylistService
{
    private readonly IPlaylistRepository _repository;

    public PlaylistService(IPlaylistRepository repository) : base(repository) =>
        _repository = repository;
}
