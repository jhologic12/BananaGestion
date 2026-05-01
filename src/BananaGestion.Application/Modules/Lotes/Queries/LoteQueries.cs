using BananaGestion.Application.Modules.Lotes.DTOs;
using MediatR;

namespace BananaGestion.Application.Modules.Lotes.Queries;

public record GetLotesQuery : IRequest<IEnumerable<LoteDto>>;

public record GetLoteByIdQuery(Guid Id) : IRequest<LoteDto>;

public record GetActiveLotesQuery : IRequest<IEnumerable<LoteDto>>;
