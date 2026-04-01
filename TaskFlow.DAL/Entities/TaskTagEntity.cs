namespace TaskFlow.DAL.Entities;

public class TaskTagEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<TaskItemTagEntity> TaskItemTags { get; set; } = new List<TaskItemTagEntity>();
}
