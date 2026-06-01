namespace TaskFlow.DAL.Entities;

public class GoalEntity : BaseEntity
{
    public int ProjectId { get; set; }
    public required string Title { get; set; } = string.Empty;
    public DateTime TargetDate { get; set; }
    public bool IsCompleted { get; set; }

    public virtual ProjectEntity Project { get; set; } = null!;
}
