using BananaGestion.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BananaGestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly BananaGestion.Infrastructure.Data.BananaDbContext _db;

    public HealthController(BananaGestion.Infrastructure.Data.BananaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
        await Task.CompletedTask;
        return Ok(new { status = "healthy", version = "v2.0", timestamp = DateTime.UtcNow });
    }

    [HttpGet("db-check")]
    public async Task<IActionResult> DbCheck()
    {
        try
        {
            var canConnect = await _db.Database.CanConnectAsync();
            if (!canConnect)
                return Ok(new { canConnect = false });

            var tables = new List<string>();
            using var cmd = _db.Database.GetDbConnection().CreateCommand();
            cmd.CommandText = "SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname = 'public'";
            await _db.Database.OpenConnectionAsync();
            using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
                tables.Add(reader.GetString(0));

            return Ok(new { canConnect = true, tables, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return Ok(new { canConnect = false, error = ex.Message, timestamp = DateTime.UtcNow });
        }
    }

    [HttpGet("user-check")]
    public async Task<IActionResult> UserCheck()
    {
        try
        {
            var sw = System.Diagnostics.Stopwatch.StartNew();
            var user = await _db.Set<User>().FirstOrDefaultAsync(u => u.Email.ToLower() == "jhonospino@gmail.com".ToLower());
            if (user == null)
                return Ok(new { step = "select", elapsedMs = sw.ElapsedMilliseconds, found = false });
            
            var verify = BCrypt.Net.BCrypt.Verify("J@0f90121554860", user.PasswordHash);
            
            user.UltimoLogin = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            sw.Stop();
            
            return Ok(new { 
                step = "select+verify+update", 
                elapsedMs = sw.ElapsedMilliseconds, 
                found = true, 
                email = user.Email, 
                verifyOk = verify,
                rol = user.Rol.ToString(),
                activo = user.Activo
            });
        }
        catch (Exception ex)
        {
            return Ok(new { error = ex.GetType().Name, message = ex.Message });
        }
    }

    [HttpGet("calendar-check")]
    [AllowAnonymous]
    public async Task<IActionResult> CalendarCheck([FromQuery] int year = 2026)
    {
        try
        {
            var count = await _db.Set<BananaGestion.Domain.Entities.HarvestCalendar>()
                .Where(c => c.Ano == year)
                .CountAsync();

            var allYears = await _db.Set<BananaGestion.Domain.Entities.HarvestCalendar>()
                .Select(c => c.Ano)
                .Distinct()
                .ToListAsync();

            return Ok(new { year, totalRecords = count, allYearsInDb = allYears, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return Ok(new { error = ex.Message, timestamp = DateTime.UtcNow });
        }
    }
}
