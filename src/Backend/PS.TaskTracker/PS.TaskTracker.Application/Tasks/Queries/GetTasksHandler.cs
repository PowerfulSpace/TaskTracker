using MediatR;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Dtos;
using PS.TaskTracker.Application.Tasks.Mappings;

namespace PS.TaskTracker.Application.Tasks.Queries;

public sealed class GetTasksHandler : IRequestHandler<GetTasksQuery, IReadOnlyList<TaskDto>>
{
    private readonly ITaskRepository _repository;

    public GetTasksHandler(ITaskRepository repository)
    {
        _repository = repository;
    }

    public async Task<IReadOnlyList<TaskDto>> Handle(GetTasksQuery request, CancellationToken cancellationToken)
    {
        var tasks = await _repository.GetAllAsync(cancellationToken);
        return tasks.Select(t => t.ToDto()).ToList();
    }
}
