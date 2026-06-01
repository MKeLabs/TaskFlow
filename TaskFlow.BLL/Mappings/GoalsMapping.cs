using System;
using System.Collections.Generic;
using System.Text;
using TaskFlow.BLL.DTOs;
using TaskFlow.DAL.Entities;

namespace TaskFlow.BLL.Mappings
{
    public static class GoalsMapping
    {
        public static GoalDto ToDto(this GoalEntity entity) => new GoalDto { Id = entity.Id, ProjectId = entity.ProjectId, Title = entity.Title, TargetDate = entity.TargetDate, IsCompleted = entity.IsCompleted };

        public static GoalEntity ToEntity(this GoalDto dto) => new GoalEntity { Id = dto.Id, ProjectId = dto.ProjectId, Title = dto.Title, TargetDate = dto.TargetDate, IsCompleted = dto.IsCompleted };

        public static GoalEntity ToEntity(this GoalCreateDto dto) => new GoalEntity { ProjectId = dto.ProjectId, Title = dto.Title, TargetDate = dto.TargetDate, IsCompleted = dto.IsCompleted };
    }
}
