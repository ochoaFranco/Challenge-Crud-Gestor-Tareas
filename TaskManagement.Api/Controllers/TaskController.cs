using Microsoft.AspNetCore.Mvc;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Services;

namespace TaskManagement.Api.Controllers
{
    public class TaskController : CustomApiController
    {
        #region CTR
        private ITaskService _service;

        public TaskController(ITaskService service)
        {
            _service = service;
        }
        #endregion

        /// <summary>Creates a new task</summary>
        [HttpPost]
        [ProducesResponseType(typeof(TaskResponseDTO), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<ActionResult<TaskResponseDTO>> Create(TaskRequestDTO taskRequestDTO)
        {
            var task = await _service.CreateTask(taskRequestDTO);
            return CreatedAtAction(nameof(GetTaskById), new { id = task.Id }, task);
        }

        /// <summary>Returns all tasks.</summary>
        [HttpGet]
        [ProducesResponseType(typeof(TaskResponseDTO), StatusCodes.Status200OK)]
        public async Task<ActionResult<TaskResponseDTO>> GetTasks()
        {
            var tasks = await _service.GetTasks();
            return Ok(tasks);
        }

        /// <summary>Gets a task by its ID</summary>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(TaskResponseDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<ActionResult<TaskResponseDTO>> GetTaskById(Guid id)
        {
            var task = await _service.GetTaskById(id);
            return Ok(task);
        }

        /// <summary>Updates an existing task.</summary>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> UpdateTask(Guid id, TaskRequestDTO taskRequestDTO)
        {
            var task = await _service.UpdateTask(id, taskRequestDTO);
            return NoContent();
        }
        /// <summary>Deletes a task by its ID.</summary>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ValidationProblemDetails), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeactivateTask(Guid id) // soft delete
        {
            await _service.DeactivateTask(id);
            return NoContent();
        }
    }
}