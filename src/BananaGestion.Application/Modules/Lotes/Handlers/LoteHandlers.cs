using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Application.Modules.Lotes.Commands;
using BananaGestion.Application.Modules.Lotes.DTOs;
using BananaGestion.Application.Modules.Lotes.Queries;
using BananaGestion.Domain.Entities;
using MediatR;

namespace BananaGestion.Application.Modules.Lotes.Handlers;

public class LoteHandlers :
    IRequestHandler<GetLotesQuery, IEnumerable<LoteDto>>,
    IRequestHandler<GetLoteByIdQuery, LoteDto>,
    IRequestHandler<GetActiveLotesQuery, IEnumerable<LoteDto>>,
    IRequestHandler<CreateLoteCommand, LoteDto>,
    IRequestHandler<UpdateLoteCommand, LoteDto>,
    IRequestHandler<DeleteLoteCommand, bool>
{
    private readonly IRepository<Lote> _loteRepository;

    public LoteHandlers(IRepository<Lote> loteRepository)
    {
        _loteRepository = loteRepository;
    }

    public async Task<IEnumerable<LoteDto>> Handle(GetLotesQuery request, CancellationToken cancellationToken)
    {
        var lotes = await _loteRepository.GetAllAsync();
        return lotes.Select(MapToDto);
    }

    public async Task<LoteDto> Handle(GetLoteByIdQuery request, CancellationToken cancellationToken)
    {
        var lote = await _loteRepository.GetByIdAsync(request.Id);
        if (lote == null)
            throw new InvalidOperationException("Lote no encontrado");
        return MapToDto(lote);
    }

    public async Task<IEnumerable<LoteDto>> Handle(GetActiveLotesQuery request, CancellationToken cancellationToken)
    {
        var lotes = await _loteRepository.FindAsync(l => l.Activo);
        return lotes.Select(MapToDto);
    }

    public async Task<LoteDto> Handle(CreateLoteCommand request, CancellationToken cancellationToken)
    {
        var existing = (await _loteRepository.FindAsync(l => l.Codigo == request.Request.Codigo)).FirstOrDefault();
        if (existing != null)
            throw new InvalidOperationException("Ya existe un lote con este código");

        var lote = new Lote
        {
            Codigo = request.Request.Codigo,
            Nombre = request.Request.Nombre,
            Hectareas = request.Request.Hectareas,
            Ubicacion = request.Request.Ubicacion,
            Latitud = request.Request.Latitud,
            Longitud = request.Request.Longitud,
            Notas = request.Request.Notas
        };

        await _loteRepository.AddAsync(lote);
        return MapToDto(lote);
    }

    public async Task<LoteDto> Handle(UpdateLoteCommand request, CancellationToken cancellationToken)
    {
        var lote = await _loteRepository.GetByIdAsync(request.Id);
        if (lote == null)
            throw new InvalidOperationException("Lote no encontrado");

        if (!string.IsNullOrEmpty(request.Request.Nombre))
            lote.Nombre = request.Request.Nombre;
        if (request.Request.Hectareas.HasValue)
            lote.Hectareas = request.Request.Hectareas.Value;
        if (!string.IsNullOrEmpty(request.Request.Ubicacion))
            lote.Ubicacion = request.Request.Ubicacion;
        if (request.Request.Latitud.HasValue)
            lote.Latitud = request.Request.Latitud;
        if (request.Request.Longitud.HasValue)
            lote.Longitud = request.Request.Longitud;
        if (request.Request.Activo.HasValue)
            lote.Activo = request.Request.Activo.Value;
        if (request.Request.Notas != null)
            lote.Notas = request.Request.Notas;

        await _loteRepository.UpdateAsync(lote);
        return MapToDto(lote);
    }

    public async Task<bool> Handle(DeleteLoteCommand request, CancellationToken cancellationToken)
    {
        await _loteRepository.DeleteAsync(request.Id);
        return true;
    }

    private static LoteDto MapToDto(Lote lote) => new(
        lote.Id, lote.Codigo, lote.Nombre, lote.Hectareas, lote.Ubicacion,
        lote.Latitud, lote.Longitud, lote.Activo, lote.FechaCreacion, lote.Notas
    );
}
