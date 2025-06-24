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
    public IEnumerable<MusicDto>? Musics { get; set; }

    public PlaylistDto() { }

    public PlaylistDto
    (
        Guid? id,
        string name,
        IEnumerable<MusicDto>? musics
    )
    {
        Id = id;
        Name = name;
        Musics = musics ?? new List<MusicDto>();
    }
}
