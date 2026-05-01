using System.ComponentModel.DataAnnotations;

namespace BananaGestion.Application.Modules.Users.DTOs;

public record LoginRequest(
    [Required][EmailAddress] string Email,
    [Required] string Password
);

public record RegisterRequest(
    [Required][EmailAddress] string Email,
    [Required][MinLength(6)] string Password,
    [Required] string Nombre,
    [Required] string Apellido,
    string? Telefono,
    [Required] string Rol
);

public record AuthResponse(
    Guid Id,
    string Email,
    string Nombre,
    string Apellido,
    string Rol,
    string Token
);

public record UserDto(
    Guid Id,
    string Email,
    string Nombre,
    string Apellido,
    string? Telefono,
    string Rol,
    bool Activo,
    DateTime FechaCreacion,
    DateTime? UltimoLogin
);

public record UpdateUserRequest(
    string? Nombre,
    string? Apellido,
    string? Telefono
);

public record ChangePasswordRequest(
    [Required] string CurrentPassword,
    [Required][MinLength(6)] string NewPassword
);
