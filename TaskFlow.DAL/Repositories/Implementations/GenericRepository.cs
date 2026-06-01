using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.Context;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL.Repositories.Implementations;

public class GenericRepository<TEntity>(TaskFlowDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly TaskFlowDbContext _dbContext = dbContext;
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        _dbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _dbSet.ToListAsync(cancellationToken);

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        _dbSet.AddAsync(entity, cancellationToken).AsTask();

    public void Update(TEntity entity) => _dbSet.Update(entity);

    public void Delete(TEntity entity)  => _dbSet.Remove(entity);
}
