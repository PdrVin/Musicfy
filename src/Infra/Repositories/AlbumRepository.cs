using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories;

public class AlbumRepository : Repository<Album>, IAlbumRepository
{
    private readonly AppDbContext _context;

    public AlbumRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<IEnumerable<Album>> GetAllWithDataAsync()
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Musics)
            .OrderBy(a => a.Artist.Name)
            .ThenBy(a => a.Title)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Album?> GetByIdWithDataAsync(Guid id)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .Include(a => a.Musics)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Album?> GetByTitleAsync(string title)
    {
        return await _context.Albums.FirstOrDefaultAsync(a => a.Title == title);
    }

    public async Task<List<Album>> GetByTitlesAsync(IEnumerable<string> titles)
    {
        return await _context.Albums
            .Where(a => titles.Contains(a.Title))
            .Distinct()
            .ToListAsync();
    }
}
