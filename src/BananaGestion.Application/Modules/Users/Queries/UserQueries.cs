using BananaGestion.Application.Modules.Users.DTOs;
using MediatR;

namespace BananaGestion.Application.Modules.Users.Queries;

public record GetUsersQuery : IRequest<IEnumerable<UserDto>>;

public record GetUserByIdQuery(Guid Id) : IRequest<UserDto>;

public record GetObrerosQuery : IRequest<IEnumerable<UserDto>>;

public record GetSupervisoresQuery : IRequest<IEnumerable<UserDto>>;
