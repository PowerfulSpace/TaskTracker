using MediatR;
using PS.TaskTracker.Application.Dtos;

namespace PS.TaskTracker.Application.Tasks.Queries;

public sealed record GetTasksQuery : IRequest<IReadOnlyList<TaskDto>>;
