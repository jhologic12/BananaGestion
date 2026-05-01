using BananaGestion.Application.Modules.Cosecha.Commands;
using BananaGestion.Application.Modules.Cosecha.DTOs;
using BananaGestion.Application.Modules.Cosecha.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BananaGestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CosechaController : ControllerBase
{
    private readonly IMediator _mediator;

    public CosechaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("calendar/{year}")]
    public async Task<ActionResult<IEnumerable<HarvestCalendarDto>>> GetCalendar(int year)
    {
        var calendar = await _mediator.Send(new GetHarvestCalendarQuery(year));
        return Ok(calendar);
    }

    [HttpGet("calendar/week/{week}/{year}")]
    public async Task<ActionResult<HarvestCalendarDto>> GetCalendarByWeek(int week, int year)
    {
        var calendar = await _mediator.Send(new GetHarvestCalendarByWeekQuery(week, year));
        return Ok(calendar);
    }

    [HttpGet("encinte/{year}")]
    public async Task<ActionResult<IEnumerable<EncinteDto>>> GetEncinte(int year)
    {
        var encinte = await _mediator.Send(new GetEncinteByYearQuery(year));
        return Ok(encinte);
    }

    [HttpGet("encinte/{semana}/{ano}")]
    public async Task<ActionResult<IEnumerable<EncinteDto>>> GetEncinteByWeek(int semana, int ano)
    {
        var encinte = await _mediator.Send(new GetEncinteByWeekQuery(semana, ano));
        return Ok(encinte);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPost("encinte")]
    public async Task<ActionResult<EncinteDto>> CreateEncinte([FromBody] CreateEncinteRequest request)
    {
        var currentUserId = GetCurrentUserId();
        if (currentUserId == null)
            return Unauthorized("No se pudo identificar el usuario");

        var requestWithUser = request with { UserId = currentUserId.Value };
        var encinte = await _mediator.Send(new CreateEncinteCommand(requestWithUser));
        return Created("", encinte);
    }

    [HttpGet("cosecha/{year}")]
    public async Task<ActionResult<IEnumerable<HarvestCosechaDto>>> GetCosechas(int year)
    {
        var cosechas = await _mediator.Send(new GetCosechasByYearQuery(year));
        return Ok(cosechas);
    }

    [HttpGet("cosecha/encinte/{semana}/{ano}")]
    public async Task<ActionResult<IEnumerable<HarvestCosechaDto>>> GetCosechasByEncinte(int semana, int ano)
    {
        var cosechas = await _mediator.Send(new GetCosechasByEncinteWeekQuery(semana, ano));
        return Ok(cosechas);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPost("cosecha")]
    public async Task<ActionResult<HarvestCosechaDto>> CreateCosecha([FromBody] CreateCosechaRequest request)
    {
        var cosecha = await _mediator.Send(new CreateCosechaCommand(request));
        return Created("", cosecha);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPut("cosecha/{id}")]
    public async Task<ActionResult<HarvestCosechaDto>> UpdateCosecha(Guid id, [FromBody] UpdateCosechaRequest request)
    {
        var cosecha = await _mediator.Send(new UpdateCosechaCommand(id, request));
        return Ok(cosecha);
    }

    [HttpGet("proyeccion")]
    public async Task<ActionResult<IEnumerable<SemanaProyeccionDto>>> GetProyeccion([FromQuery] int? year = null)
    {
        var proyeccion = await _mediator.Send(new GetProyeccionQuery(year));
        return Ok(proyeccion);
    }

    [HttpGet("boxtypes")]
    public async Task<ActionResult<IEnumerable<BoxTypeDto>>> GetBoxTypes()
    {
        var types = await _mediator.Send(new GetBoxTypesQuery());
        return Ok(types);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPost("boxtypes")]
    public async Task<ActionResult<BoxTypeDto>> CreateBoxType([FromBody] CreateBoxTypeRequest request)
    {
        var boxType = await _mediator.Send(new CreateBoxTypeCommand(request));
        return Created("", boxType);
    }

    private Guid? GetCurrentUserId()
    {
        var idClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")
                   ?? User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        if (idClaim != null && Guid.TryParse(idClaim.Value, out var id))
            return id;
        return null;
    }
}
