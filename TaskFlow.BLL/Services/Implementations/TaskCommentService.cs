using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations;

public class TaskCommentService(
    IGenericRepository<TaskCommentEntity> commentRepository,
    ITaskItemRepository taskItemRepository) : ITaskCommentService
{
    public async Task<List<TaskCommentDto>> GetByTaskItemIdAsync(int taskItemId, CancellationToken cancellationToken = default)
    {
        var task = await taskItemRepository.GetByIdWithDetailsAsync(taskItemId, cancellationToken);
        if (task is null) return [];

        return task.Comments.Select(x => new TaskCommentDto(x.Id, x.TaskItemId, x.Text)).ToList();
    }

    public async Task<TaskCommentDto> CreateAsync(TaskCommentCreateDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new TaskCommentEntity
        {
            TaskItemId = dto.TaskItemId,
            Text = dto.Text
        };

        await commentRepository.AddAsync(entity, cancellationToken);
        await commentRepository.SaveChangesAsync(cancellationToken);
        return new TaskCommentDto(entity.Id, entity.TaskItemId, entity.Text);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await commentRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        commentRepository.SoftDelete(entity);
        await commentRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
