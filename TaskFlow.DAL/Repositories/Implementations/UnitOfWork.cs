using TaskFlow.DAL.Context;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.DAL.Repositories.Implementations;

public class UnitOfWork : IUnitOfWork
{
    private readonly TaskFlowDbContext _dbContext;

    private readonly IProjectsRepository _projectsRepository;
    private readonly ITaskCommentRepository _taskCommentsRepository;
    private readonly ITaskItemRepository _taskItemsRepository;
    private readonly ITaskTagRepository _taskTagsRepository;

    public UnitOfWork(TaskFlowDbContext dbContext, IProjectsRepository projectsRepository, ITaskCommentRepository taskCommentsRepository, ITaskItemRepository taskItemsRepository, ITaskTagRepository taskTagsRepository)
    {
        _dbContext = dbContext;
        _projectsRepository = projectsRepository;
        _taskCommentsRepository = taskCommentsRepository;
        _taskItemsRepository = taskItemsRepository;
        _taskTagsRepository = taskTagsRepository;
    }

    public IProjectsRepository ProjectsRepository => _projectsRepository;

    public ITaskCommentRepository TaskCommentsRepository => _taskCommentsRepository;

    public ITaskItemRepository TaskItemsRepository => _taskItemsRepository;

    public ITaskTagRepository TaskTagsRepository => _taskTagsRepository;

    public Task SaveChangesAsync(CancellationToken cancellationToken = default)
        => _dbContext.SaveChangesAsync(cancellationToken);
}
