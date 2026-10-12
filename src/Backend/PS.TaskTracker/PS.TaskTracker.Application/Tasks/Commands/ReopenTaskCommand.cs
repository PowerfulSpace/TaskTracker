using MediatR;
using PS.TaskTracker.Application.Dtos;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed record ReopenTaskCommand(Guid Id) : IRequest<TaskDto>;
