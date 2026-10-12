namespace PS.TaskTracker.API.Contracts;

public sealed record CreateTaskRequest
{
    public string Title { get; init; } = string.Empty;
    public string? Description { get; init; }
}
