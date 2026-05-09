using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Domain.Entities;
using BananaGestion.Domain.Enums;
using BananaGestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BananaGestion.Infrastructure.Repositories;

public class UserRepository : Repository<User>, IUserRepository
{
    public UserRepository(BananaDbContext context) : base(context) { }

    public async Task<User?> GetByEmailAsync(string email)
    {
        // Use raw SQL to avoid EF Core LINQ timeout issues through Supabase pooler
        var conn = _context.Database.GetDbConnection();
        await conn.OpenAsync();
        await using var cmd = conn.CreateCommand();
        cmd.CommandText = "SELECT \"Id\", \"Email\", \"PasswordHash\", \"Nombre\", \"Apellido\", \"Telefono\", \"Rol\", \"Activo\", \"FechaCreacion\", \"UltimoLogin\" FROM \"users\" WHERE LOWER(\"Email\") = LOWER(@email) LIMIT 1";
        cmd.Parameters.Add(new Npgsql.NpgsqlParameter("email", email));
        await using var reader = await cmd.ExecuteReaderAsync();
        if (await reader.ReadAsync())
        {
            return new User
            {
                Id = reader.GetGuid(0),
                Email = reader.GetString(1),
                PasswordHash = reader.GetString(2),
                Nombre = reader.GetString(3),
                Apellido = reader.GetString(4),
                Telefono = reader.IsDBNull(5) ? null : reader.GetString(5),
                Rol = (UserRole)reader.GetInt32(6),
                Activo = reader.GetBoolean(7),
                FechaCreacion = reader.GetDateTime(8),
                UltimoLogin = reader.IsDBNull(9) ? null : reader.GetDateTime(9)
            };
        }
        return null;
    }

    public async Task<User?> GetWithAssignmentsAsync(Guid id)
    {
        return await _dbSet
            .Include(u => u.TaskAssignments)
            .ThenInclude(t => t.TaskConfig)
            .Include(u => u.TaskAssignments)
            .ThenInclude(t => t.Lote)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetByRoleAsync(string role)
    {
        if (Enum.TryParse<UserRole>(role, out var roleEnum))
        {
            return await _dbSet.Where(u => u.Rol == roleEnum && u.Activo).ToListAsync();
        }
        return Enumerable.Empty<User>();
    }
}
