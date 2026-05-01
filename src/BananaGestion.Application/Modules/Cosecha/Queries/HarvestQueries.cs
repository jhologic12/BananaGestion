using BananaGestion.Application.Modules.Cosecha.DTOs;
using MediatR;

namespace BananaGestion.Application.Modules.Cosecha.Queries;

public record GetHarvestCalendarQuery(int Year) : IRequest<IEnumerable<HarvestCalendarDto>>;

public record GetHarvestCalendarByWeekQuery(int Week, int Year) : IRequest<HarvestCalendarDto?>;

public record GetEncinteByYearQuery(int Year) : IRequest<IEnumerable<EncinteDto>>;

public record GetEncinteByWeekQuery(int Semana, int Ano) : IRequest<IEnumerable<EncinteDto>>;

public record GetCosechasByYearQuery(int Year) : IRequest<IEnumerable<HarvestCosechaDto>>;

public record GetCosechasByEncinteWeekQuery(int SemanaEncinte, int AnoEncinte) : IRequest<IEnumerable<HarvestCosechaDto>>;

public record GetProyeccionQuery(int? Year = null) : IRequest<IEnumerable<SemanaProyeccionDto>>;

public record GetBoxTypesQuery : IRequest<IEnumerable<BoxTypeDto>>;
