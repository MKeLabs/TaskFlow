namespace TaskFlow.DAL.Entities;

public class TaskItemTagEntity
{
    public int TaskItemId { get; set; }
    public TaskItemEntity? TaskItem { get; set; }
    public int TaskTagId { get; set; }
    public TaskTagEntity? TaskTag { get; set; }
}
