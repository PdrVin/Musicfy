using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Domain.Entities;
using WebUI.Models;
using WebUI.ViewModels.Home;

namespace WebUI.Controllers;

public class HomeController : Controller
{
    private readonly IArtistService _artistService;
    private readonly IAlbumService _albumService;
    private readonly IMusicService _musicService;
    private readonly IPlaylistService _playlistService;

    public HomeController(
        IArtistService artistService,
        IAlbumService albumService,
        IMusicService musicService,
        IPlaylistService playlistService)
    {
        _artistService = artistService;
        _albumService = albumService;
        _musicService = musicService;
        _playlistService = playlistService;
    }

    public async Task<IActionResult> Index()
    {
        IEnumerable<Artist> artists = await _artistService.GetAllArtistsAsync();
        IEnumerable<Album> albums = await _albumService.GetAllAlbumsAsync();
        IEnumerable<Music> musics = await _musicService.GetAllMusicsAsync();
        IEnumerable<Playlist> playlists = await _playlistService.GetAllPlaylistsAsync();

        IEnumerable<Artist> topArtistsByMusic = await _artistService.GetTopArtistsByMusicAsync(10);
        IEnumerable<Artist> topArtistsByAlbum = await _artistService.GetTopArtistsByAlbumAsync(10);

        DashboardViewModel model = new()
        {
            TotalArtists = artists.Count(),
            TotalAlbums = albums.Count(),
            TotalMusics = musics.Count(),
            TotalPlaylists = playlists.Count(),
            TopArtistsByMusicCount = topArtistsByMusic.ToList()
        };

        // Donut Chart - Musics by Artist
        var musicsByArtist = topArtistsByMusic
            .ToDictionary(
                a => a.Name,
                a => a.Musics.Count
            );

        var otherMusicCount = artists
            .Where(a => !topArtistsByMusic.Any(t => t.Id == a.Id))
            .Sum(a => a.Musics.Count > 0 ? a.Musics.Count : 0);

        if (otherMusicCount > 0)
            musicsByArtist.Add("Outros", otherMusicCount);

        ViewBag.MusicLabels = musicsByArtist.Keys;
        ViewBag.MusicValues = musicsByArtist.Values;

        // Bar Chart - Albums by Artist
        var albumByArtist = topArtistsByAlbum
            .ToDictionary(
                a => a.Name,
                a => a.Albums?.Count ?? 0
            );

        var otherAlbumCount = artists
            .Where(a => !topArtistsByAlbum.Any(t => t.Id == a.Id))
            .Sum(a => a.Musics.Count > 0 ? a.Musics.Count : 0);

        if (otherAlbumCount > 0)
            albumByArtist.Add("Outros", otherAlbumCount);

        ViewBag.AlbumLabels = albumByArtist.Keys;
        ViewBag.AlbumValues = albumByArtist.Values;

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
