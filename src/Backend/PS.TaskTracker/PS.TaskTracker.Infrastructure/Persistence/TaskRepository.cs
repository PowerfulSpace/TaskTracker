using Microsoft.EntityFrameworkCore;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Infrastructure.Persistence;

public sealed class TaskRepository : ITaskRepository
{
    private readonly AppDbContext _context;

    public TaskRepository(AppDbContext context)
    {
        _context = context;
    }

    public Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Tasks.FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public Task<TaskItem?> GetByIdReadOnlyAsync(Guid id, CancellationToken cancellationToken)
    {
        return _context.Tasks
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    public async Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken cancellationToken)
    {
        return await _context.Tasks
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    public async Task AddAsync(TaskItem task, CancellationToken cancellationToken)
    {
        await _context.Tasks.AddAsync(task, cancellationToken);
    }

    public void Remove(TaskItem task)
    {
        _context.Tasks.Remove(task);
    }
}
