using NSubstitute;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Exceptions;
using PS.TaskTracker.Application.Tasks.Commands;
using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Tests.Tasks;

public sealed class UpdateTaskHandlerTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly UpdateTaskHandler _handler;

    public UpdateTaskHandlerTests()
    {
        _handler = new UpdateTaskHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ExistingTask_UpdatesDetailsSavesAndReturnsUpdatedDto()
    {
        // Arrange
        var task = TestData.CreateTodoTask();
        var request = new UpdateTaskCommand(task.Id, "Updated title", "Updated description");
        _repository.GetByIdAsync(task.Id, Arg.Any<CancellationToken>()).Returns(task);

        // Act
        var dto = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal("Updated title", dto.Title);
        Assert.Equal("Updated description", dto.Description);
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
    }

    [Fact]
    public async Task Handle_TaskNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateTaskCommand(id, "Title", null);
        var expected = $"Task with id {id} was not found.";
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((TaskItem?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public async Task Handle_TaskNotFound_DoesNotCallSaveChanges()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new UpdateTaskCommand(id, "Title", null);
        _repository.GetByIdAsync(id, Arg.Any<CancellationToken>()).Returns((TaskItem?)null);

        // Act
        await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        // Assert
        await _unitOfWork.Received(0).SaveChangesAsync(Arg.Any<CancellationToken>());
    }
}
