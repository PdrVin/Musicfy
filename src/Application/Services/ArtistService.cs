using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ArtistService : Service<ArtistDto, Artist>, IArtistService
{
    private readonly IArtistRepository _artistRepository;

    public ArtistService(IArtistRepository repository) : base(repository) =>
        _artistRepository = repository;

    public async Task AddArtistAsync(ArtistDto artistDto)
    {
        Artist artist = new() { Name = artistDto.Name };
        await _artistRepository.SaveAsync(artist);
    }

    public async Task UpdateArtistAsync(ArtistDto artistDto)
    {
        Artist artist = new() { Name = artistDto.Name };

        _artistRepository.Update(artist);
        await Task.CompletedTask;
    }
}
