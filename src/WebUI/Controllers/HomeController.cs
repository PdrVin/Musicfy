using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using WebUI.Models;

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
        var model = new DashboardViewModel
        {
            TotalArtists = await _artistService.CountAsync(),
            TotalAlbums = await _albumService.CountAsync(),
            TotalMusics = await _musicService.CountAsync(),
            TotalPlaylists = await _playlistService.CountAsync(),
            TopArtistsByMusicCount = await _artistService.GetTopArtistsByMusicAsync(10)
        };

        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error() =>
        View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
