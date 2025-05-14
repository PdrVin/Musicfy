using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace WebUI.Controllers;

public class AlbumController : Controller
{
    private readonly IAlbumService _albumService;

    public AlbumController(IAlbumService albumService) =>
        _albumService = albumService;

    public IActionResult Index() =>
        View(_albumService.GetAllWithArtistAsync().Result);

    public IActionResult Create() =>
        View();

    [HttpPost]
    public IActionResult Create(AlbumDto album)
    {
        if (!ModelState.IsValid) return View(album);

        try
        {
            _albumService.AddAlbumAsync(album);
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
            if (_albumService.DeleteAsync(id).IsCompleted)
                TempData["MessageSuccess"] = "Contato deletado com sucesso.";
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