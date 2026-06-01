namespace TaskFlow.BLL.DTOs;

public record TaskCommentCreateDto
{
    public int TaskItemId { get; set; }

    public string Text { get; set; } = string.Empty;
}
