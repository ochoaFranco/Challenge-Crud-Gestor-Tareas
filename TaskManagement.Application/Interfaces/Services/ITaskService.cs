using TaskManagement.Application.DTOs.Task;

namespace TaskManagement.Application.Interfaces.Services
{
    public interface ITaskService
    {
        Task<IList<TaskResponseDTO>> GetTasks();
        Task<TaskResponseDTO?> GetTaskById(string id);
        Task<TaskResponseDTO> CreateTask(TaskRequestDTO taskRequestDTO);
        Task<TaskResponseDTO> UpdateTask(TaskRequestDTO taskRequestDTO);
        Task DeleteTask(string id);
    }
}
