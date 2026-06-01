using TaskFlow.DAL.Context;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL.Repositories.Implementations;

public class TaskTagRepository : GenericRepository<TaskTagEntity>, ITaskTagRepository
{
    public TaskTagRepository(TaskFlowDbContext dbContext) : base(dbContext)
    {
    }
}
