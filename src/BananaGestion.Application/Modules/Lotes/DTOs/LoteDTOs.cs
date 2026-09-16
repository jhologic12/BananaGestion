namespace BananaGestion.Application.Modules.Lotes.DTOs;

public record LoteDto(
    Guid Id,
    string Codigo,
    string Nombre,
    decimal Hectareas,
    string? Ubicacion,
    decimal? Latitud,
    decimal? Longitud,
    string? GeojsonPolygon,
    bool Activo,
    DateTime FechaCreacion,
    string? Notas
);

public record CreateLoteRequest(
    string Codigo,
    string Nombre,
    decimal? Hectareas,
    string? Ubicacion,
    decimal? Latitud,
    decimal? Longitud,
    string? GeojsonPolygon,
    string? Notas
);

public record UpdateLoteRequest(
    string? Nombre,
    decimal? Hectareas,
    string? Ubicacion,
    decimal? Latitud,
    decimal? Longitud,
    string? GeojsonPolygon,
    bool? Activo,
    string? Notas
);
