using BananaGestion.Domain.Entities;
using BananaGestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BananaGestion.Infrastructure.Repositories;

public interface IFinancialRepository
{
    Task<IEnumerable<FinancialTransaction>> GetTransactionsAsync(
        DateTime? start = null, 
        DateTime? end = null, 
        string? tipo = null,
        string? categoria = null);
    Task<FinancialTransaction?> GetTransactionByIdAsync(Guid id);
    Task<FinancialTransaction> CreateTransactionAsync(FinancialTransaction transaction);
    Task<decimal> GetBalanceAsync();
    Task<Dictionary<string, decimal>> GetMonthlySummaryAsync(int year);
    Task<Dictionary<string, decimal>> GetCategoryBreakdownAsync(string tipo);
}

public class FinancialRepository : IFinancialRepository
{
    private readonly BananaDbContext _context;

    public FinancialRepository(BananaDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<FinancialTransaction>> GetTransactionsAsync(
        DateTime? start = null,
        DateTime? end = null,
        string? tipo = null,
        string? categoria = null)
    {
        var query = _context.FinancialTransactions.AsQueryable();

        if (start.HasValue)
            query = query.Where(t => t.Fecha >= start.Value);
        if (end.HasValue)
            query = query.Where(t => t.Fecha <= end.Value);
        if (!string.IsNullOrEmpty(tipo))
            query = query.Where(t => t.Tipo == tipo);
        if (!string.IsNullOrEmpty(categoria))
            query = query.Where(t => t.Categoria == categoria);

        return await query.OrderByDescending(t => t.Fecha).ToListAsync();
    }

    public async Task<FinancialTransaction?> GetTransactionByIdAsync(Guid id)
    {
        return await _context.FinancialTransactions.FindAsync(id);
    }

    public async Task<FinancialTransaction> CreateTransactionAsync(FinancialTransaction transaction)
    {
        await _context.FinancialTransactions.AddAsync(transaction);
        await _context.SaveChangesAsync();
        return transaction;
    }

    public async Task<decimal> GetBalanceAsync()
    {
        var ingresos = await _context.FinancialTransactions
            .Where(t => t.Tipo == "Ingreso")
            .SumAsync(t => t.Monto);

        var gastos = await _context.FinancialTransactions
            .Where(t => t.Tipo == "Gasto")
            .SumAsync(t => t.Monto);

        return ingresos - gastos;
    }

    public async Task<Dictionary<string, decimal>> GetMonthlySummaryAsync(int year)
    {
        var summary = new Dictionary<string, decimal>();
        var months = Enumerable.Range(1, 12);

        foreach (var month in months)
        {
            var monthName = new DateTime(year, month, 1).ToString("MMMM");
            
            var ingresos = await _context.FinancialTransactions
                .Where(t => t.Tipo == "Ingreso" && t.Fecha.Year == year && t.Fecha.Month == month)
                .SumAsync(t => t.Monto);

            var gastos = await _context.FinancialTransactions
                .Where(t => t.Tipo == "Gasto" && t.Fecha.Year == year && t.Fecha.Month == month)
                .SumAsync(t => t.Monto);

            summary[$"{monthName}"] = ingresos - gastos;
        }

        return summary;
    }

    public async Task<Dictionary<string, decimal>> GetCategoryBreakdownAsync(string tipo)
    {
        return await _context.FinancialTransactions
            .Where(t => t.Tipo == tipo)
            .GroupBy(t => t.Categoria)
            .ToDictionaryAsync(g => g.Key, g => g.Sum(t => t.Monto));
    }
}
