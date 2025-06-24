using AutoMapper;
using Application.DTOs;
using Domain.Entities;

namespace Application.Mappings;

public class EntityMappingProfile : Profile
{
    public EntityMappingProfile()
    {
        // Album ↔ AlbumDto
        CreateMap<Album, AlbumDto>()
            .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist != null ? src.Artist.Name : null))
            .ReverseMap()
            .ForMember(dest => dest.Artist, opt => opt.Ignore())
            .ForMember(dest => dest.Musics, opt => opt.Ignore());

        // Artist ↔ ArtistDto
        CreateMap<Artist, ArtistDto>()
            .ReverseMap()
            .ForMember(dest => dest.Albums, opt => opt.Ignore())
            .ForMember(dest => dest.Musics, opt => opt.Ignore());

        // Music ↔ MusicDto
        CreateMap<Music, MusicDto>()
            .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src => src.Artist != null ? src.Artist.Name : null))
            .ForMember(dest => dest.AlbumTitle, opt => opt.MapFrom(src => src.Album != null ? src.Album.Title : null))
            .ReverseMap()
            .ForMember(dest => dest.Artist, opt => opt.Ignore())
            .ForMember(dest => dest.Album, opt => opt.Ignore())
            .ForMember(dest => dest.Playlists, opt => opt.Ignore());

        // Playlist ↔ PlaylistDto
        // Playlist ↔ PlaylistDto
        CreateMap<Playlist, PlaylistDto>()
            .ForMember(dest => dest.Musics, opt => opt.MapFrom(src => src.Musics))
            .ReverseMap()
            .ForMember(dest => dest.Musics, opt => opt.Ignore());
    }
}
