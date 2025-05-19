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
    )
        : base(repository, unitOfWork)
    {
        _albumRepository = repository;
        _artistRepository = artistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Album>> GetAllWithDataAsync() =>
        await _albumRepository.GetAllWithDataAsync();

    public async Task<Album?> GetByIdWithDataAsync(Guid id) =>
        await _albumRepository.GetByIdWithDataAsync(id);

    public async Task AddAlbumAsync(AlbumDto albumDto)
    {
        Artist? artist = await _artistRepository.GetByNameAsync(albumDto.ArtistName)
            ?? throw new InvalidOperationException("Artist NotFound.");

        Album album = new()
        {
            Title = albumDto.Title,
            ReleaseDate = albumDto.ReleaseDate,
            ArtistId = artist.Id,
            CreatedAt = DateTime.Now,
        };

        await _albumRepository.SaveAsync(album);
        await _unitOfWork.CommitAsync();
    }

    public async Task AddManyAlbumsAsync(IEnumerable<AlbumDto> albumDtos)
    {
        var artistNames = albumDtos.Select(dto => dto.ArtistName).Distinct().ToList();
        var artists = await _artistRepository.GetByNamesAsync(artistNames);

        var artistDict = artists.ToDictionary(a => a.Name, StringComparer.OrdinalIgnoreCase);

        var albums = albumDtos.Select(dto =>
        {
            if (!artistDict.TryGetValue(dto.ArtistName, out var artist))
                throw new InvalidOperationException($"Artist '{dto.ArtistName}' NotFound.");

            return new Album
            {
                Title = dto.Title,
                ReleaseDate = dto.ReleaseDate,
                ArtistId = artist.Id,
                CreatedAt = DateTime.Now
            };
        }).ToList();

        await _albumRepository.SaveRangeAsync(albums);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAlbumAsync(Album editAlbum)
    {
        Album album = await _albumRepository.GetByIdWithDataAsync(editAlbum.Id)
            ?? throw new Exception("NotFound");

        Artist? artist = await _artistRepository.GetByNameAsync(editAlbum.Artist.Name)
            ?? throw new Exception("NotFound");

        album.Title = editAlbum.Title;
        album.ReleaseDate = editAlbum.ReleaseDate;
        album.ArtistId = artist.Id;
        album.UpdatedAt = DateTime.Now;

        _albumRepository.Update(album);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }
}
