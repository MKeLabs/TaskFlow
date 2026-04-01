namespace TaskFlow.DAL.Entities;

public class TaskItemEntity : BaseEntity
{
    public int ProjectId { get; set; }
    public ProjectEntity? Project { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public TaskStatus Status { get; set; }
    public TaskCategory Category { get; set; }
    public DateTime? DueDate { get; set; }
    public ICollection<TaskCommentEntity> Comments { get; set; } = new List<TaskCommentEntity>();
    public ICollection<TaskItemTagEntity> TaskItemTags { get; set; } = new List<TaskItemTagEntity>();
}
