using BananaGestion.Application.Modules.Tareas.DTOs;
using MediatR;

namespace BananaGestion.Application.Modules.Tareas.Commands;

public record CreateTaskConfigCommand(CreateTaskConfigRequest Request) : IRequest<TaskConfigDto>;

public record UpdateTaskConfigCommand(Guid Id, UpdateTaskConfigRequest Request) : IRequest<TaskConfigDto>;

public record DeleteTaskConfigCommand(Guid Id) : IRequest<bool>;

public record CreateTaskAssignmentCommand(CreateTaskAssignmentRequest Request) : IRequest<TaskAssignmentDto>;

public record CreateTaskLogCommand(CreateTaskLogRequest Request, Stream? Foto, Stream? Firma) : IRequest<TaskLogDto>;

public record UpdateAssignmentStatusCommand(Guid Id, string Status) : IRequest<bool>;
