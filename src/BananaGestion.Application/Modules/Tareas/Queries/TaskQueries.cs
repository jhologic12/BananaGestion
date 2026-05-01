using BananaGestion.Application.Modules.Tareas.DTOs;
using MediatR;

namespace BananaGestion.Application.Modules.Tareas.Queries;

public record GetTaskConfigsQuery : IRequest<IEnumerable<TaskConfigDto>>;

public record GetTaskConfigByIdQuery(Guid Id) : IRequest<TaskConfigDto>;

public record GetPendingAssignmentsQuery(Guid? UserId = null) : IRequest<IEnumerable<TaskAssignmentDto>>;

public record GetAssignmentsByDateRangeQuery(DateTime Start, DateTime End) : IRequest<IEnumerable<TaskAssignmentDto>>;

public record GetAssignmentWithDetailsQuery(Guid Id) : IRequest<TaskAssignmentDto>;

public record GetOverdueAssignmentsQuery : IRequest<IEnumerable<TaskAssignmentDto>>;

public record GetTaskLogQuery(Guid AssignmentId) : IRequest<TaskLogDto?>;
