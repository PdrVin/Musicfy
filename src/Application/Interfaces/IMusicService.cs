using Application.DTOs;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IMusicService : IService<MusicDto, Music>
{
    Task<IEnumerable<Music>> GetAllWithDataAsync();
    Task<Music?> GetByIdWithDataAsync(Guid id);
    Task AddMusicAsync(MusicDto musicDto);
    Task UpdateMusicAsync(Music editMusic);
}
