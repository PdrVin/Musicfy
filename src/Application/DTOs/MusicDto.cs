using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class MusicDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O título da música é obrigatório.")]
    [StringLength(50, ErrorMessage = "O título da música deve ter no máximo 50 caracteres.")]
    [Display(Name = "Título")]
    public string Title { get; set; }

    [Required(ErrorMessage = "A duração da música é obrigatória.")]
    [Display(Name = "Duração")]
    public TimeSpan Duration { get; set; }

    [Required(ErrorMessage = "O álbum da música é obrigatório.")]
    public Guid AlbumId { get; set; }

    [StringLength(50, ErrorMessage = "O nome do álbum deve ter no máximo 50 caracteres.")]
    [Display(Name = "Álbum")]
    public string AlbumTitle { get; set; }

    [Required(ErrorMessage = "O artista da música é obrigatório.")]
    public Guid ArtistId { get; set; }

    [StringLength(50, ErrorMessage = "O nome do artista deve ter no máximo 50 caracteres.")]
    [Display(Name = "Artista")]
    public string ArtistName { get; set; }
}
