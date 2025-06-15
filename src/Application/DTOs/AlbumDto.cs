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

    public Guid? ArtistId { get; set; }

    [StringLength(50, ErrorMessage = "O nome do artista deve ter no máximo 50 caracteres.")]
    [Display(Name = "Nome do Artista")]
    public string? ArtistName { get; set; }

    public IEnumerable<MusicDto>? Musics { get; set; }

    public AlbumDto() { }

    public AlbumDto
    (
        Guid? id,
        string title,
        DateTime releaseDate,
        Guid? artistId,
        string? artistName,
        IEnumerable<MusicDto>? musicDtos = null
    )
    {
        Id = id;
        Title = title;
        ReleaseDate = releaseDate;
        ArtistId = artistId ?? null;
        ArtistName = artistName ?? null;
        Musics = musicDtos ?? new List<MusicDto>();
    }
}
