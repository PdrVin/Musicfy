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

    public async Task<IEnumerable<Album>> GetAllWithArtistAsync()
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .OrderBy(a => a.Artist.Name)
            .ThenBy(a => a.Title)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<Album?> GetByIdWithArtistAsync(Guid id)
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
}
