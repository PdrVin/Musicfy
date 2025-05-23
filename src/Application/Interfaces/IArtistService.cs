using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IArtistService : IService<ArtistDto, Artist>
{
    Task<IEnumerable<Artist>> GetAllWithDataAsync();
    Task AddManyArtistsAsync(IEnumerable<ArtistDto> artistDtos);
    Task UpdateArtistAsync(Artist artist);
    Task<IEnumerable<Artist>> GetTopArtistsByMusicAsync(int top);
}
