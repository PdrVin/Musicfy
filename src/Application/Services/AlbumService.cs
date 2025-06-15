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

    public async Task<IEnumerable<Album>> GetAllAlbumsAsync() =>
        await _albumRepository.GetAllAlbumsAsync();

    public async Task<Album?> GetAlbumByIdAsync(Guid id) =>
        await _albumRepository.GetAlbumByIdAsync(id);

    public async Task AddManyAlbumsAsync(IEnumerable<AlbumDto> albumDtos)
    {
        var artistNames = albumDtos.Select(dto => dto.ArtistName!).Distinct();
        var artistDict = await _artistRepository.GetDictByNamesAsync(artistNames);

        var albums = albumDtos.Select(dto =>
        {
            if (!artistDict.TryGetValue(dto.ArtistName!, out var artist))
                throw new InvalidOperationException($"Artist '{dto.ArtistName}' NotFound.");

            return new Album
            (
                dto.Title,
                dto.ReleaseDate,
                artist.Id
            );
        });

        await _albumRepository.SaveRangeAsync(albums);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAlbumAsync(Album editAlbum)
    {
        Album album = await _albumRepository.GetAlbumByIdAsync(editAlbum.Id)
            ?? throw new Exception("NotFound");

        Artist? artist = await _artistRepository.GetByNameAsync(editAlbum.Artist.Name)
            ?? throw new Exception("NotFound");

        album.Update(editAlbum.Title, editAlbum.ReleaseDate, artist.Id);

        _albumRepository.Update(album);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }

    public async Task<PagedResult<AlbumDto>> GetPaginatedAlbumsAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (albums, totalItems) = await _albumRepository.GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var albumsDTOs = albums.Select(album => new AlbumDto
        (
            album.Id,
            album.Title,
            album.ReleaseDate,
            album.ArtistId ?? Guid.Empty,
            album.Artist.Name,

            album.Musics?.Select(music => new MusicDto
            {
                Id = music.Id,
                Title = music.Title
            })
        ));

        return new PagedResult<AlbumDto>
        {
            Items = albumsDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
