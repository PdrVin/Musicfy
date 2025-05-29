using Application.DTOs;
using Application.Helpers.Pagination;
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

    public async Task<PagedResult<AlbumDto>> GetPaginatedAlbumsAsync(int pageNumber, int pageSize, string searchTerm = "")
    {
        var (albums, totalItems) = await _albumRepository.GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var albumsDTOs = albums.Select(a => new AlbumDto
        {
            Id = a.Id,
            Title = a.Title,
            ReleaseDate = a.ReleaseDate,
            ArtistId = a.ArtistId ?? Guid.Empty,
            ArtistName = a.Artist.Name,

            Musics = a.Musics?.Select(music => new MusicDto
            {
                Id = music.Id,
                Title = music.Title
            }).ToList()
        }).ToList();

        return new PagedResult<AlbumDto>
        {
            Items = albumsDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
