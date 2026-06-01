using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations;

public class TaskTagService(IUnitOfWork _unitOfWork) : ITaskTagService
{
    public async Task<List<TaskTagDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _unitOfWork.TaskTagsRepository.GetAllAsync(cancellationToken))
        .Select(x => new TaskTagDto { Id = x.Id, Name = x.Name })
        .ToList();

    public async Task<TaskTagDto> CreateAsync(TaskTagUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new TaskTagEntity { Name = dto.Name };
        await _unitOfWork.TaskTagsRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return new TaskTagDto { Id = entity.Id, Name = entity.Name };
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.TaskTagsRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        _unitOfWork.TaskTagsRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }
}
