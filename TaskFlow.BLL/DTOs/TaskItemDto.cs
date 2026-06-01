using DalTaskCategory = TaskFlow.DAL.Entities.TaskCategory;
using DalTaskStatus = TaskFlow.DAL.Entities.TaskStatus;

namespace TaskFlow.BLL.DTOs;

public record TaskItemDto
{
    public int Id { get; set; }
    public int ProjectId { get; set; }
    public string ProjectName { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DalTaskStatus Status { get; set; }
    public DalTaskCategory Category { get; set; }
    public DateTime? DueDate { get; set; }
    public IReadOnlyCollection<TaskTagDto> Tags { get; set; } = new List<TaskTagDto>();
    public IReadOnlyCollection<TaskCommentDto> Comments { get; set; } = new List<TaskCommentDto>();
}
