using System.Text.Json;
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

        (var latitud, var longitud) = DeriveCentroid(request.Request.GeojsonPolygon);

        var lote = new Lote
        {
            Codigo = request.Request.Codigo,
            Nombre = request.Request.Nombre,
            Hectareas = request.Request.Hectareas ?? CalculateAreaFromGeojson(request.Request.GeojsonPolygon),
            Ubicacion = request.Request.Ubicacion,
            Latitud = request.Request.Latitud ?? latitud,
            Longitud = request.Request.Longitud ?? longitud,
            GeojsonPolygon = request.Request.GeojsonPolygon,
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
        if (request.Request.GeojsonPolygon != null)
            lote.GeojsonPolygon = request.Request.GeojsonPolygon;
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
        lote.Latitud, lote.Longitud, lote.GeojsonPolygon, lote.Activo,
        lote.FechaCreacion, lote.Notas
    );

    private static (decimal? lat, decimal? lng) DeriveCentroid(string? geojson)
    {
        if (string.IsNullOrWhiteSpace(geojson)) return (null, null);

        try
        {
            using var doc = JsonDocument.Parse(geojson);
            var coords = doc.RootElement.GetProperty("coordinates")[0];
            var sumLat = 0.0;
            var sumLng = 0.0;
            var count = 0;

            foreach (var coord in coords.EnumerateArray())
            {
                sumLng += coord[0].GetDouble();
                sumLat += coord[1].GetDouble();
                count++;
            }

            if (count == 0) return (null, null);
            return ((decimal)(sumLat / count), (decimal)(sumLng / count));
        }
        catch
        {
            return (null, null);
        }
    }

    private static decimal CalculateAreaFromGeojson(string? geojson)
    {
        if (string.IsNullOrWhiteSpace(geojson)) return 0;

        try
        {
            using var doc = JsonDocument.Parse(geojson);
            var coords = doc.RootElement.GetProperty("coordinates")[0];

            var latLngs = new List<(double lat, double lng)>();
            foreach (var coord in coords.EnumerateArray())
            {
                latLngs.Add((coord[1].GetDouble(), coord[0].GetDouble()));
            }

            if (latLngs.Count < 3) return 0;

            var centerLat = latLngs.Average(p => p.lat) * Math.PI / 180.0;
            const double earthRadius = 6378137.0;
            const double hectaresDivisor = 10000.0;

            var projected = latLngs.Select(p => (
                x: p.lng * Math.PI / 180.0 * earthRadius * Math.Cos(centerLat),
                y: p.lat * Math.PI / 180.0 * earthRadius
            )).ToArray();

            var area = 0.0;
            var n = projected.Length;
            for (int i = 0; i < n; i++)
            {
                int j = (i + 1) % n;
                area += projected[i].x * projected[j].y;
                area -= projected[j].x * projected[i].y;
            }
            area = Math.Abs(area) / 2.0;

            return (decimal)(area / hectaresDivisor);
        }
        catch
        {
            return 0;
        }
    }
}
