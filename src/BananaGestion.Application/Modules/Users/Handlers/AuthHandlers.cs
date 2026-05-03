using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Application.Modules.Users.Commands;
using BananaGestion.Application.Modules.Users.DTOs;
using BananaGestion.Domain.Entities;
using BananaGestion.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Logging;

namespace BananaGestion.Application.Modules.Users.Handlers;

public class LoginHandler : IRequestHandler<LoginCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtService;
    private readonly ILogger<LoginHandler> _logger;

    public LoginHandler(IUserRepository userRepository, IJwtTokenService jwtService, ILogger<LoginHandler> logger)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
        _logger = logger;
    }

    public async Task<AuthResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        try
        {
            _logger.LogInformation("Login attempt for email: {Email}", request.Request.Email);
            
            var user = await _userRepository.GetByEmailAsync(request.Request.Email);
            if (user == null)
            {
                _logger.LogWarning("Login failed: User not found for email: {Email}", request.Request.Email);
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            _logger.LogInformation("User found: {Email}, Activo: {Activo}, Rol: {Rol}", user.Email, user.Activo, user.Rol);

            if (!BCrypt.Net.BCrypt.Verify(request.Request.Password, user.PasswordHash))
            {
                _logger.LogWarning("Login failed: Invalid password for email: {Email}", request.Request.Email);
                throw new UnauthorizedAccessException("Credenciales inválidas");
            }

            if (!user.Activo)
            {
                _logger.LogWarning("Login failed: User inactive for email: {Email}", request.Request.Email);
                throw new UnauthorizedAccessException("Usuario inactivo");
            }

            // Update last login (best effort, don't fail login if this fails)
            try
            {
                user.UltimoLogin = DateTime.UtcNow;
                await _userRepository.UpdateAsync(user);
                _logger.LogInformation("Updated last login for user: {Email}", user.Email);
            }
            catch (Exception ex)
            {
                _logger.LogWarning("Failed to update last login for {Email}: {Message}", user.Email, ex.Message);
                // Don't fail the login if this fails
            }

            _logger.LogInformation("Generating JWT token for user: {Email}", user.Email);
            var token = _jwtService.GenerateToken(user.Id, user.Email, user.Rol.ToString());
            _logger.LogInformation("Login successful for email: {Email}", request.Request.Email);

            return new AuthResponse(user.Id, user.Email, user.Nombre, user.Apellido, user.Rol.ToString(), token);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Login error for email {Email}: {Message}", request.Request.Email, ex.Message);
            throw;
        }
    }
}

public class RegisterHandler : IRequestHandler<RegisterCommand, AuthResponse>
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtService;

    public RegisterHandler(IUserRepository userRepository, IJwtTokenService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<AuthResponse> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existing = await _userRepository.GetByEmailAsync(request.Request.Email);
        if (existing != null)
        {
            throw new InvalidOperationException("El email ya está registrado");
        }

        var user = new User
        {
            Email = request.Request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.Password, 12),
            Nombre = request.Request.Nombre,
            Apellido = request.Request.Apellido,
            Telefono = request.Request.Telefono,
            Rol = UserRole.Obrero
        };

        await _userRepository.AddAsync(user);

        var token = _jwtService.GenerateToken(user.Id, user.Email, user.Rol.ToString());

        return new AuthResponse(user.Id, user.Email, user.Nombre, user.Apellido, user.Rol.ToString(), token);
    }
}

public class UpdateUserHandler : IRequestHandler<UpdateUserCommand, UserDto>
{
    private readonly IUserRepository _userRepository;

    public UpdateUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        if (!string.IsNullOrEmpty(request.Request.Nombre))
            user.Nombre = request.Request.Nombre;
        if (!string.IsNullOrEmpty(request.Request.Apellido))
            user.Apellido = request.Request.Apellido;
        if (request.Request.Telefono != null)
            user.Telefono = request.Request.Telefono;

        await _userRepository.UpdateAsync(user);

        return new UserDto(user.Id, user.Email, user.Nombre, user.Apellido, user.Telefono, 
            user.Rol.ToString(), user.Activo, user.FechaCreacion, user.UltimoLogin);
    }
}

public class ChangePasswordHandler : IRequestHandler<ChangePasswordCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ChangePasswordHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        if (!BCrypt.Net.BCrypt.Verify(request.Request.CurrentPassword, user.PasswordHash))
        {
            throw new UnauthorizedAccessException("Contraseña actual incorrecta");
        }

        user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Request.NewPassword);
        await _userRepository.UpdateAsync(user);

        return true;
    }
}

public class DeleteUserHandler : IRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public DeleteUserHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        if (request.CurrentUserRole == "Supervisor" && user.Rol.ToString() == "Administrador")
        {
            throw new UnauthorizedAccessException("Un supervisor no puede eliminar un administrador");
        }

        user.Activo = false;
        await _userRepository.UpdateAsync(user);

        return true;
    }
}

public class ToggleUserStatusHandler : IRequestHandler<ToggleUserStatusCommand, bool>
{
    private readonly IUserRepository _userRepository;

    public ToggleUserStatusHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<bool> Handle(ToggleUserStatusCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        if (request.CurrentUserRole == "Supervisor" && user.Rol.ToString() == "Administrador")
        {
            throw new UnauthorizedAccessException("Un supervisor no puede modificar un administrador");
        }

        user.Activo = !user.Activo;
        await _userRepository.UpdateAsync(user);

        return true;
    }
}
