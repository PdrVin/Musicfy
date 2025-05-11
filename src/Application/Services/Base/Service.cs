using Application.Interfaces.Base;
using Domain.Entities.Base;
using Domain.Interfaces.Base;

namespace Application.Services.Base;

public class Service<TDto, TEntity> : IService<TDto, TEntity>
    where TEntity : class, IEntity
{
    private readonly IRepository<TEntity> _repository;

    public Service(IRepository<TEntity> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _repository.GetAllAsync();
    }

    public async Task<TEntity> GetByIdAsync(Guid id)
    {
        return await _repository.GetByIdAsync(id);
    }
}
