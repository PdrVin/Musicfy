using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;

namespace Application.Services;

public class MusicService : Service<MusicDto, Music>, IMusicService
{
    private readonly IMusicRepository _musicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MusicService(IMusicRepository repository, IUnitOfWork unitOfWork) : base(repository)
    {
        _musicRepository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task AddMusicAsync(MusicDto musicDto)
    {
        Music music = new()
        {
            Title = musicDto.Title,
            Duration = musicDto.Duration,
            // Album = musicDto.Album
            // Artist = musicDto.ArtistName
        };

        await _musicRepository.SaveAsync(music);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateMusicAsync(MusicDto musicDto)
    {
        Music music = new()
        {
            Title = musicDto.Title,
            Duration = musicDto.Duration,
            // Album = musicDto.Album
            // Artist = musicDto.ArtistName
        };

        _musicRepository.Update(music);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }

    public async Task DeleteMusicAsync(Guid id)
    {
        _musicRepository.Delete(id);
        await _unitOfWork.CommitAsync();
        await Task.CompletedTask;
    }
}
