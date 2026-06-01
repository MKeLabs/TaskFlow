using TaskFlow.BLL.DTOs;

namespace TaskFlow.BLL.Services.Interfaces
{
    public interface IGoalsService
    {
        Task<List<GoalDto>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<GoalDto> CreateAsync(GoalCreateDto dto, CancellationToken cancellationToken = default);
        Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
    }
}
