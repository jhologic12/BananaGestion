using BananaGestion.Domain.Entities;

namespace BananaGestion.Application.Common.Interfaces;

public interface IHarvestRepository
{
    Task<IEnumerable<HarvestCalendar>> GetCalendarAsync(int year);
    Task<HarvestCalendar?> GetCalendarByWeekAsync(int week, int year);
    Task<HarvestCalendar> CreateCalendarAsync(HarvestCalendar calendar);
    
    Task<IEnumerable<HarvestRecord>> GetEncinteByYearAsync(int year);
    Task<IEnumerable<HarvestRecord>> GetEncinteByWeekAsync(int semana, int ano);
    Task<HarvestRecord?> GetRecordWithDetailsAsync(Guid id);
    Task<HarvestRecord> CreateEncinteAsync(HarvestRecord record);
    Task UpdateEncinteAsync(HarvestRecord record);
    
    Task<IEnumerable<HarvestCosecha>> GetCosechasByYearAsync(int year);
    Task<IEnumerable<HarvestCosecha>> GetCosechasByEncinteWeekAsync(int semanaEncinte, int anoEncinte);
    Task<HarvestCosecha?> GetCosechaByIdAsync(Guid id);
    Task<HarvestCosecha> CreateCosechaAsync(HarvestCosecha cosecha);
    Task UpdateCosechaAsync(HarvestCosecha cosecha);
    
    Task<IEnumerable<BoxType>> GetBoxTypesAsync();
    Task<BoxType?> GetBoxTypeByCodeAsync(string code);
    Task<BoxType> CreateBoxTypeAsync(BoxType boxType);
    
    Task<IEnumerable<HarvestOrder>> GetOrdersAsync();
    Task<HarvestOrder?> GetOrderWithDetailsAsync(Guid id);
    Task<HarvestOrder> CreateOrderAsync(HarvestOrder order);
    Task UpdateOrderAsync(HarvestOrder order);
    
    Task<int> GetTotalRacimosByPeriodAsync(DateTime start, DateTime end);
    Task<Dictionary<string, int>> GetProjectionByWeeksAsync(int weeksAhead);
}
