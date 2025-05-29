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
        IEnumerable<Artist> artists = await _artistService.GetAllWithDataAsync();
        IEnumerable<Album> albums = await _albumService.GetAllWithDataAsync();
        IEnumerable<Music> musics = await _musicService.GetAllWithDataAsync();
        IEnumerable<Playlist> playlists = await _playlistService.GetAllWithDataAsync();
        IEnumerable<Artist> topArtistsByMusic = await _artistService.GetTopArtistsByMusicAsync(10);

        DashboardViewModel model = new()
        {
            TotalArtists = artists.Count(),
            TotalAlbums = albums.Count(),
            TotalMusics = musics.Count(),
            TotalPlaylists = playlists.Count(),
            TopArtistsByMusicCount = topArtistsByMusic.ToList()
        };

        // var topArtistsByMusic2 = _artistService.GetTopArtistsByMusicAsync(20).Result;

        var topArtistMusicCounts = topArtistsByMusic
            .ToDictionary(
                a => a.Name,
                a => a.Musics.Count
            );

        var otherArtists = artists
            .Where(a => !topArtistsByMusic.Any(t => t.Id == a.Id));

        var otherCount = otherArtists.Sum(a => a.Musics.Count > 0 ? a.Musics.Count : 0);

        if (otherCount > 0)
            topArtistMusicCounts.Add("Outros", otherCount);

        ViewBag.Labels = topArtistMusicCounts.Keys;
        ViewBag.Values = topArtistMusicCounts.Values;

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
