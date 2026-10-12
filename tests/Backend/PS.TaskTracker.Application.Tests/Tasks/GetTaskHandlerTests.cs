using NSubstitute;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Exceptions;
using PS.TaskTracker.Application.Tasks.Queries;
using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Tests.Tasks;

public sealed class GetTaskHandlerTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly GetTaskHandler _handler;

    public GetTaskHandlerTests()
    {
        _handler = new GetTaskHandler(_repository);
    }

    [Fact]
    public async Task Handle_TaskFound_ReturnsTaskDtoWithCorrectFields()
    {
        // Arrange
        var task = TestData.CreateTodoTask();
        var request = new GetTaskQuery(task.Id);
        _repository.GetByIdReadOnlyAsync(task.Id, Arg.Any<CancellationToken>()).Returns(task);

        // Act
        var dto = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(task.Id, dto.Id);
        Assert.Equal(task.Title, dto.Title);
        Assert.Equal(task.Description, dto.Description);
        Assert.Equal(task.Status, dto.Status);
        Assert.Equal(task.CreatedAtUtc, dto.CreatedAtUtc);
        Assert.Equal(task.UpdatedAtUtc, dto.UpdatedAtUtc);
    }

    [Fact]
    public async Task Handle_TaskNotFound_ThrowsNotFoundException()
    {
        // Arrange
        var id = Guid.NewGuid();
        var request = new GetTaskQuery(id);
        var expected = $"Task with id {id} was not found.";
        _repository.GetByIdReadOnlyAsync(id, Arg.Any<CancellationToken>()).Returns((TaskItem?)null);

        // Act
        var ex = await Assert.ThrowsAsync<NotFoundException>(() => _handler.Handle(request, CancellationToken.None));

        // Assert
        Assert.Equal(expected, ex.Message);
    }
}
