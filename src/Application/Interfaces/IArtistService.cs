using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IArtistService : IService<ArtistDto, Artist>
{
    Task<IEnumerable<Artist>> GetAllArtistsAsync();
    Task<Artist?> GetArtistByIdAsync(Guid id);
    Task<Artist?> GetByNameAsync(string name);

    Task AddManyArtistsAsync(IEnumerable<ArtistDto> artistDtos);
    Task UpdateArtistAsync(Artist artist);

    Task<IEnumerable<Artist>> GetTopArtistsByMusicAsync(int top);
    Task<IEnumerable<Artist>> GetTopArtistsByAlbumAsync(int top);

    Task<PagedResult<ArtistDto>> GetPaginatedArtistsAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
