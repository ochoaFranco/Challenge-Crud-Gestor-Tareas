using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain;

namespace TaskManagement.Application.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _repository;

        public TaskService(ITaskRepository repository)
        {
            _repository = repository;
        }

        public async Task<TaskResponseDTO> CreateTask(TaskRequestDTO taskRequestDTO)
        {
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = taskRequestDTO.Title.ToLower().Trim(),
                Description = taskRequestDTO.Description?.ToLower().Trim(),
                IsCompleted = false,
                CreatedAt = taskRequestDTO.CreatedAt
            };
            
            var createdTask = await _repository.CreateTask(task);
            
            return new TaskResponseDTO
            {
                Id = createdTask.Id,
                Title = createdTask.Title,
                Description = createdTask.Description,
                CreatedAt = DateTime.UtcNow,
                IsCompleted = createdTask.IsCompleted,
                UpdatedAt = createdTask.UpdatedAt,
                IsActive = createdTask.IsActive
            };
        }

        public async Task DeleteTask(string id)
        {
            var task = await _repository.GetTaskById(id);
            if (task is null)
                throw new KeyNotFoundException("Task does not exist");
            
            task.IsActive = false;

            await _repository.DeleteTask(task);
        }

        public async Task<TaskResponseDTO?> GetTaskById(string id)
        {
            var task = await _repository.GetTaskById(id);
            if (task is null)
                throw new KeyNotFoundException("The task was the provided ID does not exist");

            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
                IsActive = task.IsActive
            };
        }

        public async Task<IList<TaskResponseDTO>> GetTasks()
        {
            var tasks = _repository.GetTasks();

            return await tasks
                .Where(t => t.IsActive) // se trae por defecto tareas activas.
                .Select(t => new TaskResponseDTO
                {
                    Id = t.Id,
                    Title = t.Title,
                    Description = t.Description,
                    IsCompleted= t.IsCompleted,
                    CreatedAt= t.CreatedAt,
                    UpdatedAt = t.UpdatedAt,
                    IsActive = t.IsActive
                }).ToListAsync();
        }

        public async Task<TaskResponseDTO> UpdateTask(TaskRequestDTO taskRequestDTO)
        {
            if (taskRequestDTO.Id is null)
                throw new KeyNotFoundException("Id cannot be null");
            
            var exists = await _repository.GetTaskById(taskRequestDTO.Id.Value.ToString());

            if (exists is null)
                throw new KeyNotFoundException("Task does not exist");

            exists.Title = taskRequestDTO.Title;
            exists.Description = taskRequestDTO.Description;
            exists.IsCompleted = taskRequestDTO.IsCompleted;
            exists.UpdatedAt = DateTime.UtcNow;
            exists.IsActive = taskRequestDTO.IsActive;
            
            var updatedTask = await _repository.UpdateTask(exists);

            return new TaskResponseDTO
            {
                Id = updatedTask.Id,
                Title = updatedTask.Title,
                Description = updatedTask.Description,
                IsCompleted = updatedTask.IsCompleted,
                CreatedAt = updatedTask.CreatedAt,
                UpdatedAt = updatedTask.UpdatedAt,
                IsActive = updatedTask.IsActive
            };
        }
    }
}