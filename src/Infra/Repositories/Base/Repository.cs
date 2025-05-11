using Domain.Entities.Base;
using Domain.Interfaces.Base;
using Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Infra.Repositories.Base;
public class Repository<T>
    : IRepository<T> where T : class, IEntity
{
    private readonly AppDbContext _context;
    public DbSet<T> Entities { get; }

    public Repository(AppDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
        Entities = _context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await Entities.AsNoTracking().ToListAsync();
        }
        catch (Exception ex)
        {
            throw new Exception("Error fetching all entities", ex);
        }
    }

    public async Task<T> GetByIdAsync(Guid id)
    {
        try
        {
            var entity = await Entities.FindAsync(id) ??
                throw new KeyNotFoundException($"Entity with id {id} not found");

            return entity;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error fetching entity with id {id}", ex);
        }
    }

    public async Task SaveAsync(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            await Entities.AddAsync(entity);
        }
        catch (Exception ex)
        {
            throw new Exception("Error saving entity", ex);
        }
    }

    public void Update(T entity)
    {
        ArgumentNullException.ThrowIfNull(entity);

        try
        {
            _context.Update(entity);
        }
        catch (Exception ex)
        {
            throw new Exception("Error updating entity", ex);
        }
    }

    public void Delete(Guid id)
    {
        ArgumentNullException.ThrowIfNull(id);

        try
        {
            var entity = Entities.Find(id) ??
                throw new KeyNotFoundException($"Entity with id {id} not found");

            Entities.Remove(entity);
        }
        catch (Exception ex)
        {
            throw new Exception("Error deleting entity", ex);
        }
    }

    public void Dispose()
    {
        _context?.Dispose();
    }
}