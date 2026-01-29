using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        #region CTR
        private ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }
        #endregion

        [HttpPost]
        public async Task<IActionResult> create(TaskRequestDTO taskRequestDTO)
        {
            var task = await _service.CreateTask(taskRequestDTO);
            return Created($"api/tasks/{task}", task);
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            var tasks = await _service.GetTasks();
            return Ok(tasks);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            var task = await _service.GetTaskById(id);
            return Ok(task);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, TaskRequestDTO taskRequestDTO)
        {
            var task = await _service.UpdateTask(id, taskRequestDTO);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeactivateTask(Guid id) // soft delete
        {
            await _service.DeactivateTask(id);
            return NoContent();
        }
    }
}