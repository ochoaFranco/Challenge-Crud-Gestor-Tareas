using TaskManagement.Domain;

namespace TaskManagement.Application.Interfaces.Repositories
{
    public interface ITaskRepository
    {
        IQueryable<TaskItem> GetTasks();
        Task<TaskItem?> GetTaskById(string id);
        Task<TaskItem> CreateTask(TaskItem task);
        Task<TaskItem> UpdateTask(TaskItem task);
        Task DeleteTask(TaskItem task);
    }
}
