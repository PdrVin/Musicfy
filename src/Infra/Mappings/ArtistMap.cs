using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infra.Mappings;

public class ArtistMap : IEntityTypeConfiguration<Artist>
{
    public void Configure(EntityTypeBuilder<Artist> builder)
    {
        builder.ToTable("Artists");

        builder.HasKey(ar => ar.Id);

        builder.Property(ar => ar.Name)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasIndex(ar => ar.Name)
            .IsUnique();

        builder.HasMany(ar => ar.Albums)
            .WithOne(al => al.Artist)
            .HasForeignKey(al => al.ArtistId);

        builder.HasMany(ar => ar.Musics)
            .WithOne(mu => mu.Artist)
            .HasForeignKey(mu => mu.ArtistId);
    }
}
