namespace TaskFlow.DAL.Repositories.Interfaces;

public interface IUnitOfWork
{
    IProjectsRepository ProjectsRepository { get; }
    ITaskCommentRepository TaskCommentsRepository { get; }
    ITaskItemRepository TaskItemsRepository { get; }
    ITaskTagRepository TaskTagsRepository { get; }

    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
