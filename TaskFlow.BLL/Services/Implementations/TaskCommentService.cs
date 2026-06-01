using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations;

public class TaskCommentService(
    IUnitOfWork _unitOfWork) : ITaskCommentService
{
    public async Task<List<TaskCommentDto>> GetByTaskItemIdAsync(int taskItemId, CancellationToken cancellationToken = default)
    {
        var task = await _unitOfWork.TaskItemsRepository.GetByIdWithDetailsAsync(taskItemId, cancellationToken);
        if (task is null) return [];

        return task.Comments.Select(x => new TaskCommentDto { Id = x.Id, TaskItemId = x.TaskItemId, Text = x.Text, CreatedByUserId = x.CreatedByUserId }).ToList();
    }

    public async Task<TaskCommentDto> CreateAsync(TaskCommentCreateDto dto, string? createdByUserId, CancellationToken cancellationToken = default)
    {
        var entity = new TaskCommentEntity
        {
            TaskItemId = dto.TaskItemId,
            Text = dto.Text,
            CreatedByUserId = createdByUserId
        };

        await _unitOfWork.TaskCommentsRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new TaskCommentDto { Id = entity.Id, TaskItemId = entity.TaskItemId, Text = entity.Text, CreatedByUserId = entity.CreatedByUserId };
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.TaskCommentsRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) throw new KeyNotFoundException();

        _unitOfWork.TaskCommentsRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
