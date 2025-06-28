using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;
using AutoMapper;

namespace Application.Services;

public class AlbumService : Service<AlbumDto, Album>, IAlbumService
{
    private readonly IAlbumRepository _albumRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public AlbumService(
        IAlbumRepository repository,
        IArtistRepository artistRepository,
        IUnitOfWork unitOfWork,
        IMapper mapper
    )
        : base(repository, unitOfWork, mapper)
    {
        _albumRepository = repository;
        _artistRepository = artistRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<IEnumerable<Album>> GetAllAlbumsAsync() =>
        await _albumRepository.GetAllAlbumsAsync();

    public async Task<Album?> GetAlbumByIdAsync(Guid id) =>
        await _albumRepository.GetAlbumByIdAsync(id);

    public async Task AddManyAlbumsAsync(IEnumerable<AlbumDto> albumDtos)
    {
        var normalizedDtos = NormalizeAlbumDtos(albumDtos);

        var artistDict = await EnsureArtistsExistAsync(
            normalizedDtos.Select(dto => dto.ArtistName!).Distinct());

        var albums = normalizedDtos.Select(dto =>
        {
            var artist = artistDict[dto.ArtistName!];
            return new Album(dto.Title, dto.ReleaseDate, artist.Id);
        });

        await _albumRepository.SaveRangeAsync(albums);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateAlbumAsync(AlbumDto editAlbum)
    {
        var album = await _albumRepository.GetAlbumByIdAsync(editAlbum.Id!.Value)
            ?? throw new Exception("NotFound");

        var artist = await _artistRepository.GetByNameAsync(editAlbum.ArtistName!)
            ?? throw new Exception("NotFound");

        album.Update(
            editAlbum.Title,
            editAlbum.ReleaseDate,
            editAlbum.Type,
            artist.Id
        );

        _albumRepository.Update(album);
        await _unitOfWork.CommitAsync();
    }

    public async Task<PagedResult<AlbumDto>> GetPaginatedAlbumsAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (albums, totalItems) = await _albumRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var albumsDTOs = _mapper.Map<IEnumerable<AlbumDto>>(albums);

        return new PagedResult<AlbumDto>
        {
            Items = albumsDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }

    // Private Methods
    private IEnumerable<AlbumDto> NormalizeAlbumDtos(IEnumerable<AlbumDto> albumDtos)
    {
        return albumDtos.Select(dto =>
        {
            dto.ArtistName = string.IsNullOrWhiteSpace(dto.ArtistName) ? "Desconhecido" : dto.ArtistName!;
            return dto;
        });
    }

    private async Task<Dictionary<string, Artist>> EnsureArtistsExistAsync(IEnumerable<string> artistNames)
    {
        var artistDict = await _artistRepository.GetDictByNamesAsync(artistNames);

        var newArtists = artistNames
            .Where(name => !artistDict.ContainsKey(name))
            .Distinct(StringComparer.CurrentCultureIgnoreCase)
            .Select(name => new Artist(name));

        if (newArtists.Any())
        {
            await _artistRepository.SaveRangeAsync(newArtists);
            await _unitOfWork.CommitAsync();

            foreach (var artist in newArtists)
                artistDict[artist.Name] = artist;
        }

        return artistDict;
    }
}
