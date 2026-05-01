using BananaGestion.Application.Modules.Users.DTOs;
using BananaGestion.Domain.Entities;
using BananaGestion.Domain.Enums;
using FluentValidation;
using MediatR;

namespace BananaGestion.Application.Modules.Users.Commands;

public record LoginCommand(LoginRequest Request) : IRequest<AuthResponse>;

public record RegisterCommand(RegisterRequest Request) : IRequest<AuthResponse>;

public record UpdateUserCommand(Guid Id, UpdateUserRequest Request) : IRequest<UserDto>;

public record ChangePasswordCommand(Guid Id, ChangePasswordRequest Request) : IRequest<bool>;

public record DeleteUserCommand(Guid Id, string CurrentUserRole) : IRequest<bool>;

public record ToggleUserStatusCommand(Guid Id, string CurrentUserRole) : IRequest<bool>;

public class LoginCommandValidator : AbstractValidator<LoginCommand>
{
    public LoginCommandValidator()
    {
        RuleFor(x => x.Request.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Request.Password).NotEmpty();
    }
}

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(x => x.Request.Email).EmailAddress().NotEmpty();
        RuleFor(x => x.Request.Password)
            .MinimumLength(12)
            .Matches(@"[A-Z]").WithMessage("La contraseña debe contener al menos una letra mayúscula")
            .Matches(@"[a-z]").WithMessage("La contraseña debe contener al menos una letra minúscula")
            .Matches(@"[0-9]").WithMessage("La contraseña debe contener al menos un número")
            .Matches(@"[^a-zA-Z0-9]").WithMessage("La contraseña debe contener al menos un carácter especial");
        RuleFor(x => x.Request.Nombre).NotEmpty();
        RuleFor(x => x.Request.Apellido).NotEmpty();
    }
}
