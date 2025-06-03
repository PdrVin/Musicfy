using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces.Base;
using Domain.Entities;

namespace Application.Interfaces;

public interface IMusicService : IService<MusicDto, Music>
{
    Task<IEnumerable<Music>> GetAllMusicsAsync();
    Task<Music?> GetMusicByIdAsync(Guid id);

    Task AddManyMusicsAsync(IEnumerable<MusicDto> musicDtos);
    Task UpdateMusicAsync(Music editMusic);

    Task<PagedResult<MusicDto>> GetPaginatedMusicsAsync(
        int pageNumber, int pageSize, string searchTerm = "");
}
