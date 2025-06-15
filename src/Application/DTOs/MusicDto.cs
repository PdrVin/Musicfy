using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class MusicDto
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "O título da música é obrigatório.")]
    [StringLength(50, ErrorMessage = "O título da música deve ter no máximo 50 caracteres.")]
    [Display(Name = "Título")]
    public string Title { get; set; }

    [Required(ErrorMessage = "A duração da música é obrigatória.")]
    [Display(Name = "Duração")]
    public TimeSpan Duration { get; set; }

    public Guid? AlbumId { get; set; }

    [StringLength(50, ErrorMessage = "O nome do álbum deve ter no máximo 50 caracteres.")]
    [Display(Name = "Álbum")]
    public string? AlbumTitle { get; set; }

    public Guid? ArtistId { get; set; }

    [StringLength(50, ErrorMessage = "O nome do artista deve ter no máximo 50 caracteres.")]
    [Display(Name = "Artista")]
    public string? ArtistName { get; set; }

    public MusicDto() { }

    public MusicDto
    (
        Guid? id,
        string title,
        TimeSpan duration,
        Guid? albumId = null,
        string? albumTitle = null,
        Guid? artistId = null,
        string? artistName = null
    )
    {
        Id = id;
        Title = title;
        Duration = duration;
        AlbumId = albumId;
        AlbumTitle = albumTitle;
        ArtistId = artistId;
        ArtistName = artistName;
    }
}
