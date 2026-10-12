using MediatR;
using PS.TaskTracker.Application.Dtos;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed record StartTaskCommand(Guid Id) : IRequest<TaskDto>;
