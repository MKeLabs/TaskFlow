using TaskFlow.DAL.Context;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL.Repositories.Implementations;

public class TaskCommentRepository : GenericRepository<Entities.TaskCommentEntity>, ITaskCommentRepository
{
    public TaskCommentRepository(TaskFlowDbContext dbContext) : base(dbContext)
    {
    }
}
