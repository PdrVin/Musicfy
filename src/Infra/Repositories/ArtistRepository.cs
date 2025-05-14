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
        return await _context.Artists.FirstOrDefaultAsync(a => a.Name == name);
    }
}
