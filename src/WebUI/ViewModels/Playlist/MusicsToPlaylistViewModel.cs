using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebUI.ViewModels.Playlist;

public class MusicsToPlaylistViewModel
{
    public Guid PlaylistId { get; set; }
    public string PlaylistName { get; set; }

    [Display(Name = "Selecione as Músicas")]
    public List<Guid> SelectedMusicIds { get; set; } = new();

    public List<SelectListItem> AvailableMusics { get; set; } = new();
}
