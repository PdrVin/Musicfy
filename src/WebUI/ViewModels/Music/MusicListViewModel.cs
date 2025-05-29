using Application.DTOs;

namespace WebUI.ViewModels.Music;

public class MusicListViewModel
{
    public IEnumerable<MusicDto> Musics { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public string SearchTerm { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}