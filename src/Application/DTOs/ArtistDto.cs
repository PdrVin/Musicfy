using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class ArtistDto
{
    public Guid? Id { get; set; }

    [Required(ErrorMessage = "O nome do artista é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome do artista deve ter no máximo 50 caracteres.")]
    [Display(Name = "Nome do Artista")]
    public string Name { get; set; }

    public IEnumerable<AlbumDto>? Albums { get; set; }
    public IEnumerable<MusicDto>? Musics { get; set; }

    public ArtistDto() { }

    public ArtistDto
    (
        Guid? id,
        string name,
        IEnumerable<AlbumDto>? albumDtos,
        IEnumerable<MusicDto>? musicDtos
    )
    {
        Id = id;
        Name = name;
        Albums = albumDtos ?? new List<AlbumDto>();
        Musics = musicDtos ?? new List<MusicDto>();
    }
}
