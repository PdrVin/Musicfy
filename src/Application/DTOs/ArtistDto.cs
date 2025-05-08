using System;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs;

public class ArtistDto
{
    public Guid Id { get; set; }

    [Required(ErrorMessage = "O nome do artista é obrigatório.")]
    [StringLength(50, ErrorMessage = "O nome do artista deve ter no máximo 50 caracteres.")]
    [Display(Name = "Nome")]
    public string Name { get; set; }
}
