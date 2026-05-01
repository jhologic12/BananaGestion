using BananaGestion.Application.Modules.Cosecha.DTOs;
using MediatR;

namespace BananaGestion.Application.Modules.Cosecha.Commands;

public record CreateHarvestCalendarCommand(CreateHarvestCalendarRequest Request) : IRequest<HarvestCalendarDto>;

public record CreateEncinteCommand(CreateEncinteRequest Request) : IRequest<EncinteDto>;

public record CreateCosechaCommand(CreateCosechaRequest Request) : IRequest<HarvestCosechaDto>;

public record UpdateCosechaCommand(Guid Id, UpdateCosechaRequest Request) : IRequest<HarvestCosechaDto>;

public record CreateBoxTypeCommand(CreateBoxTypeRequest Request) : IRequest<BoxTypeDto>;
