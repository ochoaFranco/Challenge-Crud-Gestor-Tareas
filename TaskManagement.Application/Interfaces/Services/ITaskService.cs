using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IList<TaskResponseDTO>> GetTasks();
        Task<TaskResponseDTO?> GetTaskById(Guid id);
        Task<TaskResponseDTO> CreateTask(TaskRequestDTO taskRequestDTO);
        Task<TaskResponseDTO> UpdateTask(Guid id, TaskRequestDTO taskRequestDTO);
        Task DeactivateTask(Guid id);
    }
}
