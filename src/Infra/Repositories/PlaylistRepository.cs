using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class PlaylistRepository : Repository<Playlist>, IPlaylistRepository
{
    private readonly AppDbContext _context;

    public PlaylistRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<IEnumerable<Playlist>> GetAllWithDataAsync()
    {
        return await _context.Playlists
            .Include(p => p.Musics)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Playlist?> GetByIdWithDataAsync(Guid id)
    {
        return await _context.Playlists
            .Include(p => p.Musics)
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<(IEnumerable<Playlist> Items, int TotalCount)> GetPaginatedAsync(
        int pageNumber,
        int pageSize,
        string searchTerm = ""
    )
    {
        var query = Entities
            .Include(a => a.Musics)
            .AsQueryable();

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(a =>
                a.Name.Contains(searchTerm)
            );
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .OrderByDescending(a => a.Name)
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return (items, totalCount);
    }
}
