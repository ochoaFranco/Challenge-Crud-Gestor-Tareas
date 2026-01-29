namespace TaskManagement.Domain.Exceptions
{
    public class DuplicatedTaskTitleException : Exception
    {
        public DuplicatedTaskTitleException() {}

        public DuplicatedTaskTitleException(string? message) : base(message) {}
    }
}
