using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using WebUI.Models;

namespace WebUI.Controllers;

public class PlaylistController : Controller
{
    private readonly IMusicService _musicService;
    private readonly IPlaylistService _playlistService;

    public PlaylistController(
        IPlaylistService playlistService,
        IMusicService musicService
    )
    {
        _playlistService = playlistService;
        _musicService = musicService;
    }

    public IActionResult Index() =>
        View(_playlistService.GetAllWithDataAsync().Result);

    public IActionResult Create() =>
        View();

    [HttpPost]
    public IActionResult Create(PlaylistDto playlist)
    {
        if (!ModelState.IsValid) return View(playlist);

        try
        {
            _playlistService.AddPlaylistAsync(playlist);
            TempData["MessageSuccess"] = "Playlist cadastrada com sucesso.";
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Cadastro: {error.Message}";
        }
        return RedirectToAction("Index");
    }

    public IActionResult Edit(Guid id) =>
        View(_playlistService.GetByIdAsync(id).Result);

    [HttpPost]
    public IActionResult Edit(Playlist playlist)
    {
        if (!ModelState.IsValid) return View(playlist);

        try
        {
            _playlistService.UpdatePlaylistAsync(playlist);
            TempData["MessageSuccess"] = "Playlist atualizado com sucesso.";
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Atualização: {error.Message}";
        }
        return RedirectToAction("Index");
    }

    public IActionResult DeleteConfirm(Guid id) =>
        View(_playlistService.GetByIdAsync(id).Result);

    public IActionResult Delete(Guid id)
    {
        try
        {
            if (_playlistService.DeleteAsync(id).IsCompleted)
                TempData["MessageSuccess"] = "Playlist deletado com sucesso.";
            else
                TempData["MessageError"] = "Erro no processo de Exclusão.";
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Exclusão: {error.Message}";
        }
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> AddMusics(Guid id)
    {
        var playlist = await _playlistService.GetByIdAsync(id);
        var allMusics = await _musicService.GetAllWithDataAsync();

        var model = new MusicsToPlaylistViewModel
        {
            PlaylistId = id,
            PlaylistName = playlist.Name,
            AvailableMusics = allMusics.Select(m => new SelectListItem
            {
                Value = m.Id.ToString(),
                Text = $"{m.Title} - {m.Artist.Name} ({m.Duration:mm\\:ss})"
            }).ToList()
        };

        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> AddMusics(MusicsToPlaylistViewModel model)
    {
        await _playlistService.AddMusicsToPlaylistAsync(model.PlaylistId, model.SelectedMusicIds);

        return RedirectToAction("Index");
    }
}