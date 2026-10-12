using MediatR;
using Microsoft.AspNetCore.Mvc;
using PS.TaskTracker.API.Contracts;
using PS.TaskTracker.API.Mappings;
using PS.TaskTracker.Application.Tasks.Commands;
using PS.TaskTracker.Application.Tasks.Queries;

namespace PS.TaskTracker.API.Controllers;

[ApiController]
[Route("api/tasks")]
public sealed class TasksController : ControllerBase
{
    private readonly IMediator _mediator;

    public TasksController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateTaskRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand();
        var result = await _mediator.Send(command, cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result.ToResponse());
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTasksQuery(), cancellationToken);
        return Ok(result.Select(t => t.ToResponse()).ToList());
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetTaskQuery(id), cancellationToken);
        return Ok(result.ToResponse());
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateTaskRequest request, CancellationToken cancellationToken)
    {
        var command = request.ToCommand(id);
        var result = await _mediator.Send(command, cancellationToken);
        return Ok(result.ToResponse());
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        await _mediator.Send(new DeleteTaskCommand(id), cancellationToken);
        return NoContent();
    }

    [HttpPost("{id:guid}/start")]
    public async Task<IActionResult> Start(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new StartTaskCommand(id), cancellationToken);
        return Ok(result.ToResponse());
    }

    [HttpPost("{id:guid}/complete")]
    public async Task<IActionResult> Complete(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new CompleteTaskCommand(id), cancellationToken);
        return Ok(result.ToResponse());
    }

    [HttpPost("{id:guid}/reopen")]
    public async Task<IActionResult> Reopen(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReopenTaskCommand(id), cancellationToken);
        return Ok(result.ToResponse());
    }

    [HttpPost("{id:guid}/reset")]
    public async Task<IActionResult> Reset(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new ReturnTaskToTodoCommand(id), cancellationToken);
        return Ok(result.ToResponse());
    }
}
