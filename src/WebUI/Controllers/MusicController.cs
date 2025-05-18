using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;

namespace WebUI.Controllers;

public class MusicController : Controller
{
    private readonly IMusicService _musicService;

    public MusicController(IMusicService musicService) =>
        _musicService = musicService;

    public IActionResult Index() =>
        View(_musicService.GetAllWithDataAsync().Result);

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
            return RedirectToAction("Index", "Music");
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
        View(_musicService.GetByIdWithDataAsync(id).Result);

    [HttpPost]
    public IActionResult Edit(Music music)
    {
        if (!ModelState.IsValid) return View(music);

        try
        {
            _musicService.UpdateMusicAsync(music);
            TempData["MessageSuccess"] = "Música atualizada com sucesso.";
            return RedirectToAction("Index");
        }
        catch (Exception error)
        {
            TempData["MessageError"] = $"Erro no processo de Atualização: {error.Message}";
            return RedirectToAction("Index");
        }
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
}