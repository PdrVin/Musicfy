using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IArtistService : IService<ArtistDto, Artist>
{
    
}
