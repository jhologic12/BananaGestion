using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Application.Modules.Cosecha.Commands;
using BananaGestion.Application.Modules.Cosecha.DTOs;
using BananaGestion.Application.Modules.Cosecha.Queries;
using BananaGestion.Domain.Entities;
using BananaGestion.Domain.Enums;
using MediatR;

namespace BananaGestion.Application.Modules.Cosecha.Handlers;

public class HarvestHandlers :
    IRequestHandler<GetHarvestCalendarQuery, IEnumerable<HarvestCalendarDto>>,
    IRequestHandler<GetHarvestCalendarByWeekQuery, HarvestCalendarDto?>,
    IRequestHandler<GetEncinteByYearQuery, IEnumerable<EncinteDto>>,
    IRequestHandler<GetEncinteByWeekQuery, IEnumerable<EncinteDto>>,
    IRequestHandler<CreateEncinteCommand, EncinteDto>,
    IRequestHandler<GetCosechasByYearQuery, IEnumerable<HarvestCosechaDto>>,
    IRequestHandler<GetCosechasByEncinteWeekQuery, IEnumerable<HarvestCosechaDto>>,
    IRequestHandler<CreateCosechaCommand, HarvestCosechaDto>,
    IRequestHandler<UpdateCosechaCommand, HarvestCosechaDto>,
    IRequestHandler<GetProyeccionQuery, IEnumerable<SemanaProyeccionDto>>,
    IRequestHandler<GetBoxTypesQuery, IEnumerable<BoxTypeDto>>,
    IRequestHandler<CreateBoxTypeCommand, BoxTypeDto>
{
    private readonly IHarvestRepository _repository;

    public HarvestHandlers(IHarvestRepository repository)
    {
        _repository = repository;
    }

    private static readonly Dictionary<int, (string Color, string HexColor, string? Nombre)> WeekColors = new()
    {
        { 1, ("#00FF00", "#00FF00", "Verde") }, { 2, ("#FFFF00", "#FFFF00", "Amarillo") }, { 3, ("#FFFFFF", "#FFFFFF", "Blanco") }, { 4, ("#0000FF", "#0000FF", "Azul") },
        { 5, ("#FF0000", "#FF0000", "Rojo") }, { 6, ("#8B4513", "#8B4513", "Café") }, { 7, ("#000000", "#000000", "Negro") }, { 8, ("#FFA500", "#FFA500", "Naranja") },
        { 9, ("#00FF00", "#00FF00", "Verde") }, { 10, ("#FFFF00", "#FFFF00", "Amarillo") }, { 11, ("#FFFFFF", "#FFFFFF", "Blanco") }, { 12, ("#0000FF", "#0000FF", "Azul") },
        { 13, ("#FF0000", "#FF0000", "Rojo") }, { 14, ("#8B4513", "#8B4513", "Café") }, { 15, ("#000000", "#000000", "Negro") }, { 16, ("#FFA500", "#FFA500", "Naranja") },
        { 17, ("#00FF00", "#00FF00", "Verde") }, { 18, ("#FFFF00", "#FFFF00", "Amarillo") }, { 19, ("#FFFFFF", "#FFFFFF", "Blanco") }, { 20, ("#0000FF", "#0000FF", "Azul") },
        { 21, ("#FF0000", "#FF0000", "Rojo") }, { 22, ("#8B4513", "#8B4513", "Café") }, { 23, ("#000000", "#000000", "Negro") }, { 24, ("#FFA500", "#FFA500", "Naranja") },
        { 25, ("#00FF00", "#00FF00", "Verde") }, { 26, ("#FFFF00", "#FFFF00", "Amarillo") }, { 27, ("#FFFFFF", "#FFFFFF", "Blanco") }, { 28, ("#0000FF", "#0000FF", "Azul") },
        { 29, ("#FF0000", "#FF0000", "Rojo") }, { 30, ("#8B4513", "#8B4513", "Café") }, { 31, ("#000000", "#000000", "Negro") }, { 32, ("#FFA500", "#FFA500", "Naranja") },
        { 33, ("#00FF00", "#00FF00", "Verde") }, { 34, ("#FFFF00", "#FFFF00", "Amarillo") }, { 35, ("#FFFFFF", "#FFFFFF", "Blanco") }, { 36, ("#0000FF", "#0000FF", "Azul") },
        { 37, ("#FF0000", "#FF0000", "Rojo") }, { 38, ("#8B4513", "#8B4513", "Café") }, { 39, ("#000000", "#000000", "Negro") }, { 40, ("#FFA500", "#FFA500", "Naranja") },
        { 41, ("#00FF00", "#00FF00", "Verde") }, { 42, ("#FFFF00", "#FFFF00", "Amarillo") }, { 43, ("#FFFFFF", "#FFFFFF", "Blanco") }, { 44, ("#0000FF", "#0000FF", "Azul") },
        { 45, ("#FF0000", "#FF0000", "Rojo") }, { 46, ("#8B4513", "#8B4513", "Café") }, { 47, ("#000000", "#000000", "Negro") }, { 48, ("#FFA500", "#FFA500", "Naranja") },
        { 49, ("#00FF00", "#00FF00", "Verde") }, { 50, ("#FFFF00", "#FFFF00", "Amarillo") }, { 51, ("#FFFFFF", "#FFFFFF", "Blanco") }, { 52, ("#0000FF", "#0000FF", "Azul") },
    };

    public async Task<IEnumerable<HarvestCalendarDto>> Handle(GetHarvestCalendarQuery request, CancellationToken cancellationToken)
    {
        var calendars = await _repository.GetCalendarAsync(request.Year);
        Console.WriteLine($"GetHarvestCalendarQuery: Found {calendars.Count()} records for year {request.Year}");
        
        if (!calendars.Any())
        {
            Console.WriteLine($"No calendar records found for {request.Year}, seeding...");
            calendars = new List<HarvestCalendar>();
            
            for (int week = 1; week <= 52; week++)
            {
                var weekStart = GetDateForWeek(request.Year, week, DayOfWeek.Monday);
                var weekEnd = weekStart.AddDays(6);
                var (hexColor, _, colorName) = WeekColors[week];
                
                var calendar = new HarvestCalendar
                {
                    Id = Guid.NewGuid(),
                    Semana = week,
                    Ano = request.Year,
                    ColorCinta = hexColor,
                    ColorNombre = colorName,
                    FechaInicio = weekStart,
                    FechaFin = weekEnd,
                    Activo = true
                };
                
                await _repository.CreateCalendarAsync(calendar);
                ((List<HarvestCalendar>)calendars).Add(calendar);
            }
        }
        
        return calendars.Select(MapCalendarToDto);
    }

    public async Task<HarvestCalendarDto?> Handle(GetHarvestCalendarByWeekQuery request, CancellationToken cancellationToken)
    {
        var calendar = await _repository.GetCalendarByWeekAsync(request.Week, request.Year);
        
        if (calendar == null)
        {
            var (hexColor, _, colorName) = WeekColors[request.Week];
            var weekStart = GetDateForWeek(request.Year, request.Week, DayOfWeek.Monday);
            var weekEnd = weekStart.AddDays(6);
            
            calendar = await _repository.CreateCalendarAsync(new HarvestCalendar
            {
                Id = Guid.NewGuid(),
                Semana = request.Week,
                Ano = request.Year,
                ColorCinta = hexColor,
                ColorNombre = colorName,
                FechaInicio = weekStart,
                FechaFin = weekEnd,
                Activo = true
            });
        }
        
        return MapCalendarToDto(calendar);
    }

    public async Task<IEnumerable<EncinteDto>> Handle(GetEncinteByYearQuery request, CancellationToken cancellationToken)
    {
        var records = await _repository.GetEncinteByYearAsync(request.Year);
        return records.Select(MapEncinteToDto);
    }

    public async Task<IEnumerable<EncinteDto>> Handle(GetEncinteByWeekQuery request, CancellationToken cancellationToken)
    {
        var records = await _repository.GetEncinteByWeekAsync(request.Semana, request.Ano);
        return records.Select(MapEncinteToDto);
    }

    public async Task<EncinteDto> Handle(CreateEncinteCommand request, CancellationToken cancellationToken)
    {
        if (request.Request.UserId == null || request.Request.UserId == Guid.Empty)
        {
            throw new InvalidOperationException("No se pudo identificar el usuario que registra el encinte");
        }

        var record = new HarvestRecord
        {
            LoteId = request.Request.LoteId,
            UserId = request.Request.UserId.Value,
            SemanaEncinte = request.Request.SemanaEncinte,
            AnoEncinte = request.Request.AnoEncinte,
            CantidadRacimosEmbolsados = request.Request.CantidadRacimosEmbolsados,
            ColorCinta = request.Request.ColorCinta,
            Fecha = request.Request.Fecha,
            Notas = request.Request.Notas
        };

        var created = await _repository.CreateEncinteAsync(record);
        var fullRecord = await _repository.GetRecordWithDetailsAsync(created.Id);
        
        return MapEncinteToDto(fullRecord!);
    }

    public async Task<IEnumerable<HarvestCosechaDto>> Handle(GetCosechasByYearQuery request, CancellationToken cancellationToken)
    {
        var cosechas = await _repository.GetCosechasByYearAsync(request.Year);
        return cosechas.Select(MapCosechaToDto);
    }

    public async Task<IEnumerable<HarvestCosechaDto>> Handle(GetCosechasByEncinteWeekQuery request, CancellationToken cancellationToken)
    {
        var cosechas = await _repository.GetCosechasByEncinteWeekAsync(request.SemanaEncinte, request.AnoEncinte);
        return cosechas.Select(MapCosechaToDto);
    }

    public async Task<HarvestCosechaDto> Handle(CreateCosechaCommand request, CancellationToken cancellationToken)
    {
        var encintes = await _repository.GetEncinteByWeekAsync(request.Request.SemanaEncinte, request.Request.AnoEncinte);
        var cosechas = await _repository.GetCosechasByEncinteWeekAsync(request.Request.SemanaEncinte, request.Request.AnoEncinte);

        var totalEncintados = encintes.Sum(e => e.CantidadRacimosEmbolsados);
        var totalSemitallo = cosechas.Where(c => c.Estado == HarvestState.Semitallo).Sum(c => c.CantidadRacimos);
        var totalCortados = cosechas.Where(c => c.Estado == HarvestState.Cortado).Sum(c => c.CantidadRacimos);
        var disponibles = totalEncintados - totalSemitallo - totalCortados;

        if (request.Request.CantidadRacimos <= 0)
        {
            throw new InvalidOperationException("La cantidad de racimos debe ser mayor a cero");
        }

        if (request.Request.CantidadRacimos > disponibles)
        {
            throw new InvalidOperationException(
                $"No hay suficientes racimos para {request.Request.Estado.ToLower()}. " +
                $"Disponibles para la semana {request.Request.SemanaEncinte}: {disponibles}. " +
                $"Encintados: {totalEncintados}, Semitallo: {totalSemitallo}, Cortados: {totalCortados}");
        }

        var cosecha = new HarvestCosecha
        {
            LoteId = request.Request.LoteId,
            SemanaEncinte = request.Request.SemanaEncinte,
            AnoEncinte = request.Request.AnoEncinte,
            SemanaCosecha = request.Request.SemanaCosecha,
            AnoCosecha = request.Request.AnoCosecha,
            Estado = Enum.Parse<HarvestState>(request.Request.Estado),
            CantidadRacimos = request.Request.CantidadRacimos,
            ColorCinta = request.Request.ColorCinta,
            Fecha = request.Request.Fecha,
            Notas = request.Request.Notas
        };

        var created = await _repository.CreateCosechaAsync(cosecha);
        var fullCosecha = await _repository.GetCosechaByIdAsync(created.Id);
        
        return MapCosechaToDto(fullCosecha!);
    }

    public async Task<HarvestCosechaDto> Handle(UpdateCosechaCommand request, CancellationToken cancellationToken)
    {
        var cosecha = await _repository.GetCosechaByIdAsync(request.Id);
        if (cosecha == null)
            throw new InvalidOperationException("Registro de cosecha no encontrado");

        cosecha.CantidadRacimos = request.Request.CantidadRacimos;
        cosecha.Notas = request.Request.Notas;

        await _repository.UpdateCosechaAsync(cosecha);
        
        return MapCosechaToDto(cosecha);
    }

    public async Task<IEnumerable<SemanaProyeccionDto>> Handle(GetProyeccionQuery request, CancellationToken cancellationToken)
    {
        var year = request.Year ?? DateTime.UtcNow.Year;
        var encintes = (await _repository.GetEncinteByYearAsync(year)).ToList();
        var cosechas = (await _repository.GetCosechasByYearAsync(year)).ToList();
        var calendars = (await _repository.GetCalendarAsync(year)).ToDictionary(c => c.Semana);

        var result = new List<SemanaProyeccionDto>();
        var semanasEncinte = encintes.GroupBy(e => new { e.SemanaEncinte, e.AnoEncinte });

        foreach (var grupo in semanasEncinte)
        {
            var semanaEncinte = grupo.Key.SemanaEncinte;
            var anoEncinte = grupo.Key.AnoEncinte;
            var encintados = grupo.Sum(e => e.CantidadRacimosEmbolsados);
            
            var cosechasSemana = cosechas.Where(c => c.SemanaEncinte == semanaEncinte && c.AnoEncinte == anoEncinte);
            var semitallo = cosechasSemana.Where(c => c.Estado == HarvestState.Semitallo).Sum(c => c.CantidadRacimos);
            var cortados = cosechasSemana.Where(c => c.Estado == HarvestState.Cortado).Sum(c => c.CantidadRacimos);
            var pendientes = encintados - semitallo - cortados;
            
            var semanaProyeccionMin = semanaEncinte + 11;
            var semanaProyeccionMax = semanaEncinte + 13;
            var isBarrido = semanaProyeccionMin > 52 && pendientes > 0;

            calendars.TryGetValue(semanaEncinte, out var calendar);
            var colorNombre = calendar?.ColorNombre ?? WeekColors.GetValueOrDefault(semanaEncinte).Nombre;

            result.Add(new SemanaProyeccionDto(
                semanaEncinte,
                anoEncinte,
                encintados,
                semitallo,
                cortados,
                pendientes,
                semanaProyeccionMin,
                semanaProyeccionMax,
                isBarrido,
                calendar?.ColorCinta ?? WeekColors.GetValueOrDefault(semanaEncinte).Color,
                colorNombre
            ));
        }

        return result.OrderByDescending(r => r.AnoEncinte).ThenByDescending(r => r.SemanaEncinte);
    }

    public async Task<IEnumerable<BoxTypeDto>> Handle(GetBoxTypesQuery request, CancellationToken cancellationToken)
    {
        var types = await _repository.GetBoxTypesAsync();
        return types.Select(MapBoxTypeToDto);
    }

    public async Task<BoxTypeDto> Handle(CreateBoxTypeCommand request, CancellationToken cancellationToken)
    {
        var existing = await _repository.GetBoxTypeByCodeAsync(request.Request.Codigo);
        if (existing != null)
            throw new InvalidOperationException("Ya existe un tipo de caja con este código");

        var boxType = new BoxType
        {
            Codigo = request.Request.Codigo,
            Descripcion = request.Request.Descripcion,
            CapacidadKilos = request.Request.CapacidadKilos
        };

        await _repository.CreateBoxTypeAsync(boxType);
        return MapBoxTypeToDto(boxType);
    }

    private static DateTime GetDateForWeek(int year, int week, DayOfWeek dayOfWeek)
    {
        var jan1 = new DateTime(year, 1, 1);
        var daysOffset = (int)dayOfWeek - (int)jan1.DayOfWeek;
        if (daysOffset < 0) daysOffset += 7;
        var firstMonday = jan1.AddDays(daysOffset);
        return DateTime.SpecifyKind(firstMonday.AddDays((week - 1) * 7), DateTimeKind.Utc);
    }

    private static HarvestCalendarDto MapCalendarToDto(HarvestCalendar c) => new(
        c.Id, c.Semana, c.Ano, c.ColorCinta, c.ColorNombre, c.FechaInicio, c.FechaFin, c.Activo
    );

    private static EncinteDto MapEncinteToDto(HarvestRecord r) => new(
        r.Id, r.LoteId, r.Lote.Nombre, r.UserId, $"{r.User.Nombre} {r.User.Apellido}",
        r.SemanaEncinte, r.AnoEncinte, r.CantidadRacimosEmbolsados, r.ColorCinta, r.Fecha, r.Notas
    );

    private static HarvestCosechaDto MapCosechaToDto(HarvestCosecha c) => new(
        c.Id, c.LoteId, c.Lote.Nombre, c.UserId ?? Guid.Empty, c.User != null ? $"{c.User.Nombre} {c.User.Apellido}" : "",
        c.SemanaEncinte, c.AnoEncinte, c.SemanaCosecha, c.AnoCosecha,
        c.Estado.ToString(), c.CantidadRacimos, c.ColorCinta, c.Fecha, c.Notas
    );

    private static BoxTypeDto MapBoxTypeToDto(BoxType b) => new(
        b.Id, b.Codigo, b.Descripcion, b.CapacidadKilos, b.Activo
    );
}
