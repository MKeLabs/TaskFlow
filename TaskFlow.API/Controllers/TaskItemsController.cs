using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskItemsController(ITaskItemService taskItemService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TaskItemDto>>> Get(CancellationToken cancellationToken) =>
        Ok(await taskItemService.GetAllAsync(cancellationToken));

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskItemDto>> GetById(int id, CancellationToken cancellationToken)
    {
        var item = await taskItemService.GetByIdAsync(id, cancellationToken);
        return Ok(item);
    }

    [HttpPost]
    public async Task<ActionResult<TaskItemDto>> Create([FromBody] TaskItemUpsertDto dto, CancellationToken cancellationToken)
    {
        var created = await taskItemService.CreateAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] TaskItemUpsertDto dto, CancellationToken cancellationToken)
    {
        await taskItemService.UpdateAsync(id, dto, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await taskItemService.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
