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
        var normalizedDtos = NormalizeMusicDtos(musicDtos);

        var artistDict = await EnsureArtistsExistAsync(normalizedDtos.Select(m => m.ArtistName!).Distinct());
        var albumDict = await EnsureAlbumsExistAsync(normalizedDtos, artistDict);

        var musics = normalizedDtos.Select(dto =>
        {
            var artist = artistDict[dto.ArtistName!];
            var album = albumDict[dto.AlbumTitle!];

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

    public async Task UpdateMusicAsync(MusicDto editMusic)
    {
        Music? music = await _musicRepository.GetMusicByIdAsync(editMusic.Id!.Value)
            ?? throw new InvalidOperationException("Music NotFound.");

        Album? album = await _albumRepository.GetByTitleAsync(editMusic.AlbumTitle!)
            ?? throw new InvalidOperationException("Album NotFound.");

        Artist? artist = await _artistRepository.GetByNameAsync(editMusic.ArtistName!)
            ?? throw new InvalidOperationException("Artist NotFound.");

        music.Update(
            editMusic.Title,
            new TimeSpan(0, editMusic.Duration.Hours, editMusic.Duration.Minutes),
            album.Id,
            artist.Id
        );

        _musicRepository.Update(music);
        await _unitOfWork.CommitAsync();
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

    // Private Methods
    private static IEnumerable<MusicDto> NormalizeMusicDtos(IEnumerable<MusicDto> musicDtos)
    {
        return musicDtos.Select(dto =>
        {
            dto.ArtistName = string.IsNullOrWhiteSpace(dto.ArtistName) ? "Desconhecido" : dto.ArtistName!;
            dto.AlbumTitle = string.IsNullOrWhiteSpace(dto.AlbumTitle) ? "Sem Álbum" : dto.AlbumTitle!;
            return dto;
        });
    }

    private async Task<Dictionary<string, Artist>> EnsureArtistsExistAsync(IEnumerable<string> artistNames)
    {
        var artistDict = await _artistRepository.GetDictByNamesAsync(artistNames);

        var newArtists = artistNames
            .Where(name => !artistDict.ContainsKey(name))
            .Select(name => new Artist(name));

        if (newArtists.Any())
        {
            await _artistRepository.SaveRangeAsync(newArtists);
            await _unitOfWork.CommitAsync();

            foreach (var artist in newArtists)
                artistDict[artist.Name] = artist;
        }

        return artistDict;
    }

    private async Task<Dictionary<string, Album>> EnsureAlbumsExistAsync(
        IEnumerable<MusicDto> musicDtos,
        Dictionary<string, Artist> artistDict)
    {
        var albumTitles = musicDtos.Select(m => m.AlbumTitle!).Distinct();
        var albumDict = await _albumRepository.GetDictByTitlesAsync(albumTitles);

        var newAlbums = albumTitles
            .Where(title => !albumDict.ContainsKey(title))
            .Select(title =>
            {
                var sample = musicDtos.First(m => m.AlbumTitle == title);
                var artist = artistDict[sample.ArtistName!];
                return new Album(title, DateTime.Now, artist.Id);
            });

        if (newAlbums.Any())
        {
            await _albumRepository.SaveRangeAsync(newAlbums);
            await _unitOfWork.CommitAsync();

            foreach (var album in newAlbums)
                albumDict[album.Title] = album;
        }

        return albumDict;
    }
}
