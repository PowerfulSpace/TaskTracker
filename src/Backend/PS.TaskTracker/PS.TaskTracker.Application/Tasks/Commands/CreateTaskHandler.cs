using MediatR;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Dtos;
using PS.TaskTracker.Application.Tasks.Mappings;
using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed class CreateTaskHandler : IRequestHandler<CreateTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTaskHandler(ITaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskDto> Handle(CreateTaskCommand request, CancellationToken cancellationToken)
    {
        var task = TaskItem.Create(request.Title, request.Description, DateTime.UtcNow);
        await _repository.AddAsync(task, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return task.ToDto();
    }
}
