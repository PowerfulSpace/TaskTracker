using MediatR;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Exceptions;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed class DeleteTaskHandler : IRequestHandler<DeleteTaskCommand, Unit>
{
    private readonly ITaskRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTaskHandler(ITaskRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Unit> Handle(DeleteTaskCommand request, CancellationToken cancellationToken)
    {
        var task = await _repository.GetByIdAsync(request.Id, cancellationToken);
        if (task is null)
        {
            throw new NotFoundException($"Task with id {request.Id} was not found.");
        }

        _repository.Remove(task);
        await _unitOfWork.SaveChangesAsync(cancellationToken);
        return Unit.Value;
    }
}
