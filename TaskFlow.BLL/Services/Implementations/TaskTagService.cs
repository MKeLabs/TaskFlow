using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations;

public class TaskTagService(IGenericRepository<TaskTagEntity> taskTagRepository) : ITaskTagService
{
    public async Task<List<TaskTagDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await taskTagRepository.GetAllAsync(cancellationToken))
        .Select(x => new TaskTagDto(x.Id, x.Name))
        .ToList();

    public async Task<TaskTagDto> CreateAsync(TaskTagUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new TaskTagEntity { Name = dto.Name };
        await taskTagRepository.AddAsync(entity, cancellationToken);
        await taskTagRepository.SaveChangesAsync(cancellationToken);
        return new TaskTagDto(entity.Id, entity.Name);
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await taskTagRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        taskTagRepository.SoftDelete(entity);
        await taskTagRepository.SaveChangesAsync(cancellationToken);
        return true;
    }
}
