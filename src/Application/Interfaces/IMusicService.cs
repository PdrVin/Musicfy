using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IMusicService : IService<MusicDto, Music>
{
    
}
