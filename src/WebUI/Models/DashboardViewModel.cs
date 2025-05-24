using Domain.Entities;

namespace WebUI.Models;

public class DashboardViewModel
{
    public int TotalMusics { get; set; }
    public int TotalAlbums { get; set; }
    public int TotalArtists { get; set; }
    public int TotalPlaylists { get; set; }

    public List<Artist> TopArtistsByMusicCount { get; set; } = [];
}