using TaskFlow.DAL.Entities;

namespace TaskFlow.DAL.Repositories.Interfaces;

public interface ITaskItemRepository : IGenericRepository<TaskItemEntity>
{
    Task<List<TaskItemEntity>> GetAllWithDetailsAsync(CancellationToken cancellationToken = default);
    Task<TaskItemEntity?> GetByIdWithDetailsAsync(int id, CancellationToken cancellationToken = default);
}
