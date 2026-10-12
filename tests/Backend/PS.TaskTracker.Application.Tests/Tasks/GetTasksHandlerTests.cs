using NSubstitute;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Tasks.Queries;
using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Tests.Tasks;

public sealed class GetTasksHandlerTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly GetTasksHandler _handler;

    public GetTasksHandlerTests()
    {
        _handler = new GetTasksHandler(_repository);
    }

    [Fact]
    public async Task Handle_EmptyList_ReturnsEmptyResult()
    {
        // Arrange
        IReadOnlyList<TaskItem> tasks = Array.Empty<TaskItem>();
        _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(tasks);
        var request = new GetTasksQuery();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Empty(result);
    }

    [Fact]
    public async Task Handle_NonEmptyList_ReturnsTaskDtosInSameOrder()
    {
        // Arrange
        var first = TaskItem.Create("First", null, TestData.UtcNow);
        var second = TaskItem.Create("Second", null, TestData.UtcNow);
        IReadOnlyList<TaskItem> tasks = new List<TaskItem> { first, second };
        _repository.GetAllAsync(Arg.Any<CancellationToken>()).Returns(tasks);
        var request = new GetTasksQuery();

        // Act
        var result = await _handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.Equal(2, result.Count);
        Assert.Equal(first.Id, result[0].Id);
        Assert.Equal(second.Id, result[1].Id);
    }
}
