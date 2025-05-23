using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace WebUI.Controllers;

public class ArtistController : Controller
{
    private readonly IArtistService _artistService;

    public ArtistController(IArtistService artistService) =>
        _artistService = artistService;

    public IActionResult Index() =>
        View(_artistService.GetAllWithDataAsync().Result);

    public IActionResult Create() =>
        View();

    [HttpPost]
    public IActionResult Create(IEnumerable<ArtistDto> artists)
    {
        if (!ModelState.IsValid) return View(artists);

        try
        {
            _artistService.AddManyArtistsAsync(artists);
            TempData["MessageSuccess"] = "Artistas cadastrados com sucesso.";
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Cadastro: {error.Message}";
            return RedirectToAction("Index");
        }
    }

    public IActionResult Edit(Guid id) =>
        View(_artistService.GetByIdAsync(id).Result);

    [HttpPost]
    public IActionResult Edit(Artist artist)
    {
        if (!ModelState.IsValid) return View(artist);

        try
        {
            _artistService.UpdateArtistAsync(artist);
            TempData["MessageSuccess"] = "Artista atualizado com sucesso.";
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Atualização: {error.Message}";
            return RedirectToAction("Index");
        }
    }

    public IActionResult DeleteConfirm(Guid id) =>
        View(_artistService.GetByIdAsync(id).Result);

    public IActionResult Delete(Guid id)
    {
        try
        {
            if (_artistService.DeleteAsync(id).IsCompleted)
                TempData["MessageSuccess"] = "Artista deletado com sucesso.";
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