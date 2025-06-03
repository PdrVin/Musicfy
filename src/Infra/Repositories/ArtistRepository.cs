using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class ArtistRepository : Repository<Artist>, IArtistRepository
{
    public ArtistRepository(AppDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Artist>> GetAllArtistsAsync()
    {
        return await Entities
            .Include(a => a.Albums)
            .Include(a => a.Musics)
            .OrderBy(a => a.Name)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Artist?> GetByNameAsync(string name)
    {
        return await Entities
            .Include(a => a.Albums)
            .Include(a => a.Musics)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Name.ToLower() == name.ToLower());
    }

    public async Task<Dictionary<string, Artist>> GetDictByNamesAsync(IEnumerable<string> names)
    {
        return await Entities
            .Where(a => names.Contains(a.Name))
            .Distinct()
            .AsNoTracking()
            .ToDictionaryAsync(a => a.Name, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<IEnumerable<Artist>> GetTopArtistsByMusicAsync(int top)
    {
        return await Entities
            .Include(a => a.Musics)
            .OrderByDescending(a => a.Musics.Count)
            .ThenByDescending(a => a.Albums.Count)
            .Take(top)
            .ToListAsync();
    }

    public async Task<IEnumerable<Artist>> GetTopArtistsByAlbumAsync(int top)
    {
        return await Entities
            .Include(a => a.Albums)
            .OrderByDescending(a => a.Albums.Count)
            .ThenByDescending(a => a.Musics.Count)
            .Take(top)
            .ToListAsync();
    }

    public async Task<(IEnumerable<Artist> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(a => a.Albums)
            .Include(a => a.Musics)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(a => a.Name.Contains(searchTerm));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(a => a.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
