using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Tests.Tasks;

internal static class TestData
{
    public static readonly DateTime UtcNow = new(2026, 1, 1, 12, 0, 0, DateTimeKind.Utc);
    public static readonly DateTime LaterUtc = new(2026, 1, 2, 12, 0, 0, DateTimeKind.Utc);

    public static TaskItem CreateTodoTask()
    {
        return TaskItem.Create("Test", null, UtcNow);
    }

    public static TaskItem CreateInProgressTask()
    {
        var task = CreateTodoTask();
        task.Start(LaterUtc);
        return task;
    }

    public static TaskItem CreateDoneTask()
    {
        var task = CreateInProgressTask();
        task.Complete(LaterUtc);
        return task;
    }
}
