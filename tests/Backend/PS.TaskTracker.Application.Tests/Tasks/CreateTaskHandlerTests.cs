using NSubstitute;
using PS.TaskTracker.Application.Abstractions;
using PS.TaskTracker.Application.Tasks.Commands;
using PS.TaskTracker.Domain;

namespace PS.TaskTracker.Application.Tests.Tasks;

public sealed class CreateTaskHandlerTests
{
    private readonly ITaskRepository _repository = Substitute.For<ITaskRepository>();
    private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();
    private readonly CreateTaskHandler _handler;

    public CreateTaskHandlerTests()
    {
        _handler = new CreateTaskHandler(_repository, _unitOfWork);
    }

    [Fact]
    public async Task Handle_ValidCommand_CreatesTaskSavesAndReturnsTodoDto()
    {
        // Arrange
        var request = new CreateTaskCommand("New task", "New description");

        // Act
        var dto = await _handler.Handle(request, CancellationToken.None);

        // Assert
        await _repository.Received(1).AddAsync(Arg.Any<TaskItem>(), Arg.Any<CancellationToken>());
        await _unitOfWork.Received(1).SaveChangesAsync(Arg.Any<CancellationToken>());
        Assert.Equal(TaskItemStatus.Todo, dto.Status);
        Assert.Equal("New task", dto.Title);
        Assert.Equal("New description", dto.Description);
    }
}
