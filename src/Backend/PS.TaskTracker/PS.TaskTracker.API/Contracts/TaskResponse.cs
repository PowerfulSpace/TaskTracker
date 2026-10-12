using PS.TaskTracker.Domain;

namespace PS.TaskTracker.API.Contracts;

public sealed record TaskResponse
{
    public Guid Id { get; init; }
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
    public TaskItemStatus Status { get; init; }
    public DateTime CreatedAtUtc { get; init; }
    public DateTime UpdatedAtUtc { get; init; }
}
