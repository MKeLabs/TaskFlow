using System;
using System.Collections.Generic;
using System.Text;
using TaskFlow.BLL.DTOs;
using TaskFlow.DAL.Entities;

namespace TaskFlow.BLL.Mappings
{
    public static class TaskItemMappings
    {
        public static TaskItemEntity ToEntity(this TaskItemUpsertDto dto) => new TaskItemEntity
        {
            ProjectId = dto.ProjectId,
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            Category = dto.Category,
            DueDate = dto.DueDate
        };

        public static TaskItemDto ToDto(this TaskItemEntity entity) => new TaskItemDto
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
                .Select(x => new TaskCommentDto { Id = x.Id, TaskItemId = x.TaskItemId, Text = x.Text, CreatedByUserId = x.CreatedByUserId })
                .ToList()
        };
    }
}
