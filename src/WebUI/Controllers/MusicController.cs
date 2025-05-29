using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebUI.ViewModels.Music;

namespace WebUI.Controllers;

public class MusicController : Controller
{
    private readonly IMusicService _musicService;
    private readonly IPlaylistService _playlistService;

    public MusicController(
        IMusicService musicService,
        IPlaylistService playlistService
    )
    {
        _musicService = musicService;
        _playlistService = playlistService;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 15, string searchTerm = "")
    {
        var paginatedMusics = await _musicService.GetPaginatedMusicsAsync(pageNumber, pageSize, searchTerm);

        var playlists = await _playlistService.GetAllAsync();

        ViewBag.Playlists = playlists.Select(p => new SelectListItem
        {
            Text = p.Name,
            Value = p.Id.ToString()
        }).ToList();

        return View(new MusicListViewModel
        {
            Musics = paginatedMusics.Items,
            PageNumber = paginatedMusics.PageNumber,
            PageSize = paginatedMusics.PageSize,
            TotalItems = paginatedMusics.TotalItems,
            SearchTerm = searchTerm
        });
    }

    public IActionResult Create(string? artistName, string? albumTitle)
    {
        if (artistName == null && albumTitle == null)
            return View();

        List<MusicDto> musics = [ new()
        {
            ArtistName = artistName!,
            AlbumTitle = albumTitle!
        }];

        return View(musics);
    }


    [HttpPost]
    public IActionResult Create(IEnumerable<MusicDto> musics)
    {
        if (!ModelState.IsValid) return View(musics);

        try
        {
            _musicService.AddManyMusicsAsync(musics);
            TempData["MessageSuccess"] = "Músicas cadastradas com sucesso.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["MessageError"] = $"Erro: {ex.Message}";
        }
        catch (Exception ex)
        {
            TempData["MessageError"] = $"Erro inesperado: {ex.Message}";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Edit(Guid id) =>
        View(_musicService.GetByIdWithDataAsync(id).Result);

    [HttpPost]
    public IActionResult Edit(Music music)
    {
        if (!ModelState.IsValid) return View(music);

        try
        {
            _musicService.UpdateMusicAsync(music);
            TempData["MessageSuccess"] = "Música atualizada com sucesso.";
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Atualização: {error.Message}";
        }
        return RedirectToAction("Index");
    }

    public IActionResult DeleteConfirm(Guid id) =>
        View(_musicService.GetByIdAsync(id).Result);

    public IActionResult Delete(Guid id)
    {
        try
        {
            if (_musicService.DeleteAsync(id).IsCompleted)
                TempData["MessageSuccess"] = "Música deletada com sucesso.";
            else
                TempData["MessageError"] = "Erro no processo de Exclusão.";
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Exclusão: {error.Message}";
        }
        return RedirectToAction("Index");
    }

    [HttpPost]
    public async Task<IActionResult> AddToPlaylist(Guid musicId, Guid playlistId)
    {
        try
        {
            await _playlistService.AddMusicToPlaylistAsync(musicId, playlistId);
            TempData["Mensagem"] = "Música adicionada à playlist com sucesso.";
        }
        catch (InvalidOperationException ex)
        {
            TempData["Mensagem"] = ex.Message;
        }
        catch (Exception)
        {
            TempData["Mensagem"] = "Erro ao adicionar música à playlist.";
        }

        return RedirectToAction("Index");
    }
}