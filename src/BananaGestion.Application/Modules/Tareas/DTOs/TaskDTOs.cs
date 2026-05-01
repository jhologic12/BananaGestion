using BananaGestion.Domain.Enums;

namespace BananaGestion.Application.Modules.Tareas.DTOs;

public record TaskConfigDto(
    Guid Id,
    string Nombre,
    string? Descripcion,
    string TipoLabor,
    string Tipo,
    int? FrecuenciaDias,
    decimal? InsumoCantidad,
    Guid? InsumoId,
    string? InsumoNombre,
    bool RequiereFoto,
    bool RequiereFirma,
    bool RequiereGps,
    bool Activo
);

public record CreateTaskConfigRequest(
    string Nombre,
    string? Descripcion,
    string TipoLabor,
    string Tipo,
    int? FrecuenciaDias,
    decimal? InsumoCantidad,
    Guid? InsumoId,
    bool RequiereFoto,
    bool RequiereFirma,
    bool RequiereGps
);

public record UpdateTaskConfigRequest(
    string? Nombre,
    string? Descripcion,
    string? TipoLabor,
    string? Tipo,
    int? FrecuenciaDias,
    decimal? InsumoCantidad,
    Guid? InsumoId,
    bool? RequiereFoto,
    bool? RequiereFirma,
    bool? RequiereGps,
    bool? Activo
);

public record TaskAssignmentDto(
    Guid Id,
    Guid TaskConfigId,
    string TaskConfigNombre,
    Guid UserId,
    string UserNombre,
    Guid LoteId,
    string LoteNombre,
    DateTime FechaProgramada,
    DateTime? FechaCompletada,
    string Status,
    string? Notas
);

public record CreateTaskAssignmentRequest(
    Guid TaskConfigId,
    Guid UserId,
    Guid LoteId,
    DateTime FechaProgramada,
    string? Notas
);

public record TaskLogDto(
    Guid Id,
    Guid TaskAssignmentId,
    DateTime FechaRegistro,
    string? FotoUrl,
    string? FirmaUrl,
    decimal? Latitud,
    decimal? Longitud,
    string? Observaciones,
    int? CantidadInsumoUtilizado
);

public record CreateTaskLogRequest(
    Guid TaskAssignmentId,
    string? Observaciones,
    int? CantidadInsumoUtilizado,
    decimal? Latitud,
    decimal? Longitud
);
