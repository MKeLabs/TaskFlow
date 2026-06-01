using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.BLL.DTOs;
using TaskFlow.BLL.Services.Implementations;
using TaskFlow.BLL.Services.Interfaces;
using TaskFlow.DAL.Auth;

namespace TaskFlow.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GoalsController : ControllerBase
    {
        private readonly IGoalsService _goalsService;

        public GoalsController(IGoalsService goalsService)
        {
            this._goalsService = goalsService;
        }

        [HttpGet]
        public async Task<ActionResult<List<GoalDto>>> Get(CancellationToken cancellationToken) =>
            Ok(await _goalsService.GetAllAsync(cancellationToken));

        [HttpPost]
        public async Task<ActionResult<GoalDto>> Create([FromBody] GoalCreateDto dto, CancellationToken cancellationToken)
        {
            var created = await _goalsService.CreateAsync(dto, cancellationToken);
            return Created();
        }

        [HttpDelete("{id:int}")]
        [Authorize(Roles = AppRoles.Admin)]
        public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
        {
            await _goalsService.DeleteAsync(id, cancellationToken);
            return NoContent();
        }
    }
}
