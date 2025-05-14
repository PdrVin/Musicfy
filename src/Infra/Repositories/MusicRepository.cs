using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Domain.Interfaces;
using Infra.Context;
using Infra.Repositories.Base;

namespace Infra.Repositories;

public class MusicRepository : Repository<Music>, IMusicRepository
{
    private readonly AppDbContext _context;

    public MusicRepository(AppDbContext context) : base(context) =>
        _context = context;

    public async Task<IEnumerable<Music>> GetAllWithDataAsync()
    {
        return await _context.Musics
            .Include(m => m.Album)
            .Include(m => m.Artist)
            .OrderBy(m => m.Artist.Name)
            .ThenBy(m => m.Album.Title)
            .ThenBy(m => m.Title)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Music?> GetByIdWithDataAsync(Guid id)
    {
        return await _context.Musics
            .Include(m => m.Album)
            .Include(m => m.Artist)
            .FirstOrDefaultAsync(m => m.Id == id);
    }
}
