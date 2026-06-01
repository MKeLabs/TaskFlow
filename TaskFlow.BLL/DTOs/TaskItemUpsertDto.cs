using TaskFlow.DAL.Entities;
using DalTaskCategory = TaskFlow.DAL.Entities.TaskCategory;
using DalTaskStatus = TaskFlow.DAL.Entities.TaskStatus;

namespace TaskFlow.BLL.DTOs;

public record TaskItemUpsertDto
{
    public int ProjectId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DalTaskStatus Status { get; set; }
    public DalTaskCategory Category { get; set; }
    public DateTime? DueDate { get; set; }
    public IReadOnlyCollection<int> TagIds { get; set; } = new List<int>();

    internal TaskItemEntity ToEntity()
    {
        throw new NotImplementedException();
    }
}
