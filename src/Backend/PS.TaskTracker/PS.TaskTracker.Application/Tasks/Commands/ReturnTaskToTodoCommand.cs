using MediatR;
using PS.TaskTracker.Application.Dtos;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed record ReturnTaskToTodoCommand(Guid Id) : IRequest<TaskDto>;
