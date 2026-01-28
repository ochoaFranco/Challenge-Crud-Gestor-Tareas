using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Api.Controllers
{
    [Route("api/tasks")]
    [ApiController]
    public class TaskController : ControllerBase
    {
        private ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<IActionResult> create(TaskRequestDTO taskRequestDTO)
        {
            try
            {
                var task = await _service.CreateTask(taskRequestDTO);
                return Created($"api/tasks/{task}", task);
            }
            catch (Exception)
            {
                return BadRequest();
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetTasks()
        {
            try
            {
                var tasks = await _service.GetTasks();
                return Ok(tasks);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTaskById(Guid id)
        {
            try
            {
                var task = await _service.GetTaskById(id.ToString());
                return Ok(task);
            }
            catch
            {
                return NotFound();
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTask(Guid id, TaskRequestDTO taskRequestDTO)
        {
            try
            {
                if (!taskRequestDTO.Id.HasValue || id != taskRequestDTO.Id.Value)
                    return BadRequest();

                var task = await _service.UpdateTask(taskRequestDTO);
               return NoContent();
            }
            catch
            {
                return BadRequest();
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(Guid id) // soft delete
        {
            try
            {
                await _service.DeleteTask(id.ToString());
                return NoContent();
            }
            catch (Exception)
            {
                return NotFound();
            }
        }
    }
}