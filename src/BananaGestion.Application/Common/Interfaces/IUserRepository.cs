using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Domain.Entities;

namespace BananaGestion.Application.Common.Interfaces;

public interface IUserRepository : IRepository<User>
{
    Task<User?> GetByEmailAsync(string email);
    Task<User?> GetWithAssignmentsAsync(Guid id);
    Task<IEnumerable<User>> GetByRoleAsync(string role);
}
