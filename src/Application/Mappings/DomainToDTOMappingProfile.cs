using Application.DTOs;
using Domain.Entities;
using AutoMapper;

namespace Application.Mappings;
public class DomainToDTOMappingProfile
    : Profile
{
    public DomainToDTOMappingProfile()
    {
        CreateMap<Album, AlbumDto>().ReverseMap();
        CreateMap<Artist, ArtistDto>().ReverseMap();
        CreateMap<Music, MusicDto>().ReverseMap();
        CreateMap<Playlist, PlaylistDto>().ReverseMap();
    }
}
