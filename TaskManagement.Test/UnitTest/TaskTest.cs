using AutoFixture;
using FluentAssertions;
using Moq;
using TaskManagement.Application.DTOs.Task;
using TaskManagement.Application.Interfaces.Repositories;
using TaskManagement.Application.Interfaces.Services;
using TaskManagement.Application.Services;
using TaskManagement.Domain.Entities;
using TaskManagement.Domain.Exceptions;

namespace TaskManagement.Test.UnitTest
{
    public class TaskTest
    {
        private readonly ITaskService _service;
        private readonly IFixture _fixture;
        private readonly Mock<ITaskRepository> _taskRepository;

        public TaskTest()
        {
            _fixture = new Fixture();
            _taskRepository = new Mock<ITaskRepository>();
            _service = new TaskService(_taskRepository.Object);
        }

        [Fact]
        public async Task CreateTask_WithValidData_ShouldBeSuccessfull()
        {
            // Arrange
            var taskRequest = _fixture.Build<TaskRequestDTO>()
                .With(x => x.Title, "Test")
                .With(x => x.Description, "This is just a test")
                .Create();

            var task = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = taskRequest.Title,
                Description = taskRequest.Description,
                CreatedAt = DateTime.UtcNow
            };

            _taskRepository
                .Setup(temp => temp.CreateTask(It.IsAny<TaskItem>()))
                .ReturnsAsync(task);

            // Act
            var result = await _service.CreateTask(taskRequest);

            // Assert
            result.Id.Should().Be(task.Id);
            result.Title.Should().Be(taskRequest.Title);

            _taskRepository.Verify(
                r => r.CreateTask(It.IsAny<TaskItem>()),
                Times.Once
                );
        }

        [Fact]
        public async Task CreateTask_WhenTitleAlreadyExists_ShouldThrowException()
        {
            // Arrange
            var taskRequest = _fixture.Build<TaskRequestDTO>()
                .With(x => x.Title, "Test")
                .Create();

            var existingTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = taskRequest.Title,
                CreatedAt = DateTime.UtcNow
            };

            _taskRepository
                .Setup(temp => temp.GetTaskByTitle(taskRequest.Title.ToLower().Trim()))
                .ReturnsAsync(existingTask);

            // Act
            var result = async () => await _service.CreateTask(taskRequest);

            // Assert
            await result.Should().ThrowAsync<DuplicatedTaskTitleException>();

            _taskRepository.Verify(
                r => r.CreateTask(It.IsAny<TaskItem>()),
                Times.Never
                );
        }

        [Fact]
        public async Task UpdateTask_WithValidData_ShouldBeSuccessfull()
        {
            // Arrange
            var taskRequest = _fixture.Build<TaskRequestDTO>()
                .With(x => x.Title, "Updated task title")
                .With(x => x.Description, "This is just a test")
                .Create();
            var normalizedTitle = taskRequest.Title.ToLower().Trim();
            var existingTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = "Existing task",
                Description = "Existing description"
            };

            var updatedTask = new TaskItem
            {
                Id = existingTask.Id,
                Title = normalizedTitle,
                Description = existingTask.Description,
            };
            
            // Mocking repo behavior
            _taskRepository
                .Setup(r => r.GetTaskById(existingTask.Id))
                .ReturnsAsync(existingTask);
            
            _taskRepository
                .Setup(r => r.GetTaskByTitle(normalizedTitle))
                .ReturnsAsync((TaskItem?)null);

            _taskRepository
                .Setup(r => r.UpdateTask(It.IsAny<TaskItem>()))
                .ReturnsAsync(updatedTask);

            // Act
            var result = await _service.UpdateTask(existingTask.Id,taskRequest);

            // Assert
            result.Id.Should().Be(existingTask.Id);
            result.Title.Should().Be(normalizedTitle);

            _taskRepository.Verify(
                r => r.GetTaskById(existingTask.Id),
                Times.Once());

            _taskRepository.Verify(
                r => r.GetTaskByTitle(normalizedTitle),
                Times.Once());

            _taskRepository.Verify(
                r => r.UpdateTask(It.Is<TaskItem>(
                    t => t.Id == existingTask.Id &&
                    t.Title == normalizedTitle)),
                Times.Once());
        }

    }
}
