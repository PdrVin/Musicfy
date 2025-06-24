using Microsoft.AspNetCore.Mvc;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using WebUI.ViewModels.Artist;
using AutoMapper;

namespace WebUI.Controllers;

public class ArtistController : Controller
{
    private readonly IArtistService _artistService;
    private readonly IMapper _mapper;

    public ArtistController(IArtistService artistService, IMapper mapper)
    {
        _artistService = artistService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20, string searchTerm = "")
    {
        var paginatedArtists = await _artistService.GetPaginatedArtistsAsync(pageNumber, pageSize, searchTerm);

        return View(new ArtistListViewModel
        {
            Artists = paginatedArtists.Items,
            PageNumber = paginatedArtists.PageNumber,
            PageSize = paginatedArtists.PageSize,
            TotalItems = paginatedArtists.TotalItems,
            SearchTerm = searchTerm
        });
    }

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

    public async Task<IActionResult> Edit(Guid id)
    {
        var artist = await _artistService.GetArtistByIdAsync(id);
        if (artist == null) return NotFound();

        var artistDto = _mapper.Map<ArtistDto>(artist);

        return View(artistDto);
    }

    [HttpPost]
    public IActionResult Edit(ArtistDto artist)
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

    public IActionResult Details() =>
        View();

    [HttpGet]
    public async Task<IActionResult> Details(Guid id)
    {
        var artist = await _artistService.GetArtistByIdAsync(id);

        if (artist == null) return NotFound();

        var viewModel = _mapper.Map<ArtistDto>(artist);

        return View(viewModel);
    }
}