using Application.DTOs;

namespace WebUI.ViewModels.Album;

public class AlbumListViewModel
{
    public IEnumerable<AlbumDto> Albums { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public string SearchTerm { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}