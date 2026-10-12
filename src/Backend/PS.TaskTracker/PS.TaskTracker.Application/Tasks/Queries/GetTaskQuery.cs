using MediatR;
using PS.TaskTracker.Application.Dtos;

namespace PS.TaskTracker.Application.Tasks.Queries;

public sealed record GetTaskQuery(Guid Id) : IRequest<TaskDto>;
