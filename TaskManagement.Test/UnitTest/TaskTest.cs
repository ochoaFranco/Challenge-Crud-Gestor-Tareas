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

        [Fact]
        public async Task UpdateTask_WithDuplicateTitle_ShouldThrowException()
        {
            // Arrange
            var taskRequest = _fixture.Build<TaskRequestDTO>()
                .With(x => x.Title, "test1")
                .With(x => x.Description, "This is just a test")
                .Create();

            var normalizedTitle = taskRequest.Title.ToLower().Trim();

            var updatedTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = normalizedTitle,
                Description = taskRequest.Description,
            };

            var existingTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = normalizedTitle,
                Description = "Existing description"
            };

            // Mocking repo behavior
            _taskRepository
                .Setup(r => r.GetTaskById(updatedTask.Id))
                .ReturnsAsync(updatedTask);

            _taskRepository
                .Setup(r => r.GetTaskByTitle(normalizedTitle))
                .ReturnsAsync(existingTask);

            // Act
            var result = async () => await _service.UpdateTask(updatedTask.Id, taskRequest);

            // Assert
            await result.Should().ThrowAsync<DuplicatedTaskTitleException>();

            _taskRepository.Verify(
                r => r.GetTaskById(updatedTask.Id),
                Times.Once());

            _taskRepository.Verify(
                r => r.GetTaskByTitle(normalizedTitle),
                Times.Once());

            _taskRepository.Verify(
                r => r.UpdateTask(It.IsAny<TaskItem>()),
                Times.Never);
        }

        [Fact]
        public async Task UpdateTask_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var taskRequest = _fixture.Build<TaskRequestDTO>()
                .With(x => x.Title, "test1")
                .With(x => x.Description, "This is just a test")
                .Create();

            var normalizedTitle = taskRequest.Title.ToLower().Trim();

            var updatedTask = new TaskItem
            {
                Id = Guid.NewGuid(),
                Title = normalizedTitle,
                Description = taskRequest.Description,
            };

            // Mocking repo behavior
            _taskRepository
                .Setup(r => r.GetTaskById(updatedTask.Id))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var result = async () => await _service.UpdateTask(updatedTask.Id, taskRequest);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>();

            _taskRepository.Verify(
                r => r.GetTaskById(updatedTask.Id),
                Times.Once());

            _taskRepository.Verify(
                r => r.UpdateTask(It.IsAny<TaskItem>()),
                Times.Never);
        }

        [Fact]
        public async Task GetTaskById_WithValidId_ShouldReturnTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            var task = new TaskItem
            {
                Id = taskId,
                Title = "test task",
                Description = "test description",
                IsCompleted = false,
                IsActive = true
            };

            _taskRepository
                .Setup(r => r.GetTaskById(taskId))
                .ReturnsAsync(task);

            // Act
            var result = await _service.GetTaskById(taskId);

            // Assert
            result.Should().NotBeNull();
            result.Id.Should().Be(taskId);
            result.Title.Should().Be(task.Title);
            result.Description.Should().Be(task.Description);
            result.IsCompleted.Should().Be(task.IsCompleted);
            result.IsActive.Should().Be(task.IsActive);

            _taskRepository.Verify(
                r => r.GetTaskById(taskId),
                Times.Once);
        }

        [Fact]
        public async Task GetTaskById_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _taskRepository
                .Setup(r => r.GetTaskById(taskId))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var result = async () => await _service.GetTaskById(taskId);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>();

            _taskRepository.Verify(
                r => r.GetTaskById(taskId),
                Times.Once);
        }

        [Fact]
        public async Task DeactivateTask_WithValidId_ShouldDeactivateTask()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            var task = new TaskItem
            {
                Id = taskId,
                Title = "test task",
                IsActive = true
            };

            _taskRepository
                .Setup(r => r.GetTaskById(taskId))
                .ReturnsAsync(task);

            _taskRepository
                .Setup(r => r.DeactivateTask(It.IsAny<TaskItem>()))
                .Returns(Task.CompletedTask);

            // Act
            await _service.DeactivateTask(taskId);

            // Assert
            task.IsActive.Should().BeFalse();

            _taskRepository.Verify(
                r => r.GetTaskById(taskId),
                Times.Once);

            _taskRepository.Verify(
                r => r.DeactivateTask(It.Is<TaskItem>(
                    t => t.Id == taskId && t.IsActive == false)),
                Times.Once);
        }

        [Fact]
        public async Task DeactivateTask_WithInvalidId_ShouldThrowException()
        {
            // Arrange
            var taskId = Guid.NewGuid();

            _taskRepository
                .Setup(r => r.GetTaskById(taskId))
                .ReturnsAsync((TaskItem?)null);

            // Act
            var result = async () => await _service.DeactivateTask(taskId);

            // Assert
            await result.Should().ThrowAsync<KeyNotFoundException>();

            _taskRepository.Verify(
                r => r.GetTaskById(taskId),
                Times.Once);

            _taskRepository.Verify(
                r => r.DeactivateTask(It.IsAny<TaskItem>()),
                Times.Never);
        }
    }
}