using BananaGestion.Application.Modules.Tareas.Commands;
using BananaGestion.Application.Modules.Tareas.DTOs;
using BananaGestion.Application.Modules.Tareas.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BananaGestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TareasController : ControllerBase
{
    private readonly IMediator _mediator;

    public TareasController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("configs")]
    public async Task<ActionResult<IEnumerable<TaskConfigDto>>> GetConfigs()
    {
        var configs = await _mediator.Send(new GetTaskConfigsQuery());
        return Ok(configs);
    }

    [HttpGet("configs/{id}")]
    public async Task<ActionResult<TaskConfigDto>> GetConfigById(Guid id)
    {
        var config = await _mediator.Send(new GetTaskConfigByIdQuery(id));
        return Ok(config);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPost("configs")]
    public async Task<ActionResult<TaskConfigDto>> CreateConfig([FromBody] CreateTaskConfigRequest request)
    {
        var config = await _mediator.Send(new CreateTaskConfigCommand(request));
        return CreatedAtAction(nameof(GetConfigById), new { id = config.Id }, config);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPut("configs/{id}")]
    public async Task<ActionResult<TaskConfigDto>> UpdateConfig(Guid id, [FromBody] UpdateTaskConfigRequest request)
    {
        var config = await _mediator.Send(new UpdateTaskConfigCommand(id, request));
        return Ok(config);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpDelete("configs/{id}")]
    public async Task<ActionResult<bool>> DeleteConfig(Guid id)
    {
        var result = await _mediator.Send(new DeleteTaskConfigCommand(id));
        return Ok(result);
    }

    [HttpGet("assignments")]
    public async Task<ActionResult<IEnumerable<TaskAssignmentDto>>> GetPendingAssignments([FromQuery] Guid? userId = null)
    {
        var assignments = await _mediator.Send(new GetPendingAssignmentsQuery(userId));
        return Ok(assignments);
    }

    [HttpGet("assignments/range")]
    public async Task<ActionResult<IEnumerable<TaskAssignmentDto>>> GetAssignmentsByRange(
        [FromQuery] DateTime start, [FromQuery] DateTime end)
    {
        var assignments = await _mediator.Send(new GetAssignmentsByDateRangeQuery(start, end));
        return Ok(assignments);
    }

    [HttpGet("assignments/{id}")]
    public async Task<ActionResult<TaskAssignmentDto>> GetAssignmentById(Guid id)
    {
        var assignment = await _mediator.Send(new GetAssignmentWithDetailsQuery(id));
        return Ok(assignment);
    }

    [HttpGet("assignments/overdue")]
    public async Task<ActionResult<IEnumerable<TaskAssignmentDto>>> GetOverdue()
    {
        var assignments = await _mediator.Send(new GetOverdueAssignmentsQuery());
        return Ok(assignments);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPost("assignments")]
    public async Task<ActionResult<TaskAssignmentDto>> CreateAssignment([FromBody] CreateTaskAssignmentRequest request)
    {
        var assignment = await _mediator.Send(new CreateTaskAssignmentCommand(request));
        return CreatedAtAction(nameof(GetAssignmentById), new { id = assignment.Id }, assignment);
    }

    [HttpPut("assignments/{id}/status")]
    public async Task<ActionResult<bool>> UpdateStatus(Guid id, [FromQuery] string status)
    {
        var result = await _mediator.Send(new UpdateAssignmentStatusCommand(id, status));
        return Ok(result);
    }

    [HttpGet("logs/{assignmentId}")]
    public async Task<ActionResult<TaskLogDto>> GetTaskLog(Guid assignmentId)
    {
        var log = await _mediator.Send(new GetTaskLogQuery(assignmentId));
        return Ok(log);
    }

    [HttpPost("logs")]
    public async Task<ActionResult<TaskLogDto>> CreateLog(
        [FromForm] CreateTaskLogRequest request,
        IFormFile? foto,
        IFormFile? firma)
    {
        Stream? fotoStream = null;
        Stream? firmaStream = null;

        if (foto != null)
        {
            fotoStream = foto.OpenReadStream();
        }

        if (firma != null)
        {
            firmaStream = firma.OpenReadStream();
        }

        var log = await _mediator.Send(new CreateTaskLogCommand(request, fotoStream, firmaStream));
        return Created("", log);
    }
}
