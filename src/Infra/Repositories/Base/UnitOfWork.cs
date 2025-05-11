using Domain.Interfaces.Base;
using Infra.Context;

namespace Infra.Repositories.Base;
public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
        => _context = context;

    public async Task CommitAsync()
    {
        try
        {
            await _context.SaveChangesAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error committing transaction", ex);
        }
    }
}