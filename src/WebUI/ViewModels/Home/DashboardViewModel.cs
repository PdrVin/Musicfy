namespace WebUI.ViewModels.Home;

public class DashboardViewModel
{
    public int TotalMusics { get; set; }
    public int TotalAlbums { get; set; }
    public int TotalArtists { get; set; }
    public int TotalPlaylists { get; set; }

    public List<Domain.Entities.Artist> TopArtistsByMusicCount { get; set; } = [];
}