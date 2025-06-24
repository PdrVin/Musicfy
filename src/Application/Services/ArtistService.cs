using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using AutoMapper;

namespace Application.Services;

public class ArtistService : Service<ArtistDto, Artist>, IArtistService
{
    private readonly IArtistRepository _artistRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public ArtistService(
        IArtistRepository repository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _artistRepository = repository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
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

        var artistDtos = _mapper.Map<IEnumerable<ArtistDto>>(artists);

        return new PagedResult<ArtistDto>
        {
            Items = artistDtos,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
