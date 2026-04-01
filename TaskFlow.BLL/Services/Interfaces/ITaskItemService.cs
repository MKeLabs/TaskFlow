using TaskFlow.BLL.DTOs;

namespace TaskFlow.BLL.Services.Interfaces;

public interface ITaskItemService
{
    Task<List<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<TaskItemDto> CreateAsync(TaskItemUpsertDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, TaskItemUpsertDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
