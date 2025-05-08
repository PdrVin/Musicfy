using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IAlbumService : IService<AlbumDto, Album>
{
    
}
