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
        View(_artistService.GetAllArtistsWithDataAsync().Result);

    public IActionResult Create() =>
        View();

    [HttpPost]
    public IActionResult Create(ArtistDto artist)
    {
        if (!ModelState.IsValid) return View(artist);

        try
        {
            _artistService.AddArtistAsync(artist);
            TempData["MessageSuccess"] = "Artista cadastrado com sucesso.";
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Cadastro: {error.Message}";
        }
        return RedirectToAction("Index");
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
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Atualização: {error.Message}";
        }
        return RedirectToAction("Index");
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
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Exclusão: {error.Message}";
        }
        return RedirectToAction("Index");
    }
}