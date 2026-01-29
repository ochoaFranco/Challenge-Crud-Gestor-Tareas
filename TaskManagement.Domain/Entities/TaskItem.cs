using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaskManagement.Domain.Entities
{
    [Table("Task")]
    public class TaskItem : BaseEntity
    {
        [Required, Column("Title")]
        public String Title { get; set; }
       
        [Column("Description")]
        public String? Description { get; set; }
        
        [Column("IsCompleted")]
        public bool? IsCompleted { get; set; }
    }
}
