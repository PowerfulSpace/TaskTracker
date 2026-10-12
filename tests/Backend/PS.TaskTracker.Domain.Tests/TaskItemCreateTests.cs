using PS.TaskTracker.Domain;
using PS.TaskTracker.Domain.Exceptions;

namespace PS.TaskTracker.Domain.Tests;

public class TaskItemCreateTests
{
    private static readonly DateTime UtcNow = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Create_ValidInput_ReturnsTaskWithTodoStatus()
    {
        // Arrange
        const string title = "Test task";
        const string description = "Test description";

        // Act
        var task = TaskItem.Create(title, description, UtcNow);

        // Assert
        Assert.NotEqual(Guid.Empty, task.Id);
        Assert.Equal(title, task.Title);
        Assert.Equal(description, task.Description);
        Assert.Equal(TaskItemStatus.Todo, task.Status);
        Assert.Equal(UtcNow, task.CreatedAtUtc);
        Assert.Equal(UtcNow, task.UpdatedAtUtc);
    }

    [Fact]
    public void Create_NullTitle_ThrowsDomainValidationException()
    {
        // Arrange
        const string expected = "Title must not be empty.";

        // Act
        var ex = Assert.Throws<DomainValidationException>(() => TaskItem.Create(null!, null, UtcNow));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void Create_EmptyTitle_ThrowsDomainValidationException()
    {
        // Arrange
        const string expected = "Title must not be empty.";

        // Act
        var ex = Assert.Throws<DomainValidationException>(() => TaskItem.Create(string.Empty, null, UtcNow));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void Create_WhitespaceTitle_ThrowsDomainValidationException()
    {
        // Arrange
        const string expected = "Title must not be empty.";

        // Act
        var ex = Assert.Throws<DomainValidationException>(() => TaskItem.Create("   ", null, UtcNow));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void Create_TitleLongerThan200_ThrowsDomainValidationException()
    {
        // Arrange
        string title = new('a', 201);
        const string expected = "Title must not exceed 200 characters.";

        // Act
        var ex = Assert.Throws<DomainValidationException>(() => TaskItem.Create(title, null, UtcNow));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void Create_DescriptionLongerThan2000_ThrowsDomainValidationException()
    {
        // Arrange
        string description = new('d', 2001);
        const string expected = "Description must not exceed 2000 characters.";

        // Act
        var ex = Assert.Throws<DomainValidationException>(() => TaskItem.Create("Valid title", description, UtcNow));

        // Assert
        Assert.Equal(expected, ex.Message);
    }
}
