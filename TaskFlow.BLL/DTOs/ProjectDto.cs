namespace TaskFlow.BLL.DTOs;

public record ProjectDto
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;
}
