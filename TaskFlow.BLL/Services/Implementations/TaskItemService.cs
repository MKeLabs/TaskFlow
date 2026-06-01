using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations;

public class TaskItemService(IUnitOfWork _unitOfWork) : ITaskItemService
{
    public async Task<List<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await _unitOfWork.TaskItemsRepository.GetAllWithDetailsAsync(cancellationToken))
        .Select(Map)
        .ToList();

    public async Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.TaskItemsRepository.GetByIdWithDetailsAsync(id, cancellationToken);

        if (entity == null)
            throw new KeyNotFoundException();

        return Map(entity);
    }

    public async Task<TaskItemDto> CreateAsync(TaskItemUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = new TaskItemEntity
        {
            ProjectId = dto.ProjectId,
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            Category = dto.Category,
            DueDate = dto.DueDate
        };

        await _unitOfWork.TaskItemsRepository.AddAsync(entity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Map(entity);
    }

    public async Task<bool> UpdateAsync(int id, TaskItemUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.TaskItemsRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (entity is null) throw new KeyNotFoundException();

        entity.ProjectId = dto.ProjectId;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Status = dto.Status;
        entity.Category = dto.Category;
        entity.DueDate = dto.DueDate;
        entity.TaskItemTags.Clear();

        _unitOfWork.TaskItemsRepository.Update(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await _unitOfWork.TaskItemsRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) throw new KeyNotFoundException();

        _unitOfWork.TaskItemsRepository.Delete(entity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return true;
    }

    private static TaskItemDto Map(TaskItemEntity entity) =>
        new TaskItemDto
        {
            Id = entity.Id,
            ProjectId = entity.ProjectId,
            ProjectName = entity.Project?.Name ?? string.Empty,
            Name = entity.Name,
            Description = entity.Description,
            Status = entity.Status,
            Category = entity.Category,
            DueDate = entity.DueDate,
            Tags = entity.TaskItemTags
                .Where(x => x.TaskTag is not null)
                .Select(x => new TaskTagDto { Id = x.TaskTag!.Id, Name = x.TaskTag.Name })
                .ToList(),
            Comments = entity.Comments
                .Select(x => new TaskCommentDto { Id = x.Id, TaskItemId = x.TaskItemId, Text = x.Text })
                .ToList()
        };
}
