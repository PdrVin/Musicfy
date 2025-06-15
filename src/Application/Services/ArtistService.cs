using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;

namespace Application.Services;

public class ArtistService : Service<ArtistDto, Artist>, IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public ArtistService(
        IArtistRepository repository,
        IUnitOfWork unitOfWork
    )
        : base(repository, unitOfWork)
    {
        _artistRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Artist>> GetAllArtistsAsync() =>
        await _artistRepository.GetAllArtistsAsync();

    public async Task<Artist?> GetArtistByIdAsync(Guid id) =>
        await _artistRepository.GetArtistByIdAsync(id);

    public async Task<Artist?> GetByNameAsync(string name) =>
        await _artistRepository.GetByNameAsync(name);

    public async Task AddManyArtistsAsync(IEnumerable<ArtistDto> artistDtos)
    {
        var artists = artistDtos.Select(dto => new Artist(dto.Name));

        await _artistRepository.SaveRangeAsync(artists);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateArtistAsync(ArtistDto editArtist)
    {
        Artist artist = await _artistRepository.GetByIdAsync(editArtist.Id!.Value);

        artist.Update(editArtist.Name);

        _artistRepository.Update(artist);
        await _unitOfWork.CommitAsync();
    }

    public async Task<IEnumerable<Artist>> GetTopArtistsByMusicAsync(int top)
    {
        return await _artistRepository.GetTopArtistsByMusicAsync(top);
    }

    public async Task<IEnumerable<Artist>> GetTopArtistsByAlbumAsync(int top)
    {
        return await _artistRepository.GetTopArtistsByAlbumAsync(top);
    }

    public async Task<PagedResult<ArtistDto>> GetPaginatedArtistsAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (artists, totalItems) = await _artistRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var artistDtos = artists.Select(artist => new ArtistDto
        (
            artist.Id,
            artist.Name,

            artist.Albums?.Select(album => new AlbumDto
            {
                Id = album.Id,
                Title = album.Title
            }),

            artist.Musics?.Select(music => new MusicDto
            {
                Id = music.Id,
                Title = music.Title
            })
        ));

        return new PagedResult<ArtistDto>
        {
            Items = artistDtos,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
