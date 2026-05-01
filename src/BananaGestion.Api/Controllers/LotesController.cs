using BananaGestion.Application.Modules.Lotes.Commands;
using BananaGestion.Application.Modules.Lotes.DTOs;
using BananaGestion.Application.Modules.Lotes.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BananaGestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class LotesController : ControllerBase
{
    private readonly IMediator _mediator;

    public LotesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<LoteDto>>> GetAll()
    {
        var lotes = await _mediator.Send(new GetLotesQuery());
        return Ok(lotes);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<LoteDto>>> GetActive()
    {
        var lotes = await _mediator.Send(new GetActiveLotesQuery());
        return Ok(lotes);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<LoteDto>> GetById(Guid id)
    {
        var lote = await _mediator.Send(new GetLoteByIdQuery(id));
        return Ok(lote);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPost]
    public async Task<ActionResult<LoteDto>> Create([FromBody] CreateLoteRequest request)
    {
        var lote = await _mediator.Send(new CreateLoteCommand(request));
        return CreatedAtAction(nameof(GetById), new { id = lote.Id }, lote);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPut("{id}")]
    public async Task<ActionResult<LoteDto>> Update(Guid id, [FromBody] UpdateLoteRequest request)
    {
        var lote = await _mediator.Send(new UpdateLoteCommand(id, request));
        return Ok(lote);
    }

    [Authorize(Roles = "Administrador")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var result = await _mediator.Send(new DeleteLoteCommand(id));
        return Ok(result);
    }
}
