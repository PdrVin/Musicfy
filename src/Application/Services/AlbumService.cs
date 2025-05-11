using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class AlbumService : Service<AlbumDto, Album>, IAlbumService
{
    private readonly IAlbumRepository _albumRepository;

    public AlbumService(IAlbumRepository repository) : base(repository)
        => _albumRepository = repository;

    public async Task AddAlbumAsync(AlbumDto albumDto)
    {
        Album Album = new()
        {
            Title = albumDto.Title,
            ReleaseDate = albumDto.ReleaseDate,
            ArtistId = albumDto.ArtistId,
            // Artist = AlbumDto.ArtistName
        };
        await _albumRepository.SaveAsync(Album);
    }

    public async Task UpdateAlbumAsync(AlbumDto albumDto)
    {
        Album Album = new()
        {
            Title = albumDto.Title,
            ReleaseDate = albumDto.ReleaseDate,
            ArtistId = albumDto.ArtistId,
            // Artist = AlbumDto.ArtistName
        };
        _albumRepository.Update(Album);
        await Task.CompletedTask;
    }

    public async Task DeleteAlbumAsync(Guid id)
    {
        _albumRepository.Delete(id);
        await Task.CompletedTask;
    }
}
