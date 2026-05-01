namespace BananaGestion.Application.Modules.Lotes.DTOs;

public record LoteDto(
    Guid Id,
    string Codigo,
    string Nombre,
    decimal Hectareas,
    string? Ubicacion,
    decimal? Latitud,
    decimal? Longitud,
    bool Activo,
    DateTime FechaCreacion,
    string? Notas
);

public record CreateLoteRequest(
    string Codigo,
    string Nombre,
    decimal Hectareas,
    string? Ubicacion,
    decimal? Latitud,
    decimal? Longitud,
    string? Notas
);

public record UpdateLoteRequest(
    string? Nombre,
    decimal? Hectareas,
    string? Ubicacion,
    decimal? Latitud,
    decimal? Longitud,
    bool? Activo,
    string? Notas
);
