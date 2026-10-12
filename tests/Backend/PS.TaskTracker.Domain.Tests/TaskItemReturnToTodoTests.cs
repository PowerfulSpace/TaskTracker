using PS.TaskTracker.Domain;
using PS.TaskTracker.Domain.Exceptions;

namespace PS.TaskTracker.Domain.Tests;

public class TaskItemReturnToTodoTests
{
    private static readonly DateTime UtcNow = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime LaterUtc = new(2026, 1, 2, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void ReturnToTodo_FromInProgress_SetsTodo()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);
        task.Start(LaterUtc);

        // Act
        task.ReturnToTodo(LaterUtc);

        // Assert
        Assert.Equal(TaskItemStatus.Todo, task.Status);
        Assert.Equal(LaterUtc, task.UpdatedAtUtc);
    }

    [Fact]
    public void ReturnToTodo_FromTodo_ThrowsDomainConflictException()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);
        const string expected = "Cannot return task to todo in status Todo.";

        // Act
        var ex = Assert.Throws<DomainConflictException>(() => task.ReturnToTodo(LaterUtc));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void ReturnToTodo_FromDone_ThrowsDomainConflictException()
    {
        // Arrange
        var task = TaskItem.Create("Test task", null, UtcNow);
        task.Start(LaterUtc);
        task.Complete(LaterUtc);
        const string expected = "Cannot return task to todo in status Done.";

        // Act
        var ex = Assert.Throws<DomainConflictException>(() => task.ReturnToTodo(LaterUtc));

        // Assert
        Assert.Equal(expected, ex.Message);
    }
}
