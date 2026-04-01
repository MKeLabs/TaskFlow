using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskCommentsController(ITaskCommentService taskCommentService) : ControllerBase
{
    [HttpGet("by-task/{taskItemId:int}")]
    public async Task<ActionResult<List<TaskCommentDto>>> GetByTask(int taskItemId, CancellationToken cancellationToken) =>
        Ok(await taskCommentService.GetByTaskItemIdAsync(taskItemId, cancellationToken));

    [HttpPost]
    public async Task<ActionResult<TaskCommentDto>> Create([FromBody] TaskCommentCreateDto dto, CancellationToken cancellationToken)
    {
        var created = await taskCommentService.CreateAsync(dto, cancellationToken);
        return Ok(created);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) =>
        await taskCommentService.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
