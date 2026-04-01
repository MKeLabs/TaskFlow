using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Entities;
using TaskFlow.DAL.Repositories.Interfaces;

namespace TaskFlow.BLL.Services.Implementations;

public class TaskItemService(
    ITaskItemRepository taskItemRepository,
    IGenericRepository<TaskTagEntity> tagRepository) : ITaskItemService
{
    public async Task<List<TaskItemDto>> GetAllAsync(CancellationToken cancellationToken = default) =>
        (await taskItemRepository.GetAllWithDetailsAsync(cancellationToken))
        .Select(Map)
        .ToList();

    public async Task<TaskItemDto?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await taskItemRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        return entity is null ? null : Map(entity);
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

        await ApplyTagsAsync(entity, dto.TagIds, cancellationToken);
        await taskItemRepository.AddAsync(entity, cancellationToken);
        await taskItemRepository.SaveChangesAsync(cancellationToken);

        var fullEntity = await taskItemRepository.GetByIdWithDetailsAsync(entity.Id, cancellationToken);
        return Map(fullEntity!);
    }

    public async Task<bool> UpdateAsync(int id, TaskItemUpsertDto dto, CancellationToken cancellationToken = default)
    {
        var entity = await taskItemRepository.GetByIdWithDetailsAsync(id, cancellationToken);
        if (entity is null) return false;

        entity.ProjectId = dto.ProjectId;
        entity.Name = dto.Name;
        entity.Description = dto.Description;
        entity.Status = dto.Status;
        entity.Category = dto.Category;
        entity.DueDate = dto.DueDate;
        entity.TaskItemTags.Clear();
        await ApplyTagsAsync(entity, dto.TagIds, cancellationToken);

        taskItemRepository.Update(entity);
        await taskItemRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    public async Task<bool> DeleteAsync(int id, CancellationToken cancellationToken = default)
    {
        var entity = await taskItemRepository.GetByIdAsync(id, cancellationToken);
        if (entity is null) return false;

        taskItemRepository.SoftDelete(entity);
        await taskItemRepository.SaveChangesAsync(cancellationToken);
        return true;
    }

    private async Task ApplyTagsAsync(TaskItemEntity taskItem, IEnumerable<int> tagIds, CancellationToken cancellationToken)
    {
        foreach (var tagId in tagIds.Distinct())
        {
            var tag = await tagRepository.GetByIdAsync(tagId, cancellationToken);
            if (tag is null) continue;

            taskItem.TaskItemTags.Add(new TaskItemTagEntity
            {
                TaskItem = taskItem,
                TaskTagId = tag.Id,
                TaskTag = tag
            });
        }
    }

    private static TaskItemDto Map(TaskItemEntity entity) =>
        new(
            entity.Id,
            entity.ProjectId,
            entity.Project?.Name ?? string.Empty,
            entity.Name,
            entity.Description,
            entity.Status,
            entity.Category,
            entity.DueDate,
            entity.TaskItemTags
                .Where(x => x.TaskTag is not null)
                .Select(x => new TaskTagDto(x.TaskTag!.Id, x.TaskTag.Name))
                .ToList(),
            entity.Comments
                .Select(x => new TaskCommentDto(x.Id, x.TaskItemId, x.Text))
                .ToList());
}
