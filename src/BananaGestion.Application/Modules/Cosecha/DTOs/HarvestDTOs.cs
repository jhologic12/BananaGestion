namespace BananaGestion.Application.Modules.Cosecha.DTOs;

public record HarvestCalendarDto(
    Guid Id,
    int Semana,
    int Ano,
    string ColorCinta,
    string? ColorNombre,
    DateTime FechaInicio,
    DateTime FechaFin,
    bool Activo
);

public record EncinteDto(
    Guid Id,
    Guid LoteId,
    string LoteNombre,
    Guid UserId,
    string UserNombre,
    int SemanaEncinte,
    int AnoEncinte,
    int CantidadRacimosEmbolsados,
    string ColorCinta,
    DateTime Fecha,
    string? Notas
);

public record HarvestCosechaDto(
    Guid Id,
    Guid LoteId,
    string LoteNombre,
    Guid UserId,
    string UserNombre,
    int SemanaEncinte,
    int AnoEncinte,
    int SemanaCosecha,
    int AnoCosecha,
    string Estado,
    int CantidadRacimos,
    string ColorCinta,
    DateTime Fecha,
    string? Notas
);

public record SemanaProyeccionDto(
    int SemanaEncinte,
    int AnoEncinte,
    int Encintados,
    int Semitallo,
    int Cortados,
    int Pendientes,
    int SemanaProyeccionMin,
    int SemanaProyeccionMax,
    bool IsBarrido,
    string ColorCinta,
    string? ColorNombre
);

public record BoxTypeDto(
    Guid Id,
    string Codigo,
    string Descripcion,
    int? CapacidadKilos,
    bool Activo
);

public record HarvestOrderDto(
    Guid Id,
    string NumeroOrden,
    int SemanaEmbarque,
    DateTime FechaEmbarque,
    string? PdfUrl,
    string? Cliente,
    string? Notas,
    bool Procesada,
    List<OrderBoxDetailDto> BoxDetails
);

public record OrderBoxDetailDto(
    Guid Id,
    Guid BoxTypeId,
    string BoxTypeCodigo,
    int CantidadPlanificada,
    int CantidadEmpacada,
    string? Notas
);

public record CreateHarvestCalendarRequest(int Semana, int Ano, string ColorCinta, DateTime FechaInicio, DateTime FechaFin);

public record CreateEncinteRequest(
    Guid LoteId,
    int SemanaEncinte,
    int AnoEncinte,
    int CantidadRacimosEmbolsados,
    string ColorCinta,
    DateTime Fecha,
    string? Notas,
    Guid? UserId
);

public record CreateCosechaRequest(
    Guid LoteId,
    int SemanaEncinte,
    int AnoEncinte,
    int SemanaCosecha,
    int AnoCosecha,
    string Estado,
    int CantidadRacimos,
    string ColorCinta,
    DateTime Fecha,
    string? Notas
);

public record UpdateCosechaRequest(
    int CantidadRacimos,
    string? Notas
);

public record CreateBoxTypeRequest(string Codigo, string Descripcion, int? CapacidadKilos);

public record CreateHarvestOrderRequest(
    string NumeroOrden,
    int SemanaEmbarque,
    DateTime FechaEmbarque,
    string? Cliente,
    string? PdfUrl,
    List<CreateOrderBoxDetailRequest>? BoxDetails
);

public record CreateOrderBoxDetailRequest(Guid BoxTypeId, int CantidadPlanificada);
