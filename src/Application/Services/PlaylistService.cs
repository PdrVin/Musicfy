using Application.DTOs;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;

namespace Application.Services;

public class PlaylistService : Service<PlaylistDto, Playlist>, IPlaylistService
{
    private readonly IPlaylistRepository _playlistRepository;
    private readonly IMusicRepository _musicRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PlaylistService(
        IPlaylistRepository repository,
        IMusicRepository musicRepository,
        IUnitOfWork unitOfWork
    )
        : base(repository, unitOfWork)
    {
        _playlistRepository = repository;
        _musicRepository = musicRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Playlist>> GetAllWithDataAsync() =>
        await _playlistRepository.GetAllWithDataAsync();

    public async Task AddPlaylistAsync(PlaylistDto playlistDto)
    {
        Playlist playlist = new()
        {
            Name = playlistDto.Name,
            CreatedAt = DateTime.Now,
        };

        await _playlistRepository.SaveAsync(playlist);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdatePlaylistAsync(Playlist editPlaylist)
    {
        Playlist playlist = await _playlistRepository.GetByIdAsync(editPlaylist.Id);

        playlist.Name = editPlaylist.Name;
        playlist.UpdatedAt = DateTime.Now;

        _playlistRepository.Update(playlist);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }

    public async Task AddMusicToPlaylistAsync(Guid musicId, Guid playlistId)
    {
        var playlist = await _playlistRepository.GetByIdWithDataAsync(playlistId);
        var music = await _musicRepository.GetByIdWithDataAsync(musicId);

        if (playlist == null || music == null) return;

        playlist.Musics ??= new List<Music>();
        if (!playlist.Musics.Any(m => m.Id == musicId))
            playlist.Musics.Add(music);

        _playlistRepository.Update(playlist);
        await _unitOfWork.CommitAsync();
    }

    public async Task AddMusicsToPlaylistAsync(Guid playlistId, List<Guid> musicIds)
    {
        Playlist playlist = await _playlistRepository.GetByIdWithDataAsync(playlistId)
            ?? throw new Exception("Playlist não encontrada.");

        List<Music> musics = await _musicRepository.GetManyByIdsAsync(musicIds);

        foreach (var music in musics)
        {
            if (!playlist.Musics!.Any(m => m.Id == music.Id))
                playlist.Musics.Add(music);
        }

        await _unitOfWork.CommitAsync();
    }
}
