using TaskFlow.DAL.Context;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL.Repositories.Implementations;

public class ProjectsRepository : GenericRepository<Entities.ProjectEntity>, IProjectsRepository
{
    public ProjectsRepository(TaskFlowDbContext dbContext) : base(dbContext)
    {
    }
}
