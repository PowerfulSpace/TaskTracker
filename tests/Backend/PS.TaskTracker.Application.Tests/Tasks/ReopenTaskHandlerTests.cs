using NSubstitute;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Exceptions;
using PS.TaskTracker.Application.Tasks.Commands;
using PS.TaskTracker.Domain;
using PS.TaskTracker.Domain.Exceptions;

namespace PS.TaskTracker.Application.Tests.Tasks;

public sealed class ReopenTaskHandlerTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly ReopenTaskHandler _handler;

    public ReopenTaskHandlerTests()
    {
        _handler = new ReopenTaskHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_DoneTask_ReopensSavesAndReturnsInProgressDto()
    {
        // Arrange
        var task = TestData.CreateDoneTask();
        var request = new ReopenTaskCommand(task.Id);
        _repository.GetByIdAsync(task.Id, Arg.Any<CancellationToken>()).Returns(task);

        // Act
        var dto = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(TaskItemStatus.InProgress, dto.Status);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_TaskNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new ReopenTaskCommand(id);
        var expected = $"Task with id {id} was not found.";
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((TaskItem?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public async Task Handle_TodoTask_ThrowsDomainConflictException()
    {
        // Arrange
        var task = TestData.CreateTodoTask();
        var request = new ReopenTaskCommand(task.Id);
        var expected = "Cannot reopen task in status Todo.";
        _repository.GetByIdAsync(task.Id, Arg.Any<CancellationToken>()).Returns(task);

        // Act
        var ex = await Assert.ThrowsAsync<DomainConflictException>(() => _handler.Handle(request, CancellationToken.None));

        // Assert
        Assert.Equal(expected, ex.Message);
    }
}
