using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace WebUI.Controllers;

public class PlaylistController : Controller
{
    private readonly IPlaylistService _playlistService;

    public PlaylistController(IPlaylistService playlistService) =>
        _playlistService = playlistService;

    public IActionResult Index() =>
        View(_playlistService.GetAllAsync().Result);

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
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Cadastro: {error.Message}";
            return RedirectToAction("Index");
        }
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
}