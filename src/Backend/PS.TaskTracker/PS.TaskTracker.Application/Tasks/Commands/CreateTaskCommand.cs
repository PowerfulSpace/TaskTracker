using MediatR;
using PS.TaskTracker.Application.Dtos;

namespace PS.TaskTracker.Application.Tasks.Commands;

public sealed record CreateTaskCommand(string Title, string? Description) : IRequest<TaskDto>;
