using PS.TaskTracker.Domain;
using PS.TaskTracker.Domain.Exceptions;

namespace PS.TaskTracker.Domain.Tests;

public class TaskItemCompleteTests
{
    private static readonly DateTime UtcNow = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime LaterUtc = new(2026, 1, 2, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime EvenLaterUtc = new(2026, 1, 3, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Complete_FromInProgress_SetsDone()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);
        task.Start(LaterUtc);

        // Act
        task.Complete(LaterUtc);

        // Assert
        Assert.Equal(TaskItemStatus.Done, task.Status);
        Assert.Equal(LaterUtc, task.UpdatedAtUtc);
    }

    [Fact]
    public void Complete_FromTodo_ThrowsDomainConflictException()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);
        const string expected = "Cannot complete task in status Todo.";

        // Act
        var ex = Assert.Throws<DomainConflictException>(() => task.Complete(LaterUtc));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void Complete_FromDone_DoesNotThrowAndKeepsUpdatedAtUtc()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);
        task.Start(LaterUtc);
        task.Complete(LaterUtc);
        DateTime updatedAtUtcBefore = task.UpdatedAtUtc;

        // Act
        task.Complete(EvenLaterUtc);

        // Assert
        Assert.Equal(TaskItemStatus.Done, task.Status);
        Assert.Equal(updatedAtUtcBefore, task.UpdatedAtUtc);
    }
}
