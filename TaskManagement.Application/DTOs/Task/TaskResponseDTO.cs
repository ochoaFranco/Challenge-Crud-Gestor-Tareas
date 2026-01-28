using System.ComponentModel.DataAnnotations;

namespace TaskManagement.Application.DTOs.Task
{
    public class TaskResponseDTO
    {
        public Guid Id { get; set; }
        public String Title { get; set; }
        public String? Description { get; set; }
        public bool? IsCompleted { get; set; }
        public DateTime CreatedAt { get; set; } 
        public DateTime UpdatedAt { get; set; }
        public bool IsActive { get; set; }
    }
}
