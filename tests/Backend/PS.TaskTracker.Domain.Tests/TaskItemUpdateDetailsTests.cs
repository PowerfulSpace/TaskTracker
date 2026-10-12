using PS.TaskTracker.Domain;
using PS.TaskTracker.Domain.Exceptions;

namespace PS.TaskTracker.Domain.Tests;

public class TaskItemUpdateDetailsTests
{
    private static readonly DateTime UtcNow = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    private static readonly DateTime LaterUtc = new(2026, 1, 2, 12, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void UpdateDetails_ValidInput_UpdatesTitleDescriptionAndUpdatedAtUtc()
    {
        // Arrange
        var task = TaskItem.Create("Old title", "Old description", UtcNow);
        task.Start(LaterUtc);
        const string newTitle = "New title";
        const string newDescription = "New description";
        var evenLaterUtc = new DateTime(2026, 1, 3, 12, 0, 0, DateTimeKind.Utc);

        // Act
        task.UpdateDetails(newTitle, newDescription, evenLaterUtc);

        // Assert
        Assert.Equal(newTitle, task.Title);
        Assert.Equal(newDescription, task.Description);
        Assert.Equal(evenLaterUtc, task.UpdatedAtUtc);
        Assert.Equal(TaskItemStatus.InProgress, task.Status);
        Assert.Equal(UtcNow, task.CreatedAtUtc);
    }

    [Fact]
    public void UpdateDetails_EmptyTitle_ThrowsDomainValidationException()
    {
        // Arrange
        var task = TaskItem.Create("Old title", null, UtcNow);
        const string expected = "Title must not be empty.";

        // Act
        var ex = Assert.Throws<DomainValidationException>(() => task.UpdateDetails(string.Empty, null, LaterUtc));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void UpdateDetails_TitleLongerThan200_ThrowsDomainValidationException()
    {
        // Arrange
        var task = TaskItem.Create("Old title", null, UtcNow);
        string title = new('a', 201);
        const string expected = "Title must not exceed 200 characters.";

        // Act
        var ex = Assert.Throws<DomainValidationException>(() => task.UpdateDetails(title, null, LaterUtc));

        // Assert
        Assert.Equal(expected, ex.Message);
    }

    [Fact]
    public void UpdateDetails_DescriptionLongerThan2000_ThrowsDomainValidationException()
    {
        // Arrange
        var task = TaskItem.Create("Old title", null, UtcNow);
        string description = new('d', 2001);
        const string expected = "Description must not exceed 2000 characters.";

        // Act
        var ex = Assert.Throws<DomainValidationException>(() => task.UpdateDetails("New title", description, LaterUtc));

        // Assert
        Assert.Equal(expected, ex.Message);
    }
}
