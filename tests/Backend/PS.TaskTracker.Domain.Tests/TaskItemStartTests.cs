using PS.TaskTracker.Domain;
using PS.TaskTracker.Domain.Exceptions;

namespace PS.TaskTracker.Domain.Tests;

public class TaskItemStartTests
{
    private static readonly DateTime UtcNow = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime LaterUtc = new(2026, 1, 2, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Start_FromTodo_SetsInProgress()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);

        // Act
        task.Start(LaterUtc);

        // Assert
        Assert.Equal(TaskItemStatus.InProgress, task.Status);
        Assert.Equal(LaterUtc, task.UpdatedAtUtc);
    }

    [Fact]
    public void Start_FromInProgress_ThrowsDomainConflictException()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);
        task.Start(LaterUtc);
        const string expected = "Cannot start task in status InProgress.";

        // Act
        var ex = Assert.Throws<DomainConflictException>(() => task.Start(LaterUtc));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void Start_FromDone_ThrowsDomainConflictException()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);
        task.Start(LaterUtc);
        task.Complete(LaterUtc);
        const string expected = "Cannot start task in status Done.";

        // Act
        var ex = Assert.Throws<DomainConflictException>(() => task.Start(LaterUtc));

        // Assert
        Assert.Equal(expected, ex.Message);
    }
}
