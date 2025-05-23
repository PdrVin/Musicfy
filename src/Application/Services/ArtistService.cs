using Application.DTOs;
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

    public async Task<IEnumerable<Artist>> GetAllArtistsWithDataAsync() =>
        await _artistRepository.GetAllWithDataAsync();

    public async Task AddManyArtistsAsync(IEnumerable<ArtistDto> artistDtos)
    {
        var artists = artistDtos.Select(dto =>
            new Artist {
                Name = dto.Name,
                CreatedAt = DateTime.Now,
            }).ToList();

        await _artistRepository.SaveRangeAsync(artists);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateArtistAsync(Artist editArtist)
    {
        Artist artist = await _artistRepository.GetByIdAsync(editArtist.Id);

        artist.Name = editArtist.Name;
        artist.UpdatedAt = DateTime.Now;

        _artistRepository.Update(artist);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }

    public async Task<List<Artist>> GetTopArtistsByMusicAsync(int top)
    {
        return await _artistRepository.GetTopArtistsByMusicAsync(top);
    }
}
