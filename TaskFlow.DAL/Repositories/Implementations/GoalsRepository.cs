using TaskFlow.DAL.Context;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL.Repositories.Implementations
{
    public class GoalsRepository : GenericRepository<GoalEntity>, IGoalsRepository
    {
        public GoalsRepository(TaskFlowDbContext dbContext) : base(dbContext)
        {
        }
    }
}
