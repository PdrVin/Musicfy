using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class AlbumDto
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "O título do álbum é obrigatório.")]
    [StringLength(50, ErrorMessage = "O título do álbum deve ter no máximo 50 caracteres.")]
    [Display(Name = "Título")]
    public string Title { get; set; }

    [Required(ErrorMessage = "A data de lançamento do álbum é obrigatória.")]
    [Display(Name = "Data de Lançamento")]
    public DateTime ReleaseDate { get; set; }

    [Required(ErrorMessage = "O artista do álbum é obrigatório.")]
    public Guid ArtistId { get; set; }

    [StringLength(50, ErrorMessage = "O nome do artista deve ter no máximo 50 caracteres.")]
    [Display(Name = "Nome do Artista")]
    public string ArtistName { get; set; }

    public IEnumerable<MusicDto>? Musics { get; set; }
}
