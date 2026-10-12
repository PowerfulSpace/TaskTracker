using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Dtos;

public sealed record TaskDto(
    Guid Id,
    string Title,
    string? Description,
    TaskItemStatus Status,
    DateTime CreatedAtUtc,
    DateTime UpdatedAtUtc);
