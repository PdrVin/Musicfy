using Application.DTOs;
using Application.Helpers.Pagination;
using Application.Interfaces;
using Application.Services.Base;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Base;

namespace Application.Services;

public class MusicService : Service<MusicDto, Music>, IMusicService
{
    private readonly IMusicRepository _musicRepository;
    private readonly IAlbumRepository _albumRepository;
    private readonly IArtistRepository _artistRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MusicService(
        IMusicRepository repository,
        IAlbumRepository albumRepository,
        IArtistRepository artistRepository,
        IUnitOfWork unitOfWork
    )
        : base(repository, unitOfWork)
    {
        _musicRepository = repository;
        _albumRepository = albumRepository;
        _artistRepository = artistRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<Music>> GetAllMusicsAsync() =>
        await _musicRepository.GetAllMusicsAsync();

    public async Task<Music?> GetMusicByIdAsync(Guid id) =>
        await _musicRepository.GetMusicByIdAsync(id);

    public async Task AddManyMusicsAsync(IEnumerable<MusicDto> musicDtos)
    {
        var artistNames = musicDtos.Select(dto => dto.ArtistName!).Distinct();
        var albumTitles = musicDtos.Select(dto => dto.AlbumTitle!).Distinct();

        var artistDict = await _artistRepository.GetDictByNamesAsync(artistNames);
        var albumDict = await _albumRepository.GetDictByTitlesAsync(albumTitles);

        var musics = musicDtos.Select(dto =>
        {
            if (!artistDict.TryGetValue(dto.ArtistName!, out var artist))
                throw new InvalidOperationException($"Artist '{dto.ArtistName}' NotFound.");

            if (!albumDict.TryGetValue(dto.AlbumTitle!, out var album))
                throw new InvalidOperationException($"Album '{dto.AlbumTitle}' NotFound.");

            return new Music(
                dto.Title,
                new TimeSpan(0, dto.Duration.Hours, dto.Duration.Minutes),
                artist.Id,
                album.Id
            );
        });

        await _musicRepository.SaveRangeAsync(musics);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateMusicAsync(Music editMusic)
    {
        Music? music = await _musicRepository.GetMusicByIdAsync(editMusic.Id)
            ?? throw new InvalidOperationException("Music NotFound.");

        Album? album = await _albumRepository.GetByTitleAsync(editMusic.Album.Title)
            ?? throw new InvalidOperationException("Album NotFound.");

        Artist? artist = await _artistRepository.GetByNameAsync(editMusic.Artist.Name)
            ?? throw new InvalidOperationException("Artist NotFound.");

        music.Update(
            editMusic.Title,
            new TimeSpan(0, editMusic.Duration.Hours, editMusic.Duration.Minutes),
            album.Id,
            artist.Id
        );

        _musicRepository.Update(music);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }

    public async Task<PagedResult<MusicDto>> GetPaginatedMusicsAsync(
        int pageNumber, int pageSize, string searchTerm = "")
    {
        var (musics, totalItems) = await _musicRepository
            .GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var musicsDTOs = musics.Select(music => new MusicDto
        (
            music.Id,
            music.Title,
            music.Duration,
            music.AlbumId ?? Guid.Empty,
            music.Album.Title,
            music.ArtistId ?? Guid.Empty,
            music.Artist.Name
        ));

        return new PagedResult<MusicDto>
        {
            Items = musicsDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
