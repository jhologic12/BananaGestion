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
        return await _dbSet.FirstOrDefaultAsync(u => u.Email.ToLower() == email.ToLower());
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
