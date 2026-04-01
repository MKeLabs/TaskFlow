using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.Context;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL.Repositories.Implementations;

public class GenericRepository<TEntity>(TaskFlowDbContext dbContext) : IGenericRepository<TEntity>
    where TEntity : BaseEntity
{
    protected readonly TaskFlowDbContext DbContext = dbContext;
    protected readonly DbSet<TEntity> DbSet = dbContext.Set<TEntity>();

    public Task<TEntity?> GetByIdAsync(int id, CancellationToken cancellationToken = default) =>
        DbSet.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public Task<List<TEntity>> GetAllAsync(CancellationToken cancellationToken = default) =>
        DbSet.ToListAsync(cancellationToken);

    public Task AddAsync(TEntity entity, CancellationToken cancellationToken = default) =>
        DbSet.AddAsync(entity, cancellationToken).AsTask();

    public void Update(TEntity entity) => DbSet.Update(entity);

    public void SoftDelete(TEntity entity)
    {
        entity.IsDeleted = true;
        entity.ModifiedAt = DateTime.UtcNow;
        DbSet.Update(entity);
    }

    public Task SaveChangesAsync(CancellationToken cancellationToken = default) =>
        DbContext.SaveChangesAsync(cancellationToken);
}
