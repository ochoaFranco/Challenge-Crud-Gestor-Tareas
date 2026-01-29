using Microsoft.EntityFrameworkCore;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Domain.Entities;
using TaskManagement.Infraestructure.Persistence;

namespace TaskManagement.Infraestructure.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _context;

        public TaskRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TaskItem> CreateTask(TaskItem task)
        {
            _context.Tasks.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeactivateTask(TaskItem task)
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
        }

        public async Task<TaskItem?> GetTaskById(Guid id) => 
            await _context.Tasks.FirstOrDefaultAsync(t => (t.Id == id) && t.IsActive);

        public async Task<TaskItem?> GetTaskByTitle(string title) =>
            await _context.Tasks.FirstOrDefaultAsync(t => (t.Title == title));

        public IQueryable<TaskItem> GetTasks() => _context.Tasks.AsQueryable();

        public async Task<TaskItem> UpdateTask(TaskItem task) 
        {
            _context.Tasks.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}