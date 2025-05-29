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

    public async Task<IEnumerable<Music>> GetAllWithDataAsync() =>
        await _musicRepository.GetAllWithDataAsync();

    public async Task<Music?> GetByIdWithDataAsync(Guid id) =>
        await _musicRepository.GetByIdWithDataAsync(id);

    public async Task AddManyMusicsAsync(IEnumerable<MusicDto> musicDtos)
    {
        var artistNames = musicDtos.Select(dto => dto.ArtistName).Distinct().ToList();
        var albumTitles = musicDtos.Select(dto => dto.AlbumTitle).Distinct().ToList();

        var artists = await _artistRepository.GetByNamesAsync(artistNames);
        var albums = await _albumRepository.GetByTitlesAsync(albumTitles);

        var artistDict = artists.ToDictionary(a => a.Name, StringComparer.OrdinalIgnoreCase);
        var albumDict = albums.ToDictionary(a => a.Title, StringComparer.OrdinalIgnoreCase);

        var musics = musicDtos.Select(dto =>
        {
            if (!artistDict.TryGetValue(dto.ArtistName, out var artist))
                throw new InvalidOperationException($"Artist '{dto.ArtistName}' NotFound.");

            if (!albumDict.TryGetValue(dto.AlbumTitle, out var album))
                throw new InvalidOperationException($"Album '{dto.AlbumTitle}' NotFound.");

            return new Music
            {
                Title = dto.Title,
                Duration = new TimeSpan(0, dto.Duration.Hours, dto.Duration.Minutes),
                ArtistId = artist.Id,
                AlbumId = album.Id,
                CreatedAt = DateTime.Now,
            };
        }).ToList();

        await _musicRepository.SaveRangeAsync(musics);
        await _unitOfWork.CommitAsync();
    }

    public async Task UpdateMusicAsync(Music editMusic)
    {
        Music? music = await _musicRepository.GetByIdWithDataAsync(editMusic.Id)
            ?? throw new InvalidOperationException("Music NotFound.");

        Album? album = await _albumRepository.GetByTitleAsync(editMusic.Album.Title)
            ?? throw new InvalidOperationException("Album NotFound.");

        Artist? artist = await _artistRepository.GetByNameAsync(editMusic.Artist.Name)
            ?? throw new InvalidOperationException("Artist NotFound.");

        music.Title = editMusic.Title;
        music.Duration = new TimeSpan(0, editMusic.Duration.Hours, editMusic.Duration.Minutes);
        music.AlbumId = album.Id;
        music.ArtistId = artist.Id;
        music.UpdatedAt = DateTime.Now;

        _musicRepository.Update(music);
        await _unitOfWork.CommitAsync();

        await Task.CompletedTask;
    }

    public async Task<PagedResult<MusicDto>> GetPaginatedMusicsAsync(int pageNumber, int pageSize, string searchTerm = "")
    {
        var (musics, totalItems) = await _musicRepository.GetPaginatedAsync(pageNumber, pageSize, searchTerm);

        var musicsDTOs = musics.Select(a => new MusicDto
        {
            Id = a.Id,
            Title = a.Title,
            Duration = a.Duration,
            AlbumId = a.AlbumId ?? Guid.Empty,
            AlbumTitle = a.Album.Title,
            ArtistId = a.ArtistId ?? Guid.Empty,
            ArtistName = a.Artist.Name
        }).ToList();

        return new PagedResult<MusicDto>
        {
            Items = musicsDTOs,
            TotalItems = totalItems,
            PageNumber = pageNumber,
            PageSize = pageSize
        };
    }
}
