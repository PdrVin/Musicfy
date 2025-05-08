using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings;

public class AlbumMap : IEntityTypeConfiguration<Album>
{
    public void Configure(EntityTypeBuilder<Album> builder)
    {
        builder.ToTable("Albums");

        builder.HasKey(al => al.Id);

        builder.Property(al => al.Id)
            .IsRequired();

        builder.Property(al => al.Title)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(al => al.ReleaseDate)
            .IsRequired();

        builder.HasOne(al => al.Artist)
            .WithMany(ar => ar.Albums)
            .HasForeignKey(al => al.ArtistId);

        builder.HasMany(al => al.Musics)
            .WithOne(mu => mu.Album)
            .HasForeignKey(mu => mu.AlbumId);
    }
}
