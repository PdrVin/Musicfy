using Microsoft.EntityFrameworkCore;

namespace Infra.Context;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    { }

    // DbSets para entidades serão adicionados aqui no futuro

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurações adicionais podem ser adicionadas aqui
    }
}