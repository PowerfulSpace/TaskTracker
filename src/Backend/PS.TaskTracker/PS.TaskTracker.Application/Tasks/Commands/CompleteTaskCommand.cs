using MediatR;
using PS.TaskTracker.Application.Dtos;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed record CompleteTaskCommand(Guid Id) : IRequest<TaskDto>;
