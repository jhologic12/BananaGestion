using BananaGestion.Application.Common.Interfaces;
using BananaGestion.Domain.Entities;
using BananaGestion.Domain.Enums;
using BananaGestion.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BananaGestion.Infrastructure.Repositories;

public class TaskRepository : Repository<TaskConfig>, ITaskRepository
{
    public TaskRepository(BananaDbContext context) : base(context) { }

    public async Task<IEnumerable<TaskAssignment>> GetPendingAssignmentsAsync(Guid? userId = null)
    {
        var query = _context.TaskAssignments
            .Include(t => t.TaskConfig)
            .Include(t => t.User)
            .Include(t => t.Lote)
            .Where(t => t.Status == AssignmentStatus.Programada || t.Status == AssignmentStatus.EnProgreso);

        if (userId.HasValue)
        {
            query = query.Where(t => t.UserId == userId.Value);
        }

        return await query.OrderBy(t => t.FechaProgramada).ToListAsync();
    }

    public async Task<IEnumerable<TaskAssignment>> GetAssignmentsByDateRangeAsync(DateTime start, DateTime end)
    {
        var startUtc = DateTime.SpecifyKind(start, DateTimeKind.Utc);
        var endUtc = DateTime.SpecifyKind(end, DateTimeKind.Utc);
        
        return await _context.TaskAssignments
            .Include(t => t.TaskConfig)
            .Include(t => t.User)
            .Include(t => t.Lote)
            .Where(t => t.FechaProgramada >= startUtc && t.FechaProgramada <= endUtc)
            .OrderBy(t => t.FechaProgramada)
            .ToListAsync();
    }

    public async Task<TaskAssignment?> GetAssignmentWithDetailsAsync(Guid id)
    {
        return await _context.TaskAssignments
            .Include(t => t.TaskConfig)
            .ThenInclude(c => c.Insumo)
            .Include(t => t.User)
            .Include(t => t.Lote)
            .Include(t => t.TaskLog)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskLog?> GetTaskLogAsync(Guid assignmentId)
    {
        return await _context.TaskLogs.FirstOrDefaultAsync(l => l.TaskAssignmentId == assignmentId);
    }

    public async Task<IEnumerable<TaskAssignment>> GetOverdueAssignmentsAsync()
    {
        var today = DateTime.UtcNow.Date;
        var todayStart = new DateTime(today.Year, today.Month, today.Day, 0, 0, 0, DateTimeKind.Utc);
        
        return await _context.TaskAssignments
            .Include(t => t.TaskConfig)
            .Include(t => t.User)
            .Include(t => t.Lote)
            .Where(t => t.FechaProgramada < todayStart && 
                       (t.Status == AssignmentStatus.Programada || t.Status == AssignmentStatus.EnProgreso))
            .OrderBy(t => t.FechaProgramada)
            .ToListAsync();
    }

    public async Task CreateAssignmentAsync(TaskAssignment assignment)
    {
        await _context.TaskAssignments.AddAsync(assignment);
        await _context.SaveChangesAsync();
    }

    public async Task CreateTaskLogAsync(TaskLog log)
    {
        await _context.TaskLogs.AddAsync(log);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAssignmentStatusAsync(Guid id, AssignmentStatus status)
    {
        var assignment = await _context.TaskAssignments.FindAsync(id);
        if (assignment != null)
        {
            assignment.Status = status;
            if (status == AssignmentStatus.Completada)
            {
                assignment.FechaCompletada = DateTime.UtcNow;
            }
            await _context.SaveChangesAsync();
        }
    }
}
