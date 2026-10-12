namespace PS.TaskTracker.Domain.Exceptions;

public class DomainConflictException : DomainException
{
    public DomainConflictException(string message) : base(message)
    {
    }
}
