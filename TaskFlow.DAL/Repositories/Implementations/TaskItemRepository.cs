using Microsoft.EntityFrameworkCore;
using TaskFlow.DAL.Context;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL.Repositories.Implementations;

public class TaskItemRepository(TaskFlowDbContext dbContext)
    : GenericRepository<TaskItemEntity>(dbContext), ITaskItemRepository
{
    public Task<List<TaskItemEntity>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default) =>
        _dbSet.Include(x => x.Project)
            .Include(x => x.Comments)
            .Include(x => x.TaskItemTags)
            .ThenInclude(x => x.TaskTag)
            .ToListAsync(cancellationToken);

    public Task<TaskItemEntity?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default) =>
        _dbSet.Include(x => x.Project)
            .Include(x => x.Comments)
            .Include(x => x.TaskItemTags)
            .ThenInclude(x => x.TaskTag)
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
}
