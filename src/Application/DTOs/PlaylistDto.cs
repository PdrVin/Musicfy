using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class PlaylistDto
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "O nome da playlist é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome da playlist deve ter no máximo 50 caracteres.")]
    [Display(Name = "Nome")]
    public string Name { get; set; }

    [Display(Name = "Músicas")]
    public List<MusicInPlaylistDto>? Musics { get; set; }
}

public class MusicInPlaylistDto
{
    public Guid? Id { get; set; }

    [Display(Name = "Título")]
    public string Title { get; set; }

    [Display(Name = "Duração")]
    public TimeSpan Duration { get; set; }

    [Display(Name = "Álbum")]
    public string AlbumTitle { get; set; }
    public Guid AlbumId { get; set; }

    [Display(Name = "Artista")]
    public string ArtistName { get; set; }
    public Guid ArtistId { get; set; }
}
