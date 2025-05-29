using Application.DTOs;

namespace WebUI.ViewModels.Playlist;

public class PlaylistListViewModel
{
    public IEnumerable<PlaylistDto> Playlists { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public string SearchTerm { get; set; }
    public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
}