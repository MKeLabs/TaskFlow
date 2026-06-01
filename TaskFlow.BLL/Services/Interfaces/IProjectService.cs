using TaskFlow.BLL.DTOs;

namespace TaskFlow.BLL.Services.Interfaces;

public interface IProjectService
{
    Task<List<ProjectDto>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<ProjectDto> GetByIdAsync(int id, CancellationToken cancellationToken = default);
    Task<ProjectDto> CreateAsync(ProjectUpsertDto dto, CancellationToken cancellationToken = default);
    Task<bool> UpdateAsync(int id, ProjectUpsertDto dto, CancellationToken cancellationToken = default);
    Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default);
}
