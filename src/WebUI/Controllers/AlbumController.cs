using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using Application.Services;

namespace WebUI.Controllers;

public class AlbumController : Controller
{
    private readonly IAlbumService _albumService;
    private readonly IArtistService _artistService;

    public AlbumController(IAlbumService albumService, IArtistService artistService)
    {
        _albumService = albumService;
        _artistService = artistService;
    }

    public IActionResult Index() =>
        View(_albumService.GetAllWithArtistAsync().Result);

    public async Task<IActionResult> Create()
    {
        var artists = await _artistService.GetAllAsync();
        ViewBag.ArtistNames = artists.Select(a => a.Name).ToList();
        return View();
    }

[HttpPost]
public async Task<IActionResult> Create(AlbumDto album)
{
    if (!ModelState.IsValid)
    {
        var artists = await _artistService.GetAllAsync();
        ViewBag.ArtistNames = artists.Select(a => a.Name).ToList();
        return View(album);
    }

    try
    {
        await _albumService.AddAlbumAsync(album);
        TempData["MessageSuccess"] = "Álbum cadastrado com sucesso.";
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
        View(_albumService.GetByIdWithArtistAsync(id).Result);

    [HttpPost]
    public IActionResult Edit(Album album)
    {
        if (!ModelState.IsValid) return View(album);
        try
        {
            _albumService.UpdateAlbumAsync(album);
            TempData["MessageSuccess"] = "Álbum atualizado com sucesso.";
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Atualização: {error.Message}";
        }
        return RedirectToAction("Index");
    }

    public IActionResult DeleteConfirm(Guid id) =>
        View(_albumService.GetByIdAsync(id).Result);

    public IActionResult Delete(Guid id)
    {
        try
        {
            if (_albumService.DeleteAlbumAsync(id).IsCompleted)
                TempData["MessageSuccess"] = "Contato deletado com sucesso.";
            else
                TempData["MessageError"] = "Erro no processo de Exclusão.";

            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Exclusão: {error.Message}";
            return RedirectToAction("Index");
        }
    }
}