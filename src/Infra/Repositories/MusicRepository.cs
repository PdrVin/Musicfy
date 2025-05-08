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

    public async Task<IEnumerable<Music>> GetMusicsWithAlbumAndArtistAsync()
    {
        return await _context.Musics
            .Include(m => m.Album)
            .Include(m => m.Artist)
            .AsNoTracking()
            .ToListAsync();
    }
}
