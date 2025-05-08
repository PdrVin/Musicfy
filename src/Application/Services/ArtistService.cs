using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class ArtistService : Service<ArtistDto, Artist>, IArtistService
{
    private readonly IArtistRepository _repository;

    public ArtistService(IArtistRepository repository) : base(repository) =>
        _repository = repository;
}
