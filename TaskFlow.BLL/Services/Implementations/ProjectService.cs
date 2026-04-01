using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations;

public class ProjectService(IGenericRepository<ProjectEntity> projectRepository) : IProjectService
{
    public async Task<List<ProjectDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await projectRepository.GetAllAsync(cancellationToken))
        .Select(x => new ProjectDto(x.Id, x.Name))
        .ToList();

    public async Task<ProjectDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await projectRepository.GetByIdAsync(id, cancellationToken);
        return entity is null ? null : new ProjectDto(entity.Id, entity.Name);
    }

    public async Task<ProjectDto> CreateAsync(ProjectUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new ProjectEntity { Name = dto.Name };
        await projectRepository.AddAsync(entity, cancellationToken);
        await projectRepository.SaveChangesAsync(cancellationToken);
        return new ProjectDto(entity.Id, entity.Name);
    }

    public async Task<bool> UpdateAsync(int id, ProjectUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await projectRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        entity.Name = dto.Name;
        projectRepository.Update(entity);
        await projectRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await projectRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        projectRepository.SoftDelete(entity);
        await projectRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
