using BananaGestion.Application.Modules.Users.Commands;
using BananaGestion.Application.Modules.Users.DTOs;
using BananaGestion.Application.Modules.Users.Queries;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BananaGestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController : ControllerBase
{
    private readonly IMediator _mediator;

    public UsersController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpGet]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAll()
    {
        var users = await _mediator.Send(new GetUsersQuery());
        return Ok(users);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetById(Guid id)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRole = GetCurrentUserRole();

        if (currentUserId != id && currentUserRole != "Administrador")
        {
            return Forbid();
        }

        var user = await _mediator.Send(new GetUserByIdQuery(id));
        return Ok(user);
    }

    [HttpGet("obreros")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetObreros()
    {
        var obreros = await _mediator.Send(new GetObrerosQuery());
        return Ok(obreros);
    }

    [HttpGet("supervisores")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetSupervisores()
    {
        var supervisores = await _mediator.Send(new GetSupervisoresQuery());
        return Ok(supervisores);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<UserDto>> Update(Guid id, [FromBody] UpdateUserRequest request)
    {
        var currentUserId = GetCurrentUserId();
        var currentUserRole = GetCurrentUserRole();

        if (currentUserId != id && currentUserRole != "Administrador")
        {
            return Forbid();
        }

        var user = await _mediator.Send(new UpdateUserCommand(id, request));
        return Ok(user);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(Guid id)
    {
        var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value 
                           ?? User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value
                           ?? "Obrero";
        var result = await _mediator.Send(new DeleteUserCommand(id, currentUserRole));
        return Ok(result);
    }

    [Authorize(Roles = "Administrador,Supervisor")]
    [HttpPut("{id}/toggle-status")]
    public async Task<ActionResult<bool>> ToggleStatus(Guid id)
    {
        var currentUserRole = User.Claims.FirstOrDefault(c => c.Type == "role")?.Value 
                           ?? User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.Role)?.Value
                           ?? "Obrero";
        var result = await _mediator.Send(new ToggleUserStatusCommand(id, currentUserRole));
        return Ok(result);
    }

    private Guid? GetCurrentUserId()
    {
        var idClaim = User.Claims.FirstOrDefault(c => c.Type == "sub")
                   ?? User.Claims.FirstOrDefault(c => c.Type == System.Security.Claims.ClaimTypes.NameIdentifier);
        if (idClaim != null && Guid.TryParse(idClaim.Value, out var id))
            return id;
        return null;
    }

    private string GetCurrentUserRole()
    {
        return User.Claims.FirstOrDefault(c => c.Type == "role")?.Value ?? "Obrero";
    }
}
