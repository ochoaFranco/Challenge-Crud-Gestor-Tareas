using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;

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
            var existingTask = await _repository.GetTaskByTitle(taskRequestDTO.Title.ToLower().Trim());
            
            if  (existingTask != null)
                throw new DuplicatedTaskTitleException("Title must be unique");
            
            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = taskRequestDTO.Title.ToLower().Trim(),
                Description = taskRequestDTO.Description?.ToLower().Trim(),
                IsCompleted = false,
                CreatedAt = DateTime.UtcNow
            };
            
            var createdTask = await _repository.CreateTask(task);
            
            return new TaskResponseDTO
            {
                Id = createdTask.Id,
                Title = createdTask.Title,
                Description = createdTask.Description,
                IsCompleted = createdTask.IsCompleted,
                IsActive = createdTask.IsActive
            };
        }

        public async Task DeactivateTask(Guid id)
        {
            var task = await _repository.GetTaskById(id);
            if (task is null)
                throw new KeyNotFoundException("Task does not exist");
            
            task.IsActive = false;

            await _repository.DeactivateTask(task);
        }

        public async Task<TaskResponseDTO?> GetTaskById(Guid id)
        {
            var task = await _repository.GetTaskById(id);
            if (task is null)
                throw new KeyNotFoundException("The task with the provided ID does not exist");

            return new TaskResponseDTO
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                IsCompleted = task.IsCompleted,
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
                    IsActive = t.IsActive
                }).ToListAsync();
        }

        public async Task<TaskResponseDTO> UpdateTask(Guid id, TaskRequestDTO taskRequestDTO)
        {
            var exists = await _repository.GetTaskById(id) ?? throw new KeyNotFoundException("Task does not exist");
            var isDuplicatedTitle = await _repository.GetTaskByTitle(taskRequestDTO.Title.ToLower().Trim());
            
            if (isDuplicatedTitle != null && isDuplicatedTitle.Id != exists.Id)
                throw new DuplicatedTaskTitleException("Title must be unique");

            exists.Title = taskRequestDTO.Title.ToLower().Trim();
            exists.Description = taskRequestDTO.Description?.ToLower().Trim(); 
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
                IsActive = updatedTask.IsActive
            };
        }
    }
}