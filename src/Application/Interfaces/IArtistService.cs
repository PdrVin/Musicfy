using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IArtistService : IService<ArtistDto, Artist>
{
    Task<IEnumerable<Artist>> GetAllArtistsWithDataAsync();
    Task AddArtistAsync(ArtistDto artistDto);
    Task AddManyArtistsAsync(IEnumerable<ArtistDto> artistDtos);
    Task UpdateArtistAsync(Artist artist);
    Task<List<Artist>> GetTopArtistsByMusicAsync(int top);
}
