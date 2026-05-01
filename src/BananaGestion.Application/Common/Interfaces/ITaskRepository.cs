using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Domain.Entities;
using BananaGestion.Domain.Enums;

namespace BananaGestion.Application.Common.Interfaces;

public interface ITaskRepository : IRepository<TaskConfig>
{
    Task<IEnumerable<TaskAssignment>> GetPendingAssignmentsAsync(Guid? userId = null);
    Task<IEnumerable<TaskAssignment>> GetAssignmentsByDateRangeAsync(DateTime start, DateTime end);
    Task<TaskAssignment?> GetAssignmentWithDetailsAsync(Guid id);
    Task<TaskLog?> GetTaskLogAsync(Guid assignmentId);
    Task<IEnumerable<TaskAssignment>> GetOverdueAssignmentsAsync();
    Task CreateAssignmentAsync(TaskAssignment assignment);
    Task CreateTaskLogAsync(TaskLog log);
    Task UpdateAssignmentStatusAsync(Guid id, AssignmentStatus status);
}
