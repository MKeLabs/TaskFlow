using TaskFlow.BLL.DTOs;

namespace TaskFlow.BLL.Services.Interfaces;

public interface ITaskCommentService
{
    Task<List<TaskCommentDto>> GetByTaskItemIdAsync(int taskItemId, CancellationToken cancellationToken = default);
    Task<TaskCommentDto> CreateAsync(TaskCommentCreateDto dto, string? createdByUserId, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
