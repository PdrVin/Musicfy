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

    public ArtistService(IArtistRepository repository, IUnitOfWork unitOfWork) : base(repository)
    {
        _artistRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Artist>> GetAllArtistsWithDataAsync()
    {
        return await _artistRepository.GetAllWithDataAsync();
    }

    public async Task AddArtistAsync(ArtistDto artistDto)
    {
        Artist artist = new() { Name = artistDto.Name };

        await _artistRepository.SaveAsync(artist);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateArtistAsync(Artist artist)
    {
        Artist entity = _artistRepository.GetByIdAsync(artist.Id).Result;

        entity.Name = artist.Name;
        entity.UpdatedAt = DateTime.Now;

        _artistRepository.Update(entity);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }

    public async Task DeleteArtistAsync(Guid id)
    {
        _artistRepository.Delete(id);
        await _unitOfWork.CommitAsync();
        await Task.CompletedTask;
    }
}
