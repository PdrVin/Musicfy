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
        View(_albumService.GetAllWithDataAsync().Result);

    public IActionResult Create(string? artistName)
    {
        if (artistName == null) return View();

        List<AlbumDto> albums = [ new() {
            ArtistName = artistName!
        }];

        return View(albums);
    }

    [HttpPost]
    public IActionResult Create(IEnumerable<AlbumDto> albums)
    {
        if (!ModelState.IsValid) return View(albums);

        try
        {
            _albumService.AddManyAlbumsAsync(albums);
            TempData["MessageSuccess"] = "Álbums cadastrados com sucesso.";
            return RedirectToAction("Index");
        }
        catch (InvalidOperationException ex)
        {
            TempData["MessageError"] = $"Erro: {ex.Message}";
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            TempData["MessageError"] = $"Erro inesperado: {ex.Message}";
            return RedirectToAction("Index");
        }
    }

    public IActionResult Edit(Guid id) =>
        View(_albumService.GetByIdWithDataAsync(id).Result);

    [HttpPost]
    public IActionResult Edit(Album album)
    {
        if (!ModelState.IsValid) return View(album);

        try
        {
            _albumService.UpdateAlbumAsync(album);
            TempData["MessageSuccess"] = "Álbum atualizado com sucesso.";
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Atualização: {error.Message}";
            return RedirectToAction("Index");
        }
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

            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Exclusão: {error.Message}";
            return RedirectToAction("Index");
        }
    }
}