using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs.Task
{
    public class TaskRequestDTO
    {
        public Guid? Id { get; set; }
        [Required]
        public String Title { get; set; }
        public String? Description { get; set; } = string.Empty;
        public bool? IsCompleted { get; set; } = false;
        public bool IsActive { get; set; }
    }
}
