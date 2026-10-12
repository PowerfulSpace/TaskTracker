using MediatR;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Dtos;
using PS.TaskTracker.Application.Exceptions;
using PS.TaskTracker.Application.Tasks.Mappings;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed class ReopenTaskHandler : IRequestHandler<ReopenTaskCommand, TaskDto>
{
    private readonly ITaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public ReopenTaskHandler(ITaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<TaskDto> Handle(ReopenTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
        {
            throw new NotFoundException($"Task with id {request.Id} was not found.");
        }

        task.Reopen(DateTime.UtcNow);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return task.ToDto();
    }
}
