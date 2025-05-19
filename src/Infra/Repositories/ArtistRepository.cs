using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class ArtistRepository : Repository<Artist>, IArtistRepository
{
    private readonly AppDbContext _context;

    public ArtistRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<IEnumerable<Artist>> GetAllWithDataAsync()
    {
        return await _context.Artists
            .Include(a => a.Albums)
            .Include(a => a.Musics)
            .OrderBy(a => a.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Artist?> GetByNameAsync(string name)
    {
        return await _context.Artists
            .Include(a => a.Albums)
            .Include(a => a.Musics)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Name == name);
    }

    public async Task<List<Artist>> GetByNamesAsync(IEnumerable<string> names)
    {
        return await _context.Artists
            .Where(a => names.Contains(a.Name))
            .Distinct()
            .ToListAsync();
    }

    public async Task<List<Artist>> GetTopArtistsByMusicAsync(int top)
    {
        return await _context.Artists
            .Include(a => a.Musics)
            .OrderByDescending(a => a.Musics.Count)
            .ThenByDescending(a => a.Albums.Count)
            .Take(top)
            .ToListAsync();
    }
}
