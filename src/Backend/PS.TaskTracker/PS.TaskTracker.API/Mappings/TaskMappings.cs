using PS.TaskTracker.API.Contracts;
using PS.TaskTracker.Application.Dtos;
using PS.TaskTracker.Application.Tasks.Commands;

namespace PS.TaskTracker.API.Mappings;

public static class TaskMappings
{
    public static CreateTaskCommand ToCommand(this CreateTaskRequest request)
    {
        return new CreateTaskCommand(request.Title, request.Description);
    }

    public static UpdateTaskCommand ToCommand(this UpdateTaskRequest request, Guid id)
    {
        return new UpdateTaskCommand(id, request.Title, request.Description);
    }

    public static TaskResponse ToResponse(this TaskDto dto)
    {
        return new TaskResponse
        {
            Id = dto.Id,
            Title = dto.Title,
            Description = dto.Description,
            Status = dto.Status,
            CreatedAtUtc = dto.CreatedAtUtc,
            UpdatedAtUtc = dto.UpdatedAtUtc
        };
    }
}
