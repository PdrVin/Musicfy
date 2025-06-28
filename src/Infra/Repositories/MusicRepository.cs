using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class MusicRepository : Repository<Music>, IMusicRepository
{
    public MusicRepository(AppDbContext context) : base(context)
    { }

    public async Task<IEnumerable<Music>> GetAllMusicsAsync()
    {
        return await Entities
            .Include(m => m.Album)
            .Include(m => m.Artist)
            .OrderBy(m => m.Artist.Name)
            .ThenBy(m => m.Album.Title)
            .ThenBy(m => m.Title)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Music?> GetMusicByIdAsync(Guid id)
    {
        return await Entities
            .Include(m => m.Album)
            .Include(m => m.Artist)
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<List<Music>> GetManyByIdsAsync(List<Guid> ids)
    {
        return await Entities
            .Where(m => ids.Contains(m.Id))
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<(IEnumerable<Music> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(a => a.Album)
            .Include(a => a.Artist)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(a =>
                a.Title.Contains(searchTerm) ||
                a.Album.Title.Contains(searchTerm) ||
                a.Artist.Name.Contains(searchTerm)
            );
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderBy(a => a.Title)
            .ThenBy(a => a.Album.Title)
            .ThenBy(a => a.Artist.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
