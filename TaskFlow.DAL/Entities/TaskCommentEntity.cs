namespace TaskFlow.DAL.Entities;

public class TaskCommentEntity : BaseEntity
{
    public int TaskItemId { get; set; }
    public TaskItemEntity? TaskItem { get; set; }
    public string Text { get; set; } = string.Empty;
}
