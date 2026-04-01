using TaskFlow.BLL.DTOs;

namespace TaskFlow.BLL.Services.Interfaces;

public interface ITaskTagService
{
    Task<List<TaskTagDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<TaskTagDto> CreateAsync(TaskTagUpsertDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
