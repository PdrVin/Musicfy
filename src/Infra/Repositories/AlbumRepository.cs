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

    public async Task<IEnumerable<Album>> GetAlbumsWithArtistAsync()
    {
        return await _context.Albums
            .Include(a => a.Artist)
            .AsNoTracking()
            .ToListAsync();
    }
}
