using MediatR;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Dtos;
using PS.TaskTracker.Application.Exceptions;
using PS.TaskTracker.Application.Tasks.Mappings;

namespace PS.TaskTracker.Application.Tasks.Queries;

public sealed class GetTaskHandler : IRequestHandler<GetTaskQuery, TaskDto>
{
    private readonly ITaskRepository _repository;

    public GetTaskHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<TaskDto> Handle(GetTaskQuery request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdReadOnlyAsync(request.Id, cancellationToken);
        if (task is null)
        {
            throw new NotFoundException($"Task with id {request.Id} was not found.");
        }

        return task.ToDto();
    }
}
