using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;

namespace Application.Services;

public class MusicService : Service<MusicDto, Music>, IMusicService
{
    private readonly IMusicRepository _musicRepository;

    public MusicService(IMusicRepository repository) : base(repository) =>
        _musicRepository = repository;

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
        await Task.CompletedTask;
    }
}
