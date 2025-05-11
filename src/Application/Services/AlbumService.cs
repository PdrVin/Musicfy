using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;

namespace Application.Services;

public class AlbumService : Service<AlbumDto, Album>, IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AlbumService(IAlbumRepository repository, IUnitOfWork unitOfWork) : base(repository)
    {
        _albumRepository = repository;
        _unitOfWork = unitOfWork;
    }

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
        await _unitOfWork.CommitAsync();
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
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }

    public async Task DeleteAlbumAsync(Guid id)
    {
        _albumRepository.Delete(id);
        await _unitOfWork.CommitAsync();
        await Task.CompletedTask;
    }
}
