using TaskFlow.DAL.Entities;
using DalTaskCategory = TaskFlow.DAL.Entities.TaskCategory;
using DalTaskStatus = TaskFlow.DAL.Entities.TaskStatus;

namespace TaskFlow.BLL.DTOs;

public record ProjectDto(int Id, string Name);

public record ProjectUpsertDto(string Name);

public record TaskTagDto(int Id, string Name);

public record TaskTagUpsertDto(string Name);

public record TaskCommentDto(int Id, int TaskItemId, string Text);

public record TaskCommentCreateDto(int TaskItemId, string Text);

public record TaskItemDto(
    int Id,
    int ProjectId,
    string ProjectName,
    string Name,
    string? Description,
    DalTaskStatus Status,
    DalTaskCategory Category,
    DateTime? DueDate,
    IReadOnlyCollection<TaskTagDto> Tags,
    IReadOnlyCollection<TaskCommentDto> Comments);

public record TaskItemUpsertDto(
    int ProjectId,
    string Name,
    string? Description,
    DalTaskStatus Status,
    DalTaskCategory Category,
    DateTime? DueDate,
    IReadOnlyCollection<int> TagIds);
