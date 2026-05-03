using System.Runtime.CompilerServices;
using BananaGestion.Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BananaGestion.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class HealthController : ControllerBase
{
    private readonly BananaDbContext _db;

    public HealthController(BananaDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            service = "BananaGestion API",
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
            {
                return Ok(new { canConnect = false, error = "Cannot connect to database" });
            }

            var tables = new List<string>();
            using (var cmd = _db.Database.GetDbConnection().CreateCommand())
            {
                cmd.CommandText = "SELECT tablename FROM pg_catalog.pg_tables WHERE schemaname = 'public'";
                await _db.Database.OpenConnectionAsync();
                using (var reader = await cmd.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        tables.Add(reader.GetString(0));
                    }
                }
            }

            return Ok(new
            {
                canConnect = true,
                tableCount = tables.Count,
                tables = tables,
                timestamp = DateTime.UtcNow
            });
        }
        catch (Exception ex)
        {
            return Ok(new
            {
                canConnect = false,
                error = ex.Message,
                innerError = ex.InnerException?.Message,
                timestamp = DateTime.UtcNow
            });
        }
    }
}
