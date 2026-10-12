using PS.TaskTracker.Domain.Exceptions;

namespace PS.TaskTracker.Domain;

public class TaskItem
{
    public Guid Id { get; private set; }
    public string Title { get; private set; } = null!;
    public string? Description { get; private set; }
    public TaskItemStatus Status { get; private set; }
    public DateTime CreatedAtUtc { get; private set; }
    public DateTime UpdatedAtUtc { get; private set; }

    private TaskItem()
    {
    }

    private TaskItem(Guid id, string title, string? description, DateTime utcNow)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = TaskItemStatus.Todo;
        CreatedAtUtc = utcNow;
        UpdatedAtUtc = utcNow;
    }

    public static TaskItem Create(string title, string? description, DateTime utcNow)
    {
        Validate(title, description);
        return new TaskItem(Guid.NewGuid(), title, description, utcNow);
    }

    public void Start(DateTime utcNow)
    {
        if (Status != TaskItemStatus.Todo)
        {
            throw new DomainConflictException($"Cannot start task in status {Status}.");
        }

        Status = TaskItemStatus.InProgress;
        UpdatedAtUtc = utcNow;
    }

    public void Complete(DateTime utcNow)
    {
        if (Status == TaskItemStatus.Done)
        {
            return;
        }

        if (Status != TaskItemStatus.InProgress)
        {
            throw new DomainConflictException($"Cannot complete task in status {Status}.");
        }

        Status = TaskItemStatus.Done;
        UpdatedAtUtc = utcNow;
    }

    public void Reopen(DateTime utcNow)
    {
        if (Status != TaskItemStatus.Done)
        {
            throw new DomainConflictException($"Cannot reopen task in status {Status}.");
        }

        Status = TaskItemStatus.InProgress;
        UpdatedAtUtc = utcNow;
    }

    public void ReturnToTodo(DateTime utcNow)
    {
        if (Status != TaskItemStatus.InProgress)
        {
            throw new DomainConflictException($"Cannot return task to todo in status {Status}.");
        }

        Status = TaskItemStatus.Todo;
        UpdatedAtUtc = utcNow;
    }

    public void UpdateDetails(string title, string? description, DateTime utcNow)
    {
        Validate(title, description);
        Title = title;
        Description = description;
        UpdatedAtUtc = utcNow;
    }

    private static void Validate(string title, string? description)
    {
        if (string.IsNullOrWhiteSpace(title))
        {
            throw new DomainValidationException("Title must not be empty.");
        }

        if (title.Length > 200)
        {
            throw new DomainValidationException("Title must not exceed 200 characters.");
        }

        if (description is { Length: > 2000 })
        {
            throw new DomainValidationException("Description must not exceed 2000 characters.");
        }
    }
}
