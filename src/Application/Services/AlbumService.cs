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
    private readonly IArtistRepository _artistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public AlbumService(
        IAlbumRepository repository,
        IArtistRepository artistRepository,
        IUnitOfWork unitOfWork
        ) : base(repository)
    {
        _albumRepository = repository;
        _artistRepository = artistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Album>> GetAllWithArtistAsync()
    {
        return await _albumRepository.GetAllWithArtistAsync();
    }

    public async Task<Album?> GetByIdWithArtistAsync(Guid id)
    {
        return await _albumRepository.GetByIdWithArtistAsync(id);
    }

    public async Task AddAlbumAsync(AlbumDto albumDto)
    {
        Artist? artist = await _artistRepository.GetByNameAsync(albumDto.ArtistName)
            ?? throw new InvalidOperationException("Artist NotFound.");

        Album album = new()
        {
            Title = albumDto.Title,
            ReleaseDate = albumDto.ReleaseDate,
            ArtistId = artist.Id,
        };

        await _albumRepository.SaveAsync(album);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAlbumAsync(Album editAlbum)
    {
        Album album = _albumRepository.GetByIdWithArtistAsync(editAlbum.Id).Result
            ?? throw new Exception("NotFound");

        Artist? artist = _artistRepository.GetByNameAsync(editAlbum.Artist.Name).Result;

        album.Title = editAlbum.Title;
        album.ReleaseDate = editAlbum.ReleaseDate;
        album.ArtistId = artist.Id;
        album.UpdatedAt = DateTime.Now;

        _albumRepository.Update(album);
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
