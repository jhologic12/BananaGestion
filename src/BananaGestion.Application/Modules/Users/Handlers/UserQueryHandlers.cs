using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Application.Modules.Users.DTOs;
using BananaGestion.Application.Modules.Users.Queries;
using BananaGestion.Domain.Enums;
using MediatR;

namespace BananaGestion.Application.Modules.Users.Handlers;

public class GetUsersHandler : IRequestHandler<GetUsersQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetUsersHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetAllAsync();
        return users.Select(u => new UserDto(u.Id, u.Email, u.Nombre, u.Apellido, u.Telefono, 
            u.Rol.ToString(), u.Activo, u.FechaCreacion, u.UltimoLogin));
    }
}

public class GetUserByIdHandler : IRequestHandler<GetUserByIdQuery, UserDto>
{
    private readonly IUserRepository _userRepository;

    public GetUserByIdHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<UserDto> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.Id);
        if (user == null)
        {
            throw new InvalidOperationException("Usuario no encontrado");
        }

        return new UserDto(user.Id, user.Email, user.Nombre, user.Apellido, user.Telefono, 
            user.Rol.ToString(), user.Activo, user.FechaCreacion, user.UltimoLogin);
    }
}

public class GetObrerosHandler : IRequestHandler<GetObrerosQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetObrerosHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetObrerosQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetByRoleAsync("Obrero");
        return users.Select(u => new UserDto(u.Id, u.Email, u.Nombre, u.Apellido, u.Telefono, 
            u.Rol.ToString(), u.Activo, u.FechaCreacion, u.UltimoLogin));
    }
}

public class GetSupervisoresHandler : IRequestHandler<GetSupervisoresQuery, IEnumerable<UserDto>>
{
    private readonly IUserRepository _userRepository;

    public GetSupervisoresHandler(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task<IEnumerable<UserDto>> Handle(GetSupervisoresQuery request, CancellationToken cancellationToken)
    {
        var users = await _userRepository.GetByRoleAsync("Supervisor");
        return users.Select(u => new UserDto(u.Id, u.Email, u.Nombre, u.Apellido, u.Telefono, 
            u.Rol.ToString(), u.Activo, u.FechaCreacion, u.UltimoLogin));
    }
}
