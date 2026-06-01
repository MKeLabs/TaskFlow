namespace TaskFlow.DAL.Entities;

public class ProjectEntity : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public ICollection<TaskItemEntity> TaskItems { get; set; } = new List<TaskItemEntity>();

    public ICollection<GoalEntity> Goals { get; set; } = new List<GoalEntity>();
}
