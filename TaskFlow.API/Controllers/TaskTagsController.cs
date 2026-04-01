using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Interfaces;

namespace TaskFlow.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TaskTagsController(ITaskTagService taskTagService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<TaskTagDto>>> Get(CancellationToken cancellationToken) =>
        Ok(await taskTagService.GetAllAsync(cancellationToken));

    [HttpPost]
    public async Task<ActionResult<TaskTagDto>> Create([FromBody] TaskTagUpsertDto dto, CancellationToken cancellationToken) =>
        Ok(await taskTagService.CreateAsync(dto, cancellationToken));

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken) =>
        await taskTagService.DeleteAsync(id, cancellationToken) ? NoContent() : NotFound();
}
