using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings;

public class MusicMap : IEntityTypeConfiguration<Music>
{
    public void Configure(EntityTypeBuilder<Music> builder)
    {
        builder.ToTable("Musics");

        builder.HasKey(mu => mu.Id);

        builder.Property(mu => mu.Id)
            .IsRequired();

        builder.Property(mu => mu.Title)
            .IsRequired()
            .HasMaxLength(40);

        builder.Property(mu => mu.Duration)
            .IsRequired();

        builder.HasOne(mu => mu.Artist)
            .WithMany(ar => ar.Musics)
            .HasForeignKey(mu => mu.ArtistId)
            .IsRequired();

        builder.HasOne(mu => mu.Album)
            .WithMany(al => al.Musics)
            .HasForeignKey(mu => mu.AlbumId)
            .IsRequired();

        builder.HasMany(mu => mu.Playlists)
            .WithMany(pl => pl.Musics);
    }
}