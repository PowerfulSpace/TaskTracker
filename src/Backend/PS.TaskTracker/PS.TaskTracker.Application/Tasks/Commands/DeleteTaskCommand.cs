using MediatR;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed record DeleteTaskCommand(Guid Id) : IRequest<Unit>;
