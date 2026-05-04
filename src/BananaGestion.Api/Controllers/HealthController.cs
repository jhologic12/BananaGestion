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
    public IActionResult Get()
    {
        return Ok(new { 
            status = "healthy", 
            version = "v2.0",
            timestamp = DateTime.UtcNow 
        });
    }

    [HttpGet("env-check")]
    [AllowAnonymous]
    public IActionResult EnvCheck()
    {
        var conn = Environment.GetEnvironmentVariable("ConnectionStrings__DefaultConnection");
        var jwt = Environment.GetEnvironmentVariable("Jwt__Key");
        return Ok(new
        {
            hasConnectionString = !string.IsNullOrEmpty(conn),
            hasJwtKey = !string.IsNullOrEmpty(jwt),
            port = Environment.GetEnvironmentVariable("PORT"),
            aspnetcoreEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"),
            timestamp = DateTime.UtcNow
        });
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

    [HttpGet("calendar-check")]
    public async Task<IActionResult> CalendarCheck([FromQuery] int year = 2026)
    {
        try
        {
            var count = await _db.Set<BananaGestion.Domain.Entities.HarvestCalendar>()
                .Where(c => c.Ano == year)
                .CountAsync();

            return Ok(new { year, totalRecords = count, timestamp = DateTime.UtcNow });
        }
        catch (Exception ex)
        {
            return Ok(new { error = ex.Message, timestamp = DateTime.UtcNow });
        }
    }
}
