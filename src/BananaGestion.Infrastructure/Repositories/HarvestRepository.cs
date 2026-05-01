using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Domain.Entities;
using BananaGestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BananaGestion.Infrastructure.Repositories;

public class HarvestRepository : IHarvestRepository
{
    private readonly BananaDbContext _context;

    public HarvestRepository(BananaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<HarvestCalendar>> GetCalendarAsync(int year)
    {
        return await _context.HarvestCalendars
            .Where(c => c.Ano == year)
            .OrderBy(c => c.Semana)
            .ToListAsync();
    }

    public async Task<HarvestCalendar?> GetCalendarByWeekAsync(int week, int year)
    {
        return await _context.HarvestCalendars
            .FirstOrDefaultAsync(c => c.Semana == week && c.Ano == year);
    }

    public async Task<HarvestCalendar> CreateCalendarAsync(HarvestCalendar calendar)
    {
        await _context.HarvestCalendars.AddAsync(calendar);
        await _context.SaveChangesAsync();
        return calendar;
    }

    public async Task<IEnumerable<HarvestRecord>> GetEncinteByYearAsync(int year)
    {
        return await _context.HarvestRecords
            .Include(r => r.Lote)
            .Include(r => r.User)
            .Where(r => r.AnoEncinte == year)
            .OrderBy(r => r.SemanaEncinte)
            .ThenBy(r => r.Lote.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<HarvestRecord>> GetEncinteByWeekAsync(int semana, int ano)
    {
        return await _context.HarvestRecords
            .Include(r => r.Lote)
            .Include(r => r.User)
            .Where(r => r.SemanaEncinte == semana && r.AnoEncinte == ano)
            .OrderBy(r => r.Lote.Nombre)
            .ToListAsync();
    }

    public async Task<HarvestRecord?> GetRecordWithDetailsAsync(Guid id)
    {
        return await _context.HarvestRecords
            .Include(r => r.Lote)
            .Include(r => r.User)
            .Include(r => r.HarvestCalendar)
            .Include(r => r.BoxRecords)
            .ThenInclude(b => b.BoxType)
            .FirstOrDefaultAsync(r => r.Id == id);
    }

    public async Task<HarvestRecord> CreateEncinteAsync(HarvestRecord record)
    {
        await _context.HarvestRecords.AddAsync(record);
        await _context.SaveChangesAsync();
        return record;
    }

    public async Task UpdateEncinteAsync(HarvestRecord record)
    {
        _context.HarvestRecords.Update(record);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<HarvestCosecha>> GetCosechasByYearAsync(int year)
    {
        return await _context.HarvestCosechas
            .Include(c => c.Lote)
            .Include(c => c.User)
            .Where(c => c.AnoCosecha == year)
            .OrderBy(c => c.SemanaEncinte)
            .ThenBy(c => c.SemanaCosecha)
            .ThenBy(c => c.Lote.Nombre)
            .ToListAsync();
    }

    public async Task<IEnumerable<HarvestCosecha>> GetCosechasByEncinteWeekAsync(int semanaEncinte, int anoEncinte)
    {
        return await _context.HarvestCosechas
            .Include(c => c.Lote)
            .Include(c => c.User)
            .Where(c => c.SemanaEncinte == semanaEncinte && c.AnoEncinte == anoEncinte)
            .OrderBy(c => c.Lote.Nombre)
            .ToListAsync();
    }

    public async Task<HarvestCosecha?> GetCosechaByIdAsync(Guid id)
    {
        return await _context.HarvestCosechas
            .Include(c => c.Lote)
            .Include(c => c.User)
            .Include(c => c.HarvestCalendar)
            .FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<HarvestCosecha> CreateCosechaAsync(HarvestCosecha cosecha)
    {
        await _context.HarvestCosechas.AddAsync(cosecha);
        await _context.SaveChangesAsync();
        return cosecha;
    }

    public async Task UpdateCosechaAsync(HarvestCosecha cosecha)
    {
        _context.HarvestCosechas.Update(cosecha);
        await _context.SaveChangesAsync();
    }

    public async Task<IEnumerable<BoxType>> GetBoxTypesAsync()
    {
        return await _context.BoxTypes.Where(b => b.Activo).ToListAsync();
    }

    public async Task<BoxType?> GetBoxTypeByCodeAsync(string code)
    {
        return await _context.BoxTypes.FirstOrDefaultAsync(b => b.Codigo == code);
    }

    public async Task<BoxType> CreateBoxTypeAsync(BoxType boxType)
    {
        await _context.BoxTypes.AddAsync(boxType);
        await _context.SaveChangesAsync();
        return boxType;
    }

    public async Task<IEnumerable<HarvestOrder>> GetOrdersAsync()
    {
        return await _context.HarvestOrders
            .Include(o => o.BoxDetails)
            .ThenInclude(d => d.BoxType)
            .Include(o => o.HarvestCalendar)
            .OrderByDescending(o => o.FechaEmbarque)
            .ToListAsync();
    }

    public async Task<HarvestOrder?> GetOrderWithDetailsAsync(Guid id)
    {
        return await _context.HarvestOrders
            .Include(o => o.BoxDetails)
            .ThenInclude(d => d.BoxType)
            .Include(o => o.HarvestCalendar)
            .FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task<HarvestOrder> CreateOrderAsync(HarvestOrder order)
    {
        await _context.HarvestOrders.AddAsync(order);
        await _context.SaveChangesAsync();
        return order;
    }

    public async Task UpdateOrderAsync(HarvestOrder order)
    {
        _context.HarvestOrders.Update(order);
        await _context.SaveChangesAsync();
    }

    public async Task<int> GetTotalRacimosByPeriodAsync(DateTime start, DateTime end)
    {
        var startUtc = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(end, DateTimeKind.Utc);
        
        return await _context.HarvestRecords
            .Where(r => r.Fecha >= startUtc && r.Fecha <= endUtc)
            .SumAsync(r => r.CantidadRacimosEmbolsados);
    }

    public async Task<Dictionary<string, int>> GetProjectionByWeeksAsync(int weeksAhead)
    {
        var projections = new Dictionary<string, int>();
        var today = DateTime.UtcNow.Date;
        var currentWeek = System.Globalization.ISOWeek.GetWeekOfYear(today);

        for (int i = 0; i < weeksAhead; i++)
        {
            var projectedWeek = (currentWeek + i) % 52;
            if (projectedWeek == 0) projectedWeek = 52;
            
            var weekStart = today.AddDays(-7 * (i + 1));
            var weekEnd = today.AddDays(-7 * i);
            
            var records = await _context.HarvestRecords
                .Where(r => r.Fecha >= weekStart && r.Fecha < weekEnd)
                .Select(r => r.CantidadRacimosEmbolsados)
                .ToListAsync();
            
            var avgRacimos = records.Count > 0 ? records.Average() : 0;
            projections[$"semana-{projectedWeek}"] = (int)avgRacimos;
        }

        return projections;
    }
}
