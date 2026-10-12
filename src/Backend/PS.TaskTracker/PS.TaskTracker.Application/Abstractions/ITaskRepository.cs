using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Abstractions;

public interface ITaskRepository
{
    Task<TaskItem?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
    Task<TaskItem?> GetByIdReadOnlyAsync(Guid id, CancellationToken cancellationToken);
    Task<IReadOnlyList<TaskItem>> GetAllAsync(CancellationToken cancellationToken);
    Task AddAsync(TaskItem task, CancellationToken cancellationToken);
    void Remove(TaskItem task);
}
