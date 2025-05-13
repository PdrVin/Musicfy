using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IArtistService : IService<ArtistDto, Artist>
{
    Task<IEnumerable<Artist>> GetAllArtistsWithDataAsync();
    Task AddArtistAsync(ArtistDto artistDto);
    Task UpdateArtistAsync(Artist artist);
    Task DeleteArtistAsync(Guid id);
}
