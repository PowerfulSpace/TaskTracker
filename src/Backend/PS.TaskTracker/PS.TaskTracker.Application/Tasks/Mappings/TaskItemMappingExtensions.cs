using PS.TaskTracker.Application.Dtos;
using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Tasks.Mappings;

public static class TaskItemMappingExtensions
{
    public static TaskDto ToDto(this TaskItem task)
    {
        return new TaskDto(
            task.Id,
            task.Title,
            task.Description,
            task.Status,
            task.CreatedAtUtc,
            task.UpdatedAtUtc);
    }
}
