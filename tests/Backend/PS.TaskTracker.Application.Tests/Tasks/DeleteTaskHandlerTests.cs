using MediatR;
using NSubstitute;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Exceptions;
using PS.TaskTracker.Application.Tasks.Commands;
using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Tests.Tasks;

public sealed class DeleteTaskHandlerTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly DeleteTaskHandler _handler;

    public DeleteTaskHandlerTests()
    {
        _handler = new DeleteTaskHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ExistingTask_RemovesSavesAndReturnsUnit()
    {
        // Arrange
        var task = TestData.CreateTodoTask();
        var request = new DeleteTaskCommand(task.Id);
        _repository.GetByIdAsync(task.Id, Arg.Any<CancellationToken>()).Returns(task);

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        _repository.Received(1).Remove(task);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        Assert.Equal(Unit.Value, result);
    }

    [Fact]
    public async Task Handle_TaskNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new DeleteTaskCommand(id);
        var expected = $"Task with id {id} was not found.";
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((TaskItem?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        // Assert
        Assert.Equal(expected, ex.Message);
    }
}
