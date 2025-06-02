using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AlbumRepository : Repository<Album>, IAlbumRepository
{
    public AlbumRepository(AppDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Album>> GetAllAlbumsAsync()
    {
        return await Entities
            .Include(a => a.Artist)
            .Include(a => a.Musics)
            .OrderBy(a => a.Artist.Name)
            .ThenBy(a => a.Title)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Album?> GetAlbumByIdAsync(Guid id)
    {
        return await Entities
            .Include(a => a.Artist)
            .Include(a => a.Musics)
            .AsNoTracking()
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Album?> GetByTitleAsync(string title)
    {
        return await Entities
            .FirstOrDefaultAsync(a => a.Title == title);
    }

    public async Task<Dictionary<string, Album>> GetDictByTitlesAsync(IEnumerable<string> titles)
    {
        return await Entities
            .Where(a => titles.Contains(a.Title))
            .Distinct()
            .AsNoTracking()
            .ToDictionaryAsync(a => a.Title, StringComparer.OrdinalIgnoreCase);
    }

    public async Task<(IEnumerable<Album> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(a => a.Artist)
            .Include(a => a.Musics)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(a =>
                a.Title.Contains(searchTerm) ||
                a.Artist.Name.Contains(searchTerm)
            );
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(a => a.Title)
            .ThenBy(a => a.Artist.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
