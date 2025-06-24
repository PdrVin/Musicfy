using Microsoft.AspNetCore.Mvc;
using Application.Helpers.Pagination;
using Application.Interfaces;
using Application.DTOs;
using Domain.Entities;
using WebUI.ViewModels.Album;

using AutoMapper;

namespace WebUI.Controllers;

public class AlbumController : Controller
{
    private readonly IAlbumService _albumService;
    private readonly IMapper _mapper;

    public AlbumController(IAlbumService albumService, IMapper mapper)
    {
        _albumService = albumService;
        _mapper = mapper;
    }

    [HttpGet]
    public async Task<IActionResult> Index(int pageNumber = 1, int pageSize = 20, string searchTerm = "")
    {
        var paginatedAlbums = await _albumService.GetPaginatedAlbumsAsync(pageNumber, pageSize, searchTerm);

        return View(new AlbumListViewModel
        {
            Albums = paginatedAlbums.Items,
            PageNumber = paginatedAlbums.PageNumber,
            PageSize = paginatedAlbums.PageSize,
            TotalItems = paginatedAlbums.TotalItems,
            SearchTerm = searchTerm
        });
    }

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

    public async Task<IActionResult> Edit(Guid id)
    {
        var album = await _albumService.GetAlbumByIdAsync(id);
        if (album == null) return NotFound();

        var albumDto = _mapper.Map<AlbumDto>(album);

        return View(albumDto);
    }

    [HttpPost]
    public IActionResult Edit(AlbumDto album)
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
                TempData["MessageSuccess"] = "Álbum deletado com sucesso.";
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
        var album = await _albumService.GetAlbumByIdAsync(id);

        if (album == null) return NotFound();

        var viewModel = _mapper.Map<AlbumDto>(album);

        return View(viewModel);
    }
}