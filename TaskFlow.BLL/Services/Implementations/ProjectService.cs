using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly IUnitOfWork _unitOfWork;

    public ProjectService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<List<ProjectDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _unitOfWork.ProjectsRepository.GetAllAsync(cancellationToken))
        .Select(x => new ProjectDto { Id = x.Id, Name = x.Name })
        .ToList();

    public async Task<ProjectDto> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ProjectsRepository.GetByIdAsync(id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException($"Project with id {id} not found.");

        return new ProjectDto { Id = entity.Id, Name = entity.Name };
    }

    public async Task<ProjectDto> CreateAsync(ProjectUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new ProjectEntity { Name = dto.Name };
        await _unitOfWork.ProjectsRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new ProjectDto { Id = entity.Id, Name = entity.Name };
    }

    public async Task<bool> UpdateAsync(int id, ProjectUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ProjectsRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) throw new KeyNotFoundException();

        entity.Name = dto.Name;
        _unitOfWork.ProjectsRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.ProjectsRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) throw new KeyNotFoundException();

        _unitOfWork.ProjectsRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
