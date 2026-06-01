namespace TaskFlow.BLL.DTOs;

public record TaskCommentDto
{
    public int Id { get; set; }
    public int TaskItemId { get; set; }
    public string Text { get; set; } = string.Empty;
    public string? CreatedByUserId { get; set; }
}
